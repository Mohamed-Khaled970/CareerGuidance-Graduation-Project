namespace CareerGuidance.Api.DTO
{
    public record ApplyInterviewRequest
    (
        int InterviewId,
        string UserId,
        IFormFile CVFile
    );
}
