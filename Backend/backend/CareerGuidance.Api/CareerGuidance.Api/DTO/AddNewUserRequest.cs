namespace CareerGuidance.Api.DTO
{
    public record AddNewUserRequest
    (
    string UserName,
    string Email,
    string Password,
    string Role
    );
}
