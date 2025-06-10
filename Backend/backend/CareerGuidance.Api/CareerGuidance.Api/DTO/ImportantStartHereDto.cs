namespace CareerGuidance.Api.DTO
{
    public class ImportantStartHereDto
    {


        public string _startHereImportanceTitle = string.Empty;
        [Required]
        [MinLength(4, ErrorMessage = "startHerImportanceTitle must be at least 4 characters .")]
        public string startHereImportanceTitle
        {
            get { return _startHereImportanceTitle; } set { _startHereImportanceTitle = value?.Trim() ?? string.Empty; }
        }


        public string _startHereImportanceDes = string.Empty;
        [Required]
        [MinLength(4, ErrorMessage = "startHereImportanceDes must be at least 4 characters .")]
        public string startHereImportanceDes 
        { 
            get{return _startHereImportanceDes;} set{_startHereImportanceDes = value?.Trim() ?? string.Empty;} 
        } 


    }
}
 