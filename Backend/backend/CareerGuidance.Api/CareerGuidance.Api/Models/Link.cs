namespace CareerGuidance.Api.Models
{
    public class Link
    {
        [Key]
        public Guid Id { get; set; }

        public string Type { get; set; } = string.Empty;
        public string EnOrAr { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public Guid NodeId { get; set; } // مفتاح خارجي لـ Node
        public Node Node { get; set; }
    }
}
