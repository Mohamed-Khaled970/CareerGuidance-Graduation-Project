namespace CareerGuidance.Api.Models
{
    public class Interview
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string InterviewerId { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ApplicationUser Interviewer { get; set; } = default!;
        public ICollection<InterviewApplication> Applications { get; set; } = [];

    }
}
