namespace CareerGuidance.Api.Models
{
    public class socialMedia
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Instagram { get; set; } = string.Empty;
        public string? Facebook { get; set; } = string.Empty;
        public string? LinkedIn { get; set; } = string.Empty;
        public string? Github { get; set; } = string.Empty;

        public string ApplicationUserId { get; set; } // Foreign Key
        public ApplicationUser ApplicationUser { get; set; } // Navigation Property

    }

}
