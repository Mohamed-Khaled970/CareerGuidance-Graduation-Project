namespace CareerGuidance.Api.Models
{
    public class NewCarouselSectionDto
    {

        private string _newCarouselSection = string.Empty;
        //[Required]
        //[MinLength(3, ErrorMessage = "TypeRoadmap must be at least 3 characters .")]
        public string newCarouselSection //For Call Only  
        {
            get { return _newCarouselSection; } set { _newCarouselSection = value?.Trim() ?? string.Empty ; }
        }



    }
} 
