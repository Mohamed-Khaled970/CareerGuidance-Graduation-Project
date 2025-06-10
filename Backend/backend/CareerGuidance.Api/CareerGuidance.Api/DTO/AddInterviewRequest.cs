namespace CareerGuidance.Api.DTO
{
    public record AddInterviewRequest
    (
         string InterviewerId,
         string Title,
         string Description,
         int Duration
    );
}
