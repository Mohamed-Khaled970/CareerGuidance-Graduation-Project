namespace CareerGuidance.Api.DTO
{
    public record GetAllInterviewsResponse
    (
        int InterviewId,
        string Title,
        string Description,
        string InterviewerId,
        string InterviewerUserName,
        string InterviewerImage
    );
}
