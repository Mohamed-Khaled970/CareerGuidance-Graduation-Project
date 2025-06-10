namespace CareerGuidance.Api.DTO
{
    public record UpdateAdminProfileRequest
    (
        string UserName,
        string Name,
        string Email,
        string ImageUrl,
        string Instagram,
        string Facebook,
        string LinkedIn,
        string Github
    );
}
