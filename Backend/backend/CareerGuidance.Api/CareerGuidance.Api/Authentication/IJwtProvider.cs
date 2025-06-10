namespace CareerGuidance.Api.Authentication
{
    /*
 * File Name: IJwtProvider.cs
 * Author Information: Mohamed Khaled
 * Date of creation: 2024-08-19
 * Versions Information: v1.0
 * Dependencies:
 *      - None
 * Contributors: Mohamed Khaled
 * Last Modified Date: 2024-08-19
 *
 * Description:
 *      This interface defines the contract for JWT token generation in the application.
 *      It includes a method to generate a JWT token for a given user.
 */
    public interface IJwtProvider
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
         *      (string token, int expiresIn) - A tuple containing the generated token and the expiration time (in seconds).
         */
        (string token, int expiresIn) GenerateToken (ApplicationUser user);

        string? ValidateToken(string token);
    }
}
