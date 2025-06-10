using CareerGuidance.Api.Abstractions;
using CareerGuidance.Api.Abstractions.Const;
using CareerGuidance.Api.DTO.Validator;
using CareerGuidance.Api.Errors;
using CareerGuidance.Api.Helpers;
using CareerGuidance.Api.Models;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace CareerGuidance.Api.Services
{
    /*
     * File Name: AuthService.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies: 
     *      - using System.Collections.Generic;
     *      - using System.Linq;
     *      - using System.Threading;
     *      - using System.Threading.Tasks;
     *      - using Microsoft.AspNetCore.Identity;
     *      - using Microsoft.EntityFrameworkCore;
     *      - using Mapster;
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     */

    public class AuthService : IAuthService
    {
        // Inject UserManager for handling user-related operations
        private readonly UserManager<ApplicationUser> userManager;
        // Inject IJwtProvider for generating JWT tokens
        private readonly IJwtProvider jwtProvider;
        private readonly IEmailSender emailSender;
        private readonly GoogleAuthConfig _googleAuthConfig;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly int _refreshTokenExpiryDays = 14;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider, IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor, GoogleAuthConfig googleAuthConfig, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.jwtProvider = jwtProvider;
            this.emailSender = emailSender;
            this.httpContextAccessor = httpContextAccessor;
            this._googleAuthConfig = googleAuthConfig;
            this._context = context;
            _signInManager = signInManager;
        }

        /*
         * Method: GetTokenAsync
         * Description: Retrieves an authentication token for a user based on their email and password.
         * Parameters:
         *      - email: User's email address.
         *      - password: User's password.
         *      - cancellationToken: Optional cancellation token for the operation.
         * Returns: 
         *      - AuthResponse?: An asynchronous operation that returns an authentication response with token if successful.
         */
        public async Task<Result<AuthResponse>> GetTokenAsync(string emailOrUsername, string password, CancellationToken cancellationToken = default)
        {
            //ApplicationUser? user;
            // Check if the input is a valid email
            if (IsValidEmail(emailOrUsername))
            {
                if (await userManager.FindByEmailAsync(emailOrUsername) is not { } user)
                    return Result.Failuer<AuthResponse>(UserErrors.InvalidCredentials);
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    // Generate JWT token
                    var (token, expiresIn) = jwtProvider.GenerateToken(user);
                    var refreshToken = GenerateRefreshToken();
                    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                    user.RefreshTokens.Add(new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiresOn = refreshTokenExpiration
                    });

                    await userManager.UpdateAsync(user);
                    var response = new AuthResponse(user.Id, user.Email, user.UserName!, token, user.Role, expiresIn * 60, refreshToken, refreshTokenExpiration);

                    return Result.Success(response);
                }
            }
            else
            {
                // Otherwise, treat the input as a username
              ApplicationUser?  user = await userManager.FindByNameAsync(emailOrUsername);

                // Return specific error message if the username is not found
                if (user == null)
                    return Result.Failuer<AuthResponse>(UserErrors.InvalidCredentials);
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    // Generate JWT token
                    var (token, expiresIn) = jwtProvider.GenerateToken(user);
                    var refreshToken = GenerateRefreshToken();
                    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                    user.RefreshTokens.Add(new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiresOn = refreshTokenExpiration
                    });
                    await userManager.UpdateAsync(user);
                    var response = new AuthResponse(user.Id, user.Email, user.UserName!, token, user.Role, expiresIn * 60, refreshToken, refreshTokenExpiration);

                    return Result.Success(response);
                }
           
            }
            return Result.Failuer<AuthResponse>(UserErrors.InvalidCredentials);

            //// Check if the password is valid
            //var isValidPassword = await userManager.CheckPasswordAsync(user, password);

            //if (!isValidPassword)
            //    if (user == null)
            //        return Result.Failuer<AuthResponse>(UserErrors.InvalidCredentials);

            //// Generate JWT token
            //var (token, expiresIn) = jwtProvider.GenerateToken(user);
            //var refreshToken = GenerateRefreshToken();
            //var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            //user.RefreshTokens.Add(new RefreshToken
            //{
            //    Token = refreshToken,
            //    ExpiresOn = refreshTokenExpiration
            //});

            //var response = new AuthResponse(user.Id, user.Email, user.UserName!, token, user.Role, expiresIn * 60, refreshToken, refreshTokenExpiration);

            //return Result.Success(response);
        }

        /*
         * Method: RegisterUser
         * Description: Registers a new user with the provided registration data.
         * Parameters:
         *      - request: Registration details for the user.
         *      - cancellationToken: Optional cancellation token for the operation.
         * Returns:
         *      - (bool IsSuccessful, IEnumerable<string> Errors): A tuple indicating success and a list of errors if any.
         */
        public async Task<Result<AuthResponse>> RegisterUser(RegisterUserDto request, CancellationToken cancellationToken)
        {

            var EmailIsExists = await userManager.Users.AnyAsync(x => x.Email == request.Email);
            if (EmailIsExists)
            {
                return Result.Failuer<AuthResponse>(UserErrors.DublicatedEmail);
            }
            var user = request.Adapt<ApplicationUser>();

            var result =  await userManager.CreateAsync(user,request.Password);

            if (result.Succeeded) 
            {
                var (token, expiresIn) = jwtProvider.GenerateToken(user);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });
                user.Role = DefaultRole.Student;
                await userManager.AddToRoleAsync(user,DefaultRole.Student);
                await userManager.UpdateAsync(user);      

                var response =  new AuthResponse(user.Id, user.Email, user.UserName!, token, user.Role, expiresIn * 60, refreshToken, refreshTokenExpiration);

                return Result.Success(response);

            }

            var errors = result.Errors.First();

            return Result.Failuer<AuthResponse>(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
            //var errors = new List<string>();

            //// Check if the email is already in use
            //var existingEmailUser = await userManager.FindByEmailAsync(request.Email);
            //if (existingEmailUser != null)
            //{
            //    errors.Add("Email is already in use.");
            //}

            //// Check if the username is already in use
            //var existingUserNameUser = await userManager.FindByNameAsync(request.UserName);
            //if (existingUserNameUser != null)
            //{
            //    errors.Add("UserName is already in use.");
            //}

            //// Check if the phone number is already in use
            ////var existingPhoneNumberUser = await userManager.Users
            ////    .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

            ////if (existingPhoneNumberUser != null)
            ////{
            ////    errors.Add("Phone number is already in use.");
            ////}

            //if (errors.Any())
            //{
            //    return (false, errors); // Return tuple with failure and list of errors
            //}

            //// Map RegisterUserDto to ApplicationUser
            //var applicationUser = request.Adapt<ApplicationUser>();
            //// Create new user with the provided password
            //IdentityResult result = await userManager.CreateAsync(applicationUser, request.Password);

            //if (result.Succeeded)
            //{
            //    return (true, Enumerable.Empty<string>()); // Return success with no errors
            //}

            //// Add errors from IdentityResult if user creation fails
            //errors.AddRange(result.Errors.Select(e => e.Description));

            //return (false, errors); // Return failure with the list of errors
        }

        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = jwtProvider.ValidateToken(token);

            if (userId is null)
                return null;

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
                return null;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return null;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newtoken, expiresIn) = jwtProvider.GenerateToken(user);
            var newrefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newrefreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.Email, user.UserName!, newtoken, user.Role, expiresIn * 60, newrefreshToken, refreshTokenExpiration);

        }


        public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = jwtProvider.ValidateToken(token);

            if (userId is null)
                return false;

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
                return false;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(user);

            return true;
        }

        public async Task<Result> SendResetPasswordCodeAsync(string email)
        {
            if (await userManager.FindByEmailAsync(email) is not { } user)
            {
                return Result.Success();
            }
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await SendResetPasswordEmail(user, code);

            return Result.Success();

        }



        private async Task SendResetPasswordEmail(ApplicationUser user, string code)
        {
            var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",

                 new Dictionary<string, string>
                 {
                     { "{{name}}" , user.UserName!},
                         { "{{action_url}}" , $"{origin}/SetNewPassword?email={user.Email}&code={code}"}
                 }
            );

            await emailSender.SendEmailAsync(user.Email!, "✅ Devroot: Reset Password", emailBody);


        }


        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.Failuer(UserErrors.InvalidCode);




            if (IsPasswordInvalid(request.NewPassword))
                return Result.Failuer(new Error("InvalidPassword", "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.", StatusCodes.Status400BadRequest));



            IdentityResult result;

            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await userManager.ResetPasswordAsync(user, code, request.NewPassword);

            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
            }

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failuer(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));


        }

        private bool IsPasswordInvalid(string password)
        {
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                return true; // Password is invalid if it doesn't match the pattern
            }
            return false;
        }

        // modify 
        public async Task<Result<AuthResponse>> GoogleSignUp(GoogleSignUpRequest request, CancellationToken cancellationToken = default)
        {
            Payload payload;
            try
            {
                payload = await ValidateAsync(request.Token);
            }
            catch
            {
                return Result.Failuer<AuthResponse>(UserErrors.InvalidGoogleIdToken);
            }

            // تحقق من وجود المستخدم
            var user = await userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                // إنشاء مستخدم جديد إذا لم يكن موجودًا
                user = new ApplicationUser
                {
                    Name = payload.GivenName + " " + payload.FamilyName, // Setting to empty string as it's non-nullable
                    Email = payload.Email, // Setting to empty string as it's non-nullable
                    UserName = payload.Email, // Setting to empty string as it's nullable, but following your requirement 
                    PhoneNumberConfirmed = false, // Default value for boolean fields
                    TwoFactorEnabled = false, // Default value for boolean fields
                    LockoutEnabled = false, // Default value for boolean fields
                    AccessFailedCount = 0 // Default value for integer fields
                };

                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result.Failuer<AuthResponse>(UserErrors.FaildToCreatUserByGoogle);
                }

                var (token, expiresIn) = jwtProvider.GenerateToken(user);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });
                user.Role = DefaultRole.Student;
                await userManager.AddToRoleAsync(user, DefaultRole.Student);
                await userManager.UpdateAsync(user);

                var response = new AuthResponse(user.Id, user.Email, user.UserName!, token, user.Role, expiresIn * 60, refreshToken, refreshTokenExpiration);

                return Result.Success(response);
            }
            var (token2, expiresIn2) = jwtProvider.GenerateToken(user);
            var refreshToken2 = GenerateRefreshToken();
            var refreshTokenExpiration2 = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken2,
                ExpiresOn = refreshTokenExpiration2
            });
            user.Role = DefaultRole.Student;
            await userManager.AddToRoleAsync(user, DefaultRole.Student);
            await userManager.UpdateAsync(user);

            var response2 = new AuthResponse(user.Id, user.Email, user.UserName!, token2, user.Role, expiresIn2 * 60, refreshToken2, refreshTokenExpiration2);

            return Result.Success(response2);

        }
        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.(com|org|net|edu|gov|mil|co|info|us|biz|name|tech|store|blog|co.uk|io|me)$";
            return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
        }
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


    }
}
