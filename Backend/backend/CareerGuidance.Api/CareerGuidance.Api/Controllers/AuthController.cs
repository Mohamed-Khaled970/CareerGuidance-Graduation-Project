using CareerGuidance.Api.Abstractions;
using Microsoft.AspNetCore.Identity.Data;
using ResetPasswordRequest = CareerGuidance.Api.DTO.ResetPasswordRequest;

namespace CareerGuidance.Api.Controllers
{
    /*
     * File Name: AuthController.cs
     * Author Information: Mohamed Khaled , Abdelrahman Rezk
     * Date of creation: 2024-08-19
     * Versions Information: v1.0
     * Dependencies:
     *      - UserManager<ApplicationUser>
     *      - IAuthService
     *      - RegisterUserDto
     *      - LoginUserDto
     *      - CancellationToken
     * Contributors: Mohamed Khaled 
     * Last Modified Date: 2024-08-19
     *
     * Description:
     *      This controller handles authentication-related endpoints for user registration and login.
     *      It uses `UserManager<ApplicationUser>` for user management and `IAuthService` for authentication services.
     */
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager; // Dependency for managing users
        private readonly IAuthService authService; // Dependency for authentication services

        // Constructor to inject dependencies
        public AuthController(UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            this.userManager = userManager;
            this.authService = authService;
        }

        // Endpoint for user registration
        [HttpPost("SignUp")]
        // api endpoint
        public async Task<IActionResult> SignUpAsync(RegisterUserDto request, CancellationToken cancellationToken)
        {
            var authResult = await authService.RegisterUser(request,cancellationToken);

            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }

        // Endpoint for user login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginUserDto request, CancellationToken cancellationToken)
        {
            var authResult = await authService.GetTokenAsync(request.EmailOrUsername , request.Password,cancellationToken);

            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {

            var authResult = await authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return authResult is null
                ? BadRequest("Invalid Token")
                : Ok(authResult);
        }


        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var isRevoked = await authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return isRevoked ? Ok() : BadRequest("Operation Faild");
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var result = await authService.SendResetPasswordCodeAsync(request.Email);
            return result.IsSuccess ? Ok()
                : result.ToProblem();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await authService.ResetPasswordAsync(request);
            return result.IsSuccess ? Ok()
                : result.ToProblem();
        }
        [HttpPost("Google-Signin")]
        public async Task<IActionResult> GoogleSignUp(GoogleSignUpRequest model, CancellationToken cancellationToken)
        {
            var result = await authService.GoogleSignUp(model, cancellationToken);
            return result.IsSuccess ? Ok(result.Value)
           : result.ToProblem();

        }


    }
}
