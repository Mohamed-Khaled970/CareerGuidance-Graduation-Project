namespace CareerGuidance.Api.DTO
{
    public record ScheduleInterviewRequest
    (
        string Email,
        DateTime ScheduledDate,
        string MeetingLink
    );
    
}
