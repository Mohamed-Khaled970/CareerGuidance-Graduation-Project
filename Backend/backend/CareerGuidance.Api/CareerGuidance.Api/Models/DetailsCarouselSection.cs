using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CareerGuidance.Api.Models
{
    public class DetailsCarouselSection
    {
        public Guid Id { get; set; }

        public string carouselSection { get; set; } = string.Empty;
        public string? carouselState { get; set; } = string.Empty;
        public string carouselTitle { get; set; } = string.Empty;
        public string carouselDes { get; set; } = string.Empty;
        public string carouselImg { get; set; } = string.Empty;
        public string carouselUrl { get; set; } = string.Empty;


        [ForeignKey("CarouselSection")]
        public Guid? CarouselSectionId { get; set; } //FK


        [JsonIgnore] //have two name are equals
        public NewCarouselSection? CarouselSection { get; set; }

    }
}
