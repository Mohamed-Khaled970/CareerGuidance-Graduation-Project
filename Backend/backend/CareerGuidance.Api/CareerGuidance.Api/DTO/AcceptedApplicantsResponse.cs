namespace CareerGuidance.Api.DTO
{
    public record AcceptedApplicantsResponse
    (
        int id ,
        string UserName,
        string Email,
        string InterviewTitle,
        DateTime ScheduledDate,
        string Cv,
        string MeetingLink
    );
}
