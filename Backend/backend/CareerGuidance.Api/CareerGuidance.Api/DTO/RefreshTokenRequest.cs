using Microsoft.Extensions.Primitives;

namespace CareerGuidance.Api.DTO
{
    public record RefreshTokenRequest
   (
        string Token ,
        string RefreshToken
   );
}
