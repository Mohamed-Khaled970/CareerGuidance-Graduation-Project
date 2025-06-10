namespace CareerGuidance.Api.Models
{
    public class InterviewApplication
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; } = string.Empty;
        public ApplicationUser Applicant { get; set; } = default!;
        public int InterviewId { get; set; }
        public Interview Interview { get; set; } = default!;
        public string CvFilePath { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime? ScheduledDate { get; set; }
        public string? MeetingLink { get; set; }
    }
}
