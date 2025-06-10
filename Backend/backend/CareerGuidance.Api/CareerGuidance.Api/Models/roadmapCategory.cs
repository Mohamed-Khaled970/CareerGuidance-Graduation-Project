namespace CareerGuidance.Api.Models
{
    public class roadmapCategory
    {
        public Guid Id { get; set; }

        public string Category { get; set; }

        public  List<allRoadmapInserted> Roadmaps { get; set; } = new List<allRoadmapInserted>();
        public  List<Roadmap> ParsedRoadmap { get; set; } = new List<Roadmap>();

    }
}