namespace CareerGuidance.Api.Models
{
    public class Edge
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // معرف فريد
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;

        public Guid RoadmapId { get; set; } // مفتاح خارجي لـ Roadmap
        public Roadmap Roadmap { get; set; }
    }
}
