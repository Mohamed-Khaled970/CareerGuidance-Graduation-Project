
namespace CareerGuidance.Api.Authentication
{
    /*
    * File Name: JwtProvider.cs
    * Author Information: Mohamed Khaled
    * Date of creation: 2024-08-19
    * Versions Information: v1.0
    * Dependencies:
    *      - System.IdentityModel.Tokens.Jwt
    *      - System.Security.Claims
    *      - System.Security.Cryptography
    *      - System.Text
    *      - Microsoft.IdentityModel.Tokens
    * Contributors: Mohamed Khaled
    * Last Modified Date: 2024-08-19
    *
    * Description:
    *      This class implements the IJwtProvider interface to provide functionality for generating
    *      JWT tokens. It creates a token based on the provided ApplicationUser details and
    *      returns the token along with its expiration time.
    */
    public class JwtProvider : IJwtProvider
    {

/*
 * Method: GenerateToken
 * 
 * Description:
 *      Generates a JWT token for the specified user.
 * 
 * Parameters:
 *      ApplicationUser user - The user for whom the token is being generated.
 * 
 * Returns:
 *      (string token, int expiresIn) - A tuple containing the generated token and the expiration time (in minutes).
 */
        public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {
            // Define the claims to include in the JWT token
            Claim[] claims =
            {
                new(JwtRegisteredClaimNames.Sub, user.Id), // Subject (user ID)
                new(JwtRegisteredClaimNames.Email, user.Email!), // Email
              //  new(JwtRegisteredClaimNames.GivenName, user.Name), // Name
                new(JwtRegisteredClaimNames.UniqueName, user.UserName!), // Username
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
                new(ClaimTypes.Role, user.Role) // User role
            };

            // Generate the key that will be used to encrypt the token
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6ksVC2PjX5wkenicens4ydUmGHKitRiT"));

            // Define signing credentials using the security key and the HMAC-SHA256 algorithm
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Define the token expiration time in minutes
            var expiresIn = 30;

            // Calculate the expiration date of the token
            var expirationDate = DateTime.UtcNow.AddMinutes(expiresIn);

            // Create the JWT token
            var token = new JwtSecurityToken
            (
                issuer: "CareerGuidanceApp", // Token issuer
                audience: "CareerGuidanceApp users", // Token audience
                claims: claims, // Claims to include in the token
                expires: expirationDate, // Token expiration date
                signingCredentials: signingCredentials // Signing credentials
            );

            // Convert the token to a string and return it along with the expiration time
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn);
        }

        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6ksVC2PjX5wkenicens4ydUmGHKitRiT"));

            try 
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken ValidateToken);

                var jwtToken = (JwtSecurityToken)ValidateToken;

              return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
