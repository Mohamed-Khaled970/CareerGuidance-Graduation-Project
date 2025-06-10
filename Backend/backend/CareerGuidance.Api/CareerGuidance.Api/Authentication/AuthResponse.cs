namespace CareerGuidance.Api.Authentication
{
    /*
     * File Name: AuthResponse.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-19
     * Versions Information: v1.0
     * Dependencies: None
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     *
     * Description:
     *      This record represents the response returned after a successful authentication.
     *      It contains essential information about the authenticated user and the JWT token
     *      provided for further authorization.
     */
    public record AuthResponse
     (
         string Id, // Unique identifier for the user
         string? Email, // Email address of the user (nullable)
      // string Name, // Full name of the user
         string UserName, // Username of the user
         string Token, // JWT token for authorization
         string Role, // Role assigned to the user (e.g., "Student", "Instructor")
         int ExpiresIn, // Token expiration time in minutes
         string RefreshToken,
         DateTime RefreshTokenExpiration
     );
}
