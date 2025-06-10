namespace CareerGuidance.Api.DTO
{
    public record UpdateInterviewerProfileRequest
    (
        string UserName,
        string Name,
        string Email,
        string ImageUrl,
        string About,
        string Instagram,
        string Facebook,
        string LinkedIn,
        string Github

    );
}
