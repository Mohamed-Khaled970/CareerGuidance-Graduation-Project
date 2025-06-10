using CareerGuidance.Api.Abstractions;

namespace CareerGuidance.Api.Services
{
    /*
     * File Name: IAuthService.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies: 
     *      - using System.Threading;
     *      - using System.Threading.Tasks;
     *      - using System.Collections.Generic;
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     */

    public interface IAuthService
    {
        /*
         * Method: GetTokenAsync
         * Description: Retrieves an authentication token for a user based on their email and password.
         * Parameters:
         *      - email: User's email address.
         *      - password: User's password.
         *      - cancellationToken: Optional cancellation token for the operation.
         * Returns: 
         *      - AuthResponse?: An asynchronous operation that returns an authentication response if successful.
         */
        Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

        /*
         * Method: RegisterUser
         * Description: Registers a new user with the provided registration data.
         * Parameters:
         *      - request: Registration details for the user.
         *      - cancellationToken: Optional cancellation token for the operation.
         * Returns:
         *      - (bool IsSuccessful, IEnumerable<string> Errors): A tuple indicating success and a list of errors if any.
         */
        Task<Result<AuthResponse>> RegisterUser(RegisterUserDto request, CancellationToken cancellationToken);
        Task<Result> SendResetPasswordCodeAsync(string email);
        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
        Task<Result<AuthResponse>> GoogleSignUp(GoogleSignUpRequest request, CancellationToken cancellationToken = default);

    }
}
