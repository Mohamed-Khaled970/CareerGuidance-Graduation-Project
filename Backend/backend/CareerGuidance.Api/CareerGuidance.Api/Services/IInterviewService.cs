using System.Collections;

namespace CareerGuidance.Api.Services
{
    public interface IInterviewService
    {
        Task<Result> AddInterviewAsync(AddInterviewRequest request, CancellationToken cancellationToken = default);
        Task<Result> ApplyInterviewAsync(ApplyInterviewRequest request , CancellationToken cancellationToken = default);
        public Task<Result> UpdateInterviewAsync(UpdateInterviewRequest request, CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<InterviewResponse>>> GetInterviewsByInterviewerAsync(CancellationToken cancellationToken = default);
        public Task<Result> DeleteInterviewAsync(int interviewId, CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<InterviewApplicantResponse>>> GetApplicantsForInterviewAsync(int interviewId, CancellationToken cancellationToken = default);
        Task<Result> ScheduleInterviewAsync(int interviewId, ScheduleInterviewRequest request, CancellationToken cancellationToken = default);
        Task<Result> RejectApplicantAsync(int interviewId, RejectApplicantRequest request, CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<GetAllInterviewsResponse>>> GetAllInterviewesAsync(CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<AcceptedApplicantsResponse>>> GetAcceptedApplicantsAsync(CancellationToken cancellationToken = default);
        Task<Result> UpdateInterviewerProfileAsync(UpdateInterviewerProfileRequest request, CancellationToken cancellationToken = default);
        Task<Result<InterviewerProfileResponse>> GetInterviewerProfileAsync(string interviewerId, CancellationToken cancellationToken = default);
        Task<Result> MarkApplicantAsDoneAsync(int interviewApplicationId, CancellationToken cancellationToken = default);
    }
}
