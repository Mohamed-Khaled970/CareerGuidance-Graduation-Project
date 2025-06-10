namespace CareerGuidance.Api.Models
{
    public class Node
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // معرف فريد
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty; // على سبيل المثال: "x:291.5,y:193"

        public Guid RoadmapId { get; set; } // مفتاح خارجي لـ Roadmap
        public  Roadmap Roadmap { get; set; }

        public ICollection<Link> Links { get; set; } = new List<Link>();
    }
}
