namespace CareerGuidance.Api.DTO
{
    public record UpdateInterviewRequest(
        int InterviewId,
        string InterviewerId,
        string Title,
        string Description,
        int Duration
    );
}
