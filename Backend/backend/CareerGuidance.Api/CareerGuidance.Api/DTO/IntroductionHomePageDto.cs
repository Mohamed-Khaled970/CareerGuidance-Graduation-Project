namespace CareerGuidance.Api.DTO
{
    public class IntroductionHomePageDto
    {
        private string _description = string.Empty;

        [Required]
        [MinLength(4, ErrorMessage = "description must be at least 4 characters .")]
        public string description
        {  get { return _description; } set { _description = value?.Trim() ?? string.Empty ; }  }
         
    }
}
