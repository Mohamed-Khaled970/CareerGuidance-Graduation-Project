namespace CareerGuidance.Api.Models
{
    public class NewCarouselSection
    {
        public Guid Id { get; set; }

        public string newCarouselSection { get; set; } = string.Empty;

        public List<DetailsCarouselSection> DetailsCarouselSection { get; set; } = new List<DetailsCarouselSection>();

    }
} 
