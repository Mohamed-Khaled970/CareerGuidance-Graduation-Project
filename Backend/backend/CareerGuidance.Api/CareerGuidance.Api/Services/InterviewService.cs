
using CareerGuidance.Api.Helpers;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace CareerGuidance.Api.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public InterviewService(
            ApplicationDbContext context,
            IFileService fileService,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<Result> AddInterviewAsync(AddInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var interviewer = await _userManager.FindByIdAsync(request.InterviewerId);
            if (interviewer is null)
            {
                return Result.Failuer(UserErrors.InterviewerNotFound);
            }

            var isDuplicated = await _context.Interviews
                      .AnyAsync(i =>
                       i.InterviewerId == request.InterviewerId &&
                       i.Title.ToLower() == request.Title.ToLower() &&
                      !i.IsDeleted,
                       cancellationToken);

            if (isDuplicated)
            {
                return Result.Failuer(InterviewError.DuplicatedTitle);
            }
            var interview = request.Adapt<Interview>();
             _context.Interviews.Add(interview);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> ApplyInterviewAsync(ApplyInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return Result.Failuer(UserErrors.UserNotFound);
            }

            var alreadyApplied = await _context.InterviewApplications
                 .AnyAsync(a => a.ApplicantId == request.UserId && a.InterviewId == request.InterviewId, cancellationToken);

            if (alreadyApplied)
            {
                return Result.Failuer(InterviewError.DuplicatedApplication);
            }

            var cvFileName = await _fileService.SaveFileAsync(request.CVFile);

            var application = new InterviewApplication
            {
                ApplicantId = request.UserId,
                InterviewId = request.InterviewId,
                CvFilePath = cvFileName,
                Status = "Pending",
                ScheduledDate = null,
                MeetingLink = null
            };
            var baseUrl = GetBaseUrl();

            application.CvFilePath = $"{baseUrl}/{cvFileName}";
            await _context.InterviewApplications.AddAsync(application);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateInterviewAsync(UpdateInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var interview = await _context.Interviews
                .FirstOrDefaultAsync(i => i.Id == request.InterviewId && !i.IsDeleted, cancellationToken);

            if (interview is null)
            {
                return Result.Failuer(InterviewError.InterviewNotFound);
            }

            if (interview.InterviewerId != request.InterviewerId)
            {
                return Result.Failuer(InterviewError.UnauthorizedAccess); 
            }

            var isDuplicated = await _context.Interviews
                .AnyAsync(i =>
                    i.Id != request.InterviewId &&
                    i.InterviewerId == request.InterviewerId &&
                    i.Title.ToLower() == request.Title.ToLower() &&
                    !i.IsDeleted,
                    cancellationToken);

            if (isDuplicated)
            {
                return Result.Failuer(InterviewError.DuplicatedTitle);
            }

            interview.Title = request.Title;
            interview.Description = request.Description;
            interview.Duration = request.Duration;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<InterviewResponse>>> GetInterviewsByInterviewerAsync(CancellationToken cancellationToken = default)
        {
            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer<IEnumerable<InterviewResponse>>(InterviewError.UnauthorizedAccess);

            var interviewer = await _userManager.FindByIdAsync(interviewerId);
            if (interviewer is null)
            {
                return Result.Failuer<IEnumerable<InterviewResponse>>(UserErrors.InterviewerNotFound);
            }

            var interviews = await _context.Interviews
                .Where(i => i.InterviewerId == interviewerId && !i.IsDeleted)
                .ProjectToType<InterviewResponse>()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<InterviewResponse>>(interviews);
        }

        public async Task<Result> DeleteInterviewAsync(int interviewId, CancellationToken cancellationToken = default)
        {

            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer(InterviewError.UnauthorizedAccess);

            var interview = await _context.Interviews
                        .FirstOrDefaultAsync(i => i.Id == interviewId && i.InterviewerId == interviewerId, cancellationToken);
            if (interview is null)
                return Result.Failuer(InterviewError.InterviewNotFound);

            if (interview.IsDeleted)
                return Result.Failuer(InterviewError.AlreadyDeleted);

            interview.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<InterviewApplicantResponse>>> GetApplicantsForInterviewAsync(int interviewId, CancellationToken cancellationToken = default)
        {
            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer<IEnumerable<InterviewApplicantResponse>>(InterviewError.UnauthorizedAccess);

            var interview = await _context.Interviews
               .FirstOrDefaultAsync(i => i.Id == interviewId && i.InterviewerId == interviewerId && !i.IsDeleted, cancellationToken);
            if (interview is null)
            {
                return Result.Failuer<IEnumerable<InterviewApplicantResponse>>(InterviewError.InterviewNotFound);
            }

            var applicants = await _context.InterviewApplications
             .Where(ia => ia.InterviewId == interviewId && ia.Status == "Pending")
             .Include(ia => ia.Applicant)
             .Select(ia => new InterviewApplicantResponse(
                 ia.Applicant.UserName ?? string.Empty,
                 ia.Applicant.Email ?? string.Empty,
                 ia.CvFilePath
             ))
             .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<InterviewApplicantResponse>>(applicants);
        }

        public async Task<Result> ScheduleInterviewAsync(int interviewId, ScheduleInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer(InterviewError.UnauthorizedAccess);

            var interviewer = await _userManager.FindByIdAsync(interviewerId);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.Failuer(UserErrors.UserNotFound);

            var interview = await _context.Interviews
                .FirstOrDefaultAsync(i => i.Id == interviewId && i.InterviewerId == interviewerId && !i.IsDeleted, cancellationToken);

            if (interview is null)
                return Result.Failuer(InterviewError.InterviewNotFound);

            var application = await _context.InterviewApplications
                  .Include(a => a.Applicant)
                  .FirstOrDefaultAsync(a => a.InterviewId == interviewId && a.ApplicantId == user.Id, cancellationToken);

            if (application is null)
                return Result.Failuer(InterviewError.ApplicationNotFound);
            application.ScheduledDate = request.ScheduledDate;
            application.MeetingLink = request.MeetingLink;
            application.Status = "Accepted";

            await _context.SaveChangesAsync(cancellationToken);

            var emailBody = EmailBodyBuilder.GenerateEmailBody("Interview",
       new Dictionary<string, string>
       {
            { "{{applicantName}}", user.UserName! },
            { "{{interviewerName}}", interviewer!.UserName! },
            { "{{interviewTitle}}", interview.Title },
            { "{{interviewDate}}", request.ScheduledDate.ToString("MMMM dd, yyyy") },
            { "{{interviewTime}}", request.ScheduledDate.ToString("hh:mm tt") },
            { "{{meetingLink}}", request.MeetingLink }
       });

            // إرسال البريد الإلكتروني (افتراض أن لديك خدمة إرسال البريد الإلكتروني)
            await _emailSender.SendEmailAsync(user.Email!, "✅ Devroot: Accepted Interview Application", emailBody);

            return Result.Success();

        }

        public async Task<Result> RejectApplicantAsync(int interviewId, RejectApplicantRequest request, CancellationToken cancellationToken = default)
        {
            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer(InterviewError.UnauthorizedAccess);

            var interviewer = await _userManager.FindByIdAsync(interviewerId);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.Failuer(UserErrors.UserNotFound);

            var interview = await _context.Interviews
              .FirstOrDefaultAsync(i => i.Id == interviewId && i.InterviewerId == interviewerId && !i.IsDeleted, cancellationToken);

            if (interview is null)
                return Result.Failuer(InterviewError.InterviewNotFound);

            var application = await _context.InterviewApplications
                  .Include(a => a.Applicant)
                  .FirstOrDefaultAsync(a => a.InterviewId == interviewId && a.ApplicantId == user.Id, cancellationToken);

            if (application is null)
                return Result.Failuer(InterviewError.ApplicationNotFound);

            if (application.Status == "Pending")
            {
                application.Status = "Rejected";
            }
            else if (application.Status == "Accepted")
            {
                return Result.Failuer(InterviewError.CannotRejectAcceptedApplication);
            }
            else if (application.Status == "Rejected")
            {
                return Result.Failuer(InterviewError.AlreadyRejectedApplication);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<IEnumerable<GetAllInterviewsResponse>>> GetAllInterviewesAsync(CancellationToken cancellationToken = default)
        {
            var interviews = await _context.Interviews
               .Where(i => !i.IsDeleted)
              .Select(i => new GetAllInterviewsResponse(
                        i.Id,
                        i.Title,
                        i.Description,
                        i.InterviewerId,
                        i.Interviewer.UserName ?? string.Empty,
                        i.Interviewer.ImageUrl
                    ))
               .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<GetAllInterviewsResponse>>(interviews);
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new InvalidOperationException("HttpContext is not available.");

            return $"{request.Scheme}://{request.Host}/CVs";
        }

        public async Task<Result<IEnumerable<AcceptedApplicantsResponse>>> GetAcceptedApplicantsAsync(CancellationToken cancellationToken = default)
        {
            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer<IEnumerable<AcceptedApplicantsResponse >> (InterviewError.UnauthorizedAccess);

            var interviewer = await _userManager.FindByIdAsync(interviewerId);

            var acceptedApplicants = await _context.InterviewApplications
               .Include(ia => ia.Applicant)
               .Include(ia => ia.Interview)
               .Where(ia => ia.Interview.InterviewerId == interviewerId &&
                           ia.Status == "Accepted" &&
                           !ia.Interview.IsDeleted)
               .Select(ia => new AcceptedApplicantsResponse(
                   ia.Id,
                   ia.Applicant.UserName ?? string.Empty,
                   ia.Applicant.Email ?? string.Empty,
                   ia.Interview.Title,
                   ia.ScheduledDate ?? DateTime.MinValue ,
                   ia.CvFilePath!,
                   ia.MeetingLink!
               ))
               .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<AcceptedApplicantsResponse>>(acceptedApplicants);

        }

        public async Task<Result> UpdateInterviewerProfileAsync(UpdateInterviewerProfileRequest request, CancellationToken cancellationToken = default)
        {
            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer<IEnumerable<AcceptedApplicantsResponse>>(InterviewError.UnauthorizedAccess);

            var interviewer = await _userManager.Users
                .Include(u => u.SocialMedia)
                .FirstOrDefaultAsync(u => u.Id == interviewerId, cancellationToken);

            if (interviewer is null)
                return Result.Failuer(UserErrors.UserNotFound); // optional error handling

            var isEmailTaken = await _userManager.Users
                .AnyAsync(u => u.Email == request.Email && u.Id != interviewerId, cancellationToken);

            if (isEmailTaken)
                return Result.Failuer(UserErrors.DublicatedEmail);

            // Update basic fields
            interviewer.UserName = request.UserName;
            interviewer.Email = request.Email;
            interviewer.ImageUrl = request.ImageUrl;
            interviewer.Name = request.Name;

            // Update About in InterviewerExperience
            var experience = await _context.InterviewersExperience
                .FirstOrDefaultAsync(e => e.InterviewerId == interviewerId, cancellationToken);

            if (experience is null)
            {
                experience = new InterviewerExperience
                {
                    InterviewerId = interviewerId,
                    About = request.About
                };
                _context.InterviewersExperience.Add(experience);
            }
            else
            {
                experience.About = request.About;
            }

            // Update Social Media
            if (interviewer.SocialMedia is null)
            {
                interviewer.SocialMedia = new socialMedia
                {
                    ApplicationUserId = interviewerId,
                    Instagram = request.Instagram,
                    Facebook = request.Facebook,
                    LinkedIn = request.LinkedIn,
                    Github = request.Github
                };
                _context.socialMedia.Add(interviewer.SocialMedia);
            }
            else
            {
                interviewer.SocialMedia.Instagram = request.Instagram;
                interviewer.SocialMedia.Facebook = request.Facebook;
                interviewer.SocialMedia.LinkedIn = request.LinkedIn;
                interviewer.SocialMedia.Github = request.Github;
                _context.socialMedia.Update(interviewer.SocialMedia); // optional, EF tracks it already
            }

            await _userManager.UpdateAsync(interviewer);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<InterviewerProfileResponse>> GetInterviewerProfileAsync(string interviewerId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .Include(u => u.SocialMedia)
                .FirstOrDefaultAsync(u => u.Id == interviewerId, cancellationToken);

            if (user is null || user.Role != "Instructor")
            {
                return Result.Failuer<InterviewerProfileResponse>(UserErrors.UserNotFound);
            }

            var experience = await _context.InterviewersExperience
                .FirstOrDefaultAsync(e => e.InterviewerId == interviewerId, cancellationToken);

            var profile = new InterviewerProfileResponse(
                user.UserName!,
                user.Name,
                user.Email ?? string.Empty,
                experience?.About ?? string.Empty,
                user.ImageUrl,
                user.SocialMedia?.Instagram ?? string.Empty,
                user.SocialMedia?.Facebook ?? string.Empty,
                user.SocialMedia?.LinkedIn ?? string.Empty,
                user.SocialMedia?.Github ?? string.Empty
            );

            return Result.Success(profile);
        }

        public async Task<Result> MarkApplicantAsDoneAsync(int interviewApplicationId, CancellationToken cancellationToken = default)
        {
            var interviewerId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(interviewerId))
                return Result.Failuer(InterviewError.UnauthorizedAccess);

            var interviewer = await _userManager.FindByIdAsync(interviewerId);
            if (interviewer is null)
                return Result.Failuer(InterviewError.UnauthorizedAccess);

            var application = await _context.InterviewApplications
                .Include(ia => ia.Interview)
                .FirstOrDefaultAsync(ia => ia.Id == interviewApplicationId, cancellationToken);

            if (application is null)
                return Result.Failuer(InterviewError.ApplicationNotFound);

            if (application.Interview.InterviewerId != interviewerId)
                return Result.Failuer(InterviewError.UnauthorizedAccess); 

            if (application.Status != "Accepted")
                return Result.Failuer(InterviewError.ApplicationNotAccepted);

            application.Status = "Done";
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
