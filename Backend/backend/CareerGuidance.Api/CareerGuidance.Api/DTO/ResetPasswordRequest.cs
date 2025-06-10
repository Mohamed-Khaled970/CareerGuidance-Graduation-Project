namespace CareerGuidance.Api.DTO
{
    public record ResetPasswordRequest
    (
        string Email,
        string Code,
        string NewPassword
    );
}
