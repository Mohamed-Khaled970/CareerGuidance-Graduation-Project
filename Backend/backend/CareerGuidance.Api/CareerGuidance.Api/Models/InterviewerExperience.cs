namespace CareerGuidance.Api.Models
{
    public class InterviewerExperience
    {
        public int Id { get; set; }
        public string InterviewerId { get; set; } = string.Empty;
        public ApplicationUser Interviewer { get; set; } = default!;
        public string About { get; set; } = string.Empty;
    }
}
