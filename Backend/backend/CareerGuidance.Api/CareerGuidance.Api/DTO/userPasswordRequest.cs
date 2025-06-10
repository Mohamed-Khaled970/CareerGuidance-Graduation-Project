namespace CareerGuidance.Api.DTO
{
    public record userPasswordRequest
        (
            string OldPassword,
            string NewPassword,
            string ConfirmPassword

        );
    
}
