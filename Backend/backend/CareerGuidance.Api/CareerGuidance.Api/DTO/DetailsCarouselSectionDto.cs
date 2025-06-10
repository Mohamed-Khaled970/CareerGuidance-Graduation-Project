using System.ComponentModel.DataAnnotations;

namespace CareerGuidance.Api.Models
{
    public class DetailsCarouselSectionDto
    {
        private string _carouselSection = string.Empty;
        //[Required]
        //[MinLength(3, ErrorMessage = "carouselSection must be at least 3 characters.")]
        public string carouselSection
        {
            get { return _carouselSection; } set { _carouselSection = value?.Trim() ?? string.Empty; }
        }

        private string _carouselTitle = string.Empty;
        //[Required]
        //[MinLength(3, ErrorMessage = "carouselTitle must be at least 3 characters.")]
        public string carouselTitle
        {
            get { return _carouselTitle; } set { _carouselTitle = value?.Trim() ?? string.Empty; }
        }


        private string? _carouselState ;
        //[Required]
        //[MinLength(3, ErrorMessage = "carouselState must be at least 3 characters.")]
        public string? carouselState
        {
            get { return _carouselState; } set { _carouselState = string.IsNullOrWhiteSpace(value) ? null : value.Trim(); }

        }

        private string _carouselDes = string.Empty;
        //[Required]
        //[MinLength(3, ErrorMessage = "carouselDes must be at least 3 characters.")]
        public string carouselDes
        {
            get { return _carouselDes; } set { _carouselDes = value?.Trim() ?? string.Empty; }
        }


        private string _carouselImg = string.Empty;
        //[Required]
        //[Url(ErrorMessage ="Invalid Input , Enter Valid Url For carouselImage ")]
        public string carouselImg
        {
            get { return _carouselImg; } set { _carouselImg = value?.Trim() ?? string.Empty; }
        }


        private string _carouselUrl = string.Empty;
        //[Required]
        //[Url(ErrorMessage = "Invalid Input , Enter Valid Url For carouselUrl")]
        public string carouselUrl
        {
            get { return _carouselUrl ; } set { _carouselUrl = value?.Trim() ?? string.Empty ; }
        }

    }
}