namespace CareerGuidance.Api.Models
{
    public class progressBar
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ApplicationUserId { get; set; } // Foreign Key
        public ApplicationUser ApplicationUser { get; set; }
        public Roadmap Roadmap { get; set; } // foreign key 
        public string RoadmapName { get; set; } = string.Empty;
        public Guid RoadmapId { get; set; }
        public List<string> CompletedNodes { get; set; } = new();
        public int Progress { get; set; } // Progress bar value (0-100)

    }
}
