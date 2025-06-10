namespace CareerGuidance.Api.Models
{
    public class IntroductionStartHerePageDto  
    {


        public string _startHereIntroTitle = string.Empty;
        [Required]
        [MinLength(4, ErrorMessage = "startHereIntroTitle must be at least 4 characters .")]
        public string startHereIntroTitle
        {
            get { return _startHereIntroTitle; } set { _startHereIntroTitle = value?.Trim() ?? string.Empty; }
        }



        public string _startHereIntroDes = string.Empty;
        [Required]
        [MinLength(4, ErrorMessage = "startHereIntroDes must be at least 4 characters .")]
        public string startHereIntroDes //call only
        {
            get { return _startHereIntroDes; } set { _startHereIntroDes = value?.Trim() ?? string.Empty; }
        }

       

    }
}
