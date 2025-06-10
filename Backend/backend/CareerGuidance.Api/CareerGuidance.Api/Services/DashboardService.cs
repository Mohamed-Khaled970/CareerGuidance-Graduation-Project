using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace CareerGuidance.Api.Services
{
    public class DashboardService : IDashboardService
    {
        // Inject UserManager for handling user-related operations
        private readonly UserManager<ApplicationUser> userManager;
        // Inject IJwtProvider for generating JWT tokens
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this._context = context;
            _httpContextAccessor = httpContextAccessor;
        }
       

        public async Task<Result<IEnumerable<UsersResponse>>> GetAllUsers(CancellationToken cancellationToken = default)
        {

            var Users = await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
            if (Users is null)
                return Result.Failuer<IEnumerable<UsersResponse>>(UserErrors.NoUsersFound);


            return Result.Success(Users.Adapt<IEnumerable<UsersResponse>>());
        }

       

        public async Task<Result<userprofileResponse>> GetUserById(Guid Id, CancellationToken cancellationToken = default)
        {
            var idAsString = Id.ToString(); // Convert Guid to string since the key is string in the database.

            var user = await _context.Users
                .Include(u => u.ProgressBars)
                .Include(u => u.SocialMedia)
                .FirstOrDefaultAsync(u => u.Id == idAsString, cancellationToken);

            if (user == null)
            {
                return Result.Failuer<userprofileResponse>(UserErrors.NoUsersFound);
            }

            var userProfile = new userprofileResponse(
                UserName: user.UserName,
                PasswordHash: user.PasswordHash,
                Email: user.Email,
                Name: user.Name,
                PhoneNumber: user.PhoneNumber,
                Role: user.Role,
                ImageUrl: user.ImageUrl,
                Country: user.Country,
                DateOfBirth: user.DateOfBirth,
                Instagram: user.SocialMedia?.Instagram ?? string.Empty,
                Facebook: user.SocialMedia?.Facebook ?? string.Empty,
                GitHub: user.SocialMedia?.Github ?? string.Empty,
                LinkedIn: user.SocialMedia?.LinkedIn ?? string.Empty,
                Roadmaps_: user.ProgressBars != null
                    ? user.ProgressBars.Select(pb => new progressBarResponse(
                        roadmapName: pb.RoadmapName,
                        Progress: pb.Progress
                    )).ToList()
                    : new List<progressBarResponse>()
            );

            return Result.Success(userProfile);
        }



        public async Task<Result> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(id, cancellationToken);

            if (user is null)
            {
                return Result.Failuer(UserErrors.NoUsersFound);
            }

            if (user.UserName == "DevrootAdmin") // ← اليوزر اللي ممنوع يتحذف 
            {
                return Result.Failuer(UserErrors.RefusedUser);
            }

            _context.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();


        }

        public async Task<Result> AddNewUserAsync(AddNewUserRequest request, CancellationToken cancellationToken = default)
        {
            // note : check if the user email is already exist
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Result.Failuer(UserErrors.DublicatedEmail);
            }
            var user = request.Adapt<ApplicationUser>();

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                // عرض رسائل الخطأ في حالة فشل الإنشاء
                return Result.Failuer(UserErrors.DublicatedUserName);
            }
            return Result.Success();
        }

        public async Task<Result> AddQuestion(QuestionRequest request, CancellationToken cancellationToken = default)
        {
            // Step 1: Map the request to an FAQ entity
            var question = request.Adapt<FAQ>();

            // Validate the question
            if (!IsValidQuestion(request.Question))
            {
                return Result.Failuer(FAQErrors.InvalidQuestion);
            }

            // Validate if the question ends with a single question mark
            if (!IsValidQuestionEnd(request.Question))
            {
                return Result.Failuer(FAQErrors.InvalidQuestionEnd);
            }

            // Step 2: Normalize the input question
            var normalizedQuestion = NormalizeQuestion(request.Question);

            // Step 3: Retrieve all questions from the database (Client-side filtering)
            var existingQuestions = await _context.FAQs
                .AsNoTracking()
                .Select(f => new { f.Question })
                .ToListAsync(cancellationToken);

            // Step 4: Check if any of the existing questions match the normalized question
            var questionExists = existingQuestions
                .Any(f => NormalizeQuestion(f.Question) == normalizedQuestion);

            if (questionExists)
            {
                // Return failure if the question already exists
                return Result.Failuer(FAQErrors.DuplicatedQuestion);
            }

            // Step 4: Add the question to the DbSet if it doesn't exist
            _context.FAQs.Add(question);

            // Step 5: Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            // Step 6: Return success result
            return Result.Success("Question added successfully.");
        }

        public async Task<Result> UpdateQuestion(int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var question = await _context.FAQs.FindAsync(id, cancellationToken);
            if (question == null)
            {
                return Result.Failuer(FAQErrors.QuestionNotFound);
            }

            // Validate the new question content
            if (!IsValidQuestion(request.Question))
            {
                return Result.Failuer(FAQErrors.InvalidQuestion);
            }

            if (!IsValidQuestionEnd(request.Question))
            {
                return Result.Failuer(FAQErrors.InvalidQuestionEnd);
            }

            // Step 1: Normalize and check for duplication
            var normalizedQuestion = NormalizeQuestion(request.Question);
            var existingQuestions = await _context.FAQs
                .AsNoTracking()
                .Where(f => f.Id != id)
                .Select(f => f.Question)
                .ToListAsync(cancellationToken);

            if (existingQuestions.Any(q => NormalizeQuestion(q) == normalizedQuestion))
            {
                return Result.Failuer(FAQErrors.DuplicatedQuestion);
            }

            // Step 2: Update the question content
            question.Question = request.Question;
            question.Answer = request.Answer;

            // Save the changes
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<QuestionsResponse>>> GetAllQuestions(CancellationToken cancellationToken = default)
        {
            var Questions = await _context.FAQs.AsNoTracking().ToListAsync(cancellationToken);
            if (Questions is null)
                return Result.Failuer<IEnumerable<QuestionsResponse>>(FAQErrors.NoQuestionsFound);


            return Result.Success(Questions.Adapt<IEnumerable<QuestionsResponse>>());
        }

        public async Task<Result> DeleteQuestion(int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.FAQs.FindAsync(id, cancellationToken);

            if (question is null)
            {
                return Result.Failuer(FAQErrors.QuestionNotFound);
            }

            _context.Remove(question);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private string NormalizeQuestion(string question)
        {
            if (string.IsNullOrWhiteSpace(question)) return string.Empty;

            // إزالة علامات الترقيم وتحويل السؤال لحروف صغيرة
            string withoutPunctuation = Regex.Replace(question, @"[^\w\s]", "");

            // إزالة المسافات الزائدة وتحويل السؤال لحروف صغيرة
            return string.Join(" ", withoutPunctuation.ToLower().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        private bool IsValidQuestion(string question)
        {
            // يجب أن يكون بين 5 و 1000 حرف
            if (string.IsNullOrWhiteSpace(question) || question.Length < 5 || question.Length > 1000)
            {
                return false;
            }

            // يجب أن يحتوي على حروف وليس فقط أرقام
            if (!question.Any(char.IsLetter))
            {
                return false;
            }

            // تحقق باستخدام التعبير العادي
            string pattern = @"^[a-zA-Z].*$"; // التعبير العادي للتحقق من البداية بحرف والنهاية بعلامة استفهام واحدة
            if (!Regex.IsMatch(question.Trim(), pattern))
            {
                return false; // إذا لم يتطابق مع الشرط، يعتبر السؤال غير صالح
            }

            return true; // السؤال صالح
        }

        private bool IsValidQuestionEnd(string question)
        {
            var trimmedQuestion = question.Trim();
            return Regex.IsMatch(trimmedQuestion, @"^(.*[^?])\?$");
        }

        public async Task<Result> UpdateAdminProfile(UpdateAdminProfileRequest request, CancellationToken cancellationToken = default)
        {
            var adminId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(adminId))
                return Result.Failuer(UserErrors.UnauthorizedAccess);

            var admin = await userManager.Users
                .Include(u => u.SocialMedia)
                .FirstOrDefaultAsync(u => u.Id == adminId, cancellationToken);

            if (admin == null || admin.Role != "Admin")
                return Result.Failuer(UserErrors.UserNotFound);

            var isEmailTaken = await userManager.Users
                .AnyAsync(u => u.Email == request.Email && u.Id != adminId, cancellationToken);

            if (isEmailTaken)
            {
                return Result.Failuer(UserErrors.DublicatedEmail);
            }

            admin.UserName = request.UserName;
            admin.Name = request.Name;
            admin.Email = request.Email;
            admin.ImageUrl = request.ImageUrl;

            // تحديث Social Media
            if (admin.SocialMedia is null)
            {
                admin.SocialMedia = new socialMedia
                {
                    ApplicationUserId = admin.Id,
                    Instagram = request.Instagram,
                    Facebook = request.Facebook,
                    LinkedIn = request.LinkedIn,
                    Github = request.Github
                };
                _context.socialMedia.Add(admin.SocialMedia);
            }
            else
            {
                admin.SocialMedia.Instagram = request.Instagram;
                admin.SocialMedia.Facebook = request.Facebook;
                admin.SocialMedia.LinkedIn = request.LinkedIn;
                admin.SocialMedia.Github = request.Github;
                _context.socialMedia.Update(admin.SocialMedia);
            }

            var result = await userManager.UpdateAsync(admin);

            if (!result.Succeeded)
            {
                return Result.Failuer(UserErrors.FailedToUpdateProfile);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
