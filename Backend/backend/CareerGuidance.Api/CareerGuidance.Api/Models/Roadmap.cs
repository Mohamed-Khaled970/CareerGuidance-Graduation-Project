using System.ComponentModel.DataAnnotations.Schema;

namespace CareerGuidance.Api.Models
{
    public class Roadmap
    {

        [Key]
        public Guid Id { get; set; }  // معرف فريد
        
        [MaxLength(200)]
        public string RoadmapName { get; set; } = string.Empty;

        public string RoadmapDescription { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; } //FK

        public ICollection<progressBar> ProgressBar { get; set; } = new List<progressBar>();

        [ForeignKey("CategoryId")]
        public roadmapCategory Categories { get; set; } //Relation

        public ICollection<Node> Nodes { get; set; } = new List<Node>();
        public ICollection<Edge> Edges { get; set; } = new List<Edge>();

    }
}
