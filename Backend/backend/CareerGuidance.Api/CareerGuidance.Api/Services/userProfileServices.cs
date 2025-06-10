
﻿
using CareerGuidance.Api.Errors;
using CareerGuidance.Api.Models;
using MapsterMapper;

using System.Text.RegularExpressions;

namespace CareerGuidance.Api.Services
{

    public class userProfileServices : IuserProfileServices

    {
        private readonly UserManager<ApplicationUser> userManager;
        // Inject IJwtProvider for generating JWT tokens
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private object sharedId;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;


        public userProfileServices(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.userManager = userManager;
            this._context = context;
            mapper = _mapper;
            _passwordHasher = passwordHasher;
        }

        //public async Task<Result<IEnumerable<userprofileResponse>>> GetAllInfo(CancellationToken cancellationToken = default)
        //{
        //    var Users = await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        //    if (Users is null)
        //        return Result.Failuer<IEnumerable<userprofileResponse>>(userProfileErrors.NoInfoFound);


        //    return Result.Success(Users.Adapt<IEnumerable<userprofileResponse>>());

        //}


        public async Task<Result<IEnumerable<userprofileResponse>>> GetAllInfo(CancellationToken cancellationToken = default)
        {
            var users = await _context.Users
              .AsNoTracking()
              .Include(u => u.ProgressBars)
              .Include(u => u.SocialMedia)
              .ToListAsync(cancellationToken);


            if (users == null || users.Count == 0)
                return Result.Failuer<IEnumerable<userprofileResponse>>(userProfileErrors.NoInfoFound);

            var result = users.Select(user => new userprofileResponse(
             UserName: user.UserName,
             PasswordHash: user.PasswordHash,
             Email: user.Email,
             Name: user.Name,
             PhoneNumber: user.PhoneNumber,
             Role: user.Role,
             ImageUrl: user.ImageUrl,
             Country: user.Country,
             DateOfBirth: user.DateOfBirth,
             Instagram: user.SocialMedia?.Instagram?? string.Empty,
             Facebook: user.SocialMedia?.Facebook ?? string.Empty,
             GitHub: user.SocialMedia?.Github ?? string.Empty,
             LinkedIn: user.SocialMedia?.LinkedIn ?? string.Empty,
             Roadmaps_: user.ProgressBars != null
         ? user.ProgressBars.Select(pb => new progressBarResponse(
             roadmapName: pb.RoadmapName,
             Progress: pb.Progress
         )).ToList()
         : new List<progressBarResponse>() // لو null نرجع List فاضية
 ));


            return Result.Success(result);
        }



        public async Task<Result<userprofileResponse>> GetUserById(Guid Id, CancellationToken cancellationToken = default)
        {
            var idAsString = Id.ToString(); // Convert Guid to string since the key is string in the database.

            var user = await _context.Users
                .Include(u => u.ProgressBars)
                .Include(u => u.SocialMedia)
                .FirstOrDefaultAsync(u => u.Id == idAsString, cancellationToken);

            if (user == null)
            {
                return Result.Failuer<userprofileResponse>(UserErrors.NoUsersFound);
            }

            var userProfile = new userprofileResponse(
                UserName: user.UserName,
                PasswordHash: user.PasswordHash,
                Email: user.Email,
                Name: user.Name,
                PhoneNumber: user.PhoneNumber,
                Role: user.Role,
                ImageUrl: user.ImageUrl,
                Country: user.Country,
                DateOfBirth: user.DateOfBirth,
                Instagram: user.SocialMedia?.Instagram ?? string.Empty,
                Facebook: user.SocialMedia?.Facebook ?? string.Empty,
                GitHub: user.SocialMedia?.Github ?? string.Empty,
                LinkedIn: user.SocialMedia?.LinkedIn ?? string.Empty,
                Roadmaps_: user.ProgressBars != null
                    ? user.ProgressBars.Select(pb => new progressBarResponse(
                        roadmapName: pb.RoadmapName,
                        Progress: pb.Progress
                    )).ToList()
                    : new List<progressBarResponse>()
            );

            return Result.Success(userProfile);
        }


        //public async Task<Result<userprofileResponse>> GetUserById(Guid Id, CancellationToken cancellationToken = default)

        //{
        //    var idAsString = Id.ToString(); // Convert Guid to string since the key is string in the database.

        //    // Use LINQ to find the user.
        //    var user = await _context.Users
        //        .Where(u => u.Id == idAsString)

        //        .ProjectToType<userprofileResponse>() // Use Mapster for mapping.

        //        .FirstOrDefaultAsync(cancellationToken);

        //    if (user == null)
        //    {

        //        return Result.Failuer<userprofileResponse>(UserErrors.NoUsersFound);
        //    }

        //    return Result.Success(user.Adapt<userprofileResponse>());

        //}


        public async Task<Result> UpdateUserProfileAsync(Guid id, userprofileRequest updateRequest, CancellationToken cancellationToken)
        {
            // Fetch the user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);

            if (user == null)
            {
                return Result.Failuer(userProfileErrors.NoInfoFound);
            }

            var nameForValidation = updateRequest.Name.Trim();
            if (string.IsNullOrWhiteSpace(nameForValidation))
            {
                return Result.Failuer(userProfileErrors.NameEmpty);
            }


            if (nameForValidation.Length < 3)
            {
                return Result.Failuer(userProfileErrors.NameTooShort);
            }

            // ❌ الاسم طويل جدًا
            if (nameForValidation.Length > 30)
            {
                return Result.Failuer(userProfileErrors.NameTooLong);
            }

            // ❌ يحتوي على أرقام
            if (nameForValidation.Any(char.IsDigit))
            {
                return Result.Failuer(userProfileErrors.NameContainsNumber);
            }

            // ❌ يحتوي على رموز خاصة أو أي شيء غير حرف أو مسافة
            if (nameForValidation.Any(ch => !char.IsLetter(ch) && !char.IsWhiteSpace(ch)))
            {
                return Result.Failuer(userProfileErrors.NameContainsSpecialCharacter);
            }




            //  Validate Phone Number (only if changed)
            if (!string.IsNullOrWhiteSpace(updateRequest.PhoneNumber) && updateRequest.PhoneNumber != user.PhoneNumber)
            {
                var phoneRegex = new Regex(@"^\+20(10|11|12|15)\d{8}$");

                if (!phoneRegex.IsMatch(updateRequest.PhoneNumber))
                {
                    return Result.Failuer(userProfileErrors.InvalidPhoneNumber);
                }

                var isPhoneTaken = await _context.Users
                    .AnyAsync(u => u.PhoneNumber == updateRequest.PhoneNumber && u.Id != user.Id, cancellationToken);

                if (isPhoneTaken)
                {
                    return Result.Failuer(userProfileErrors.Istaken);
                }

                user.PhoneNumber = updateRequest.PhoneNumber;
            }


            // imageUrl 
            if (!string.IsNullOrWhiteSpace(updateRequest.ImageUrl) &&
                !updateRequest.ImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failuer(userProfileErrors.InvalidImageUrl);
            }


            // Validate Date of Birth
            var currentDate = DateTime.Now;
            int userAge = currentDate.Year - updateRequest.DateOfBirth.Year;
            if (updateRequest.DateOfBirth.Date > currentDate.AddYears(-userAge))
            {
                userAge--;
            }

            if (userAge < 9 || userAge > 55)
            {
                return Result.Failuer(userProfileErrors.InvalidDateOfBirth);
            }

            // Update User Info
            user.Name = updateRequest.Name;
            user.ImageUrl = updateRequest.ImageUrl;
            user.Country = updateRequest.Country;
            user.DateOfBirth = updateRequest.DateOfBirth;

            if (user == null || updateRequest == null)
                return Result.Failuer(userProfileErrors.NoInfoFound);

            // Validate Social Media Links
            var validationResults = new List<Result>
        {
            ValidateSocialMediaUrl(updateRequest.Instagram, "instagram", userProfileErrors.InvalidInstagramLink),
            ValidateSocialMediaUrl(updateRequest.Facebook, "facebook", userProfileErrors.InvalidFacebookLink),
            ValidateSocialMediaUrl(updateRequest.LinkedIn, "linkedin", userProfileErrors.InvalidLinkedInLink),
            ValidateSocialMediaUrl(updateRequest.GitHub, "github", userProfileErrors.InvalidGitHubLink)
        };

            var firstFailure = validationResults.FirstOrDefault(r => !r.IsSuccess);
            if (firstFailure != null)
                return firstFailure;

            var existingSocialMedia = await _context.socialMedia
                .FirstOrDefaultAsync(sm => sm.ApplicationUserId == user.Id, cancellationToken);

            if (existingSocialMedia != null)
            {
                _context.socialMedia.Remove(existingSocialMedia);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var newSocialMedia = new socialMedia
            {
                ApplicationUserId = user.Id,
                Instagram = updateRequest.Instagram,
                Facebook = updateRequest.Facebook,
                LinkedIn = updateRequest.LinkedIn,
                Github = updateRequest.GitHub
            };

            _context.socialMedia.Add(newSocialMedia);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private Result ValidateSocialMediaUrl(string? url, string platform, Error errorType)
        {
            if (string.IsNullOrWhiteSpace(url))
                return Result.Success();

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return Result.Failuer(userProfileErrors.InvalidLink);

            if (!url.Contains(platform, StringComparison.OrdinalIgnoreCase))
                return Result.Failuer(errorType);

            return Result.Success();
        }



        //    public async Task<Result> UpdateUserProfileAsync(Guid id, userprofileRequest updateRequest, CancellationToken cancellationToken)
        //    {
        //        var user = await _context.Users
        //            .FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);

        //        if (user == null || updateRequest == null)
        //            return Result.Failuer(userProfileErrors.NoInfoFound);

        //        // ✅ Name
        //        if (updateRequest.Name != null)
        //        {
        //            var nameForValidation = updateRequest.Name.Trim();

        //            if (!string.IsNullOrEmpty(nameForValidation))
        //            {
        //                if (nameForValidation.Length < 3)
        //                    return Result.Failuer(userProfileErrors.NameTooShort);

        //                if (nameForValidation.Length > 30)
        //                    return Result.Failuer(userProfileErrors.NameTooLong);

        //                if (nameForValidation.Any(char.IsDigit))
        //                    return Result.Failuer(userProfileErrors.NameContainsNumber);

        //                if (nameForValidation.Any(ch => !char.IsLetter(ch) && !char.IsWhiteSpace(ch)))
        //                    return Result.Failuer(userProfileErrors.NameContainsSpecialCharacter);
        //            }

        //            user.Name = updateRequest.Name;
        //        }

        //        // ✅ Phone Number
        //        if (updateRequest.PhoneNumber != null)
        //        {
        //            if (!string.IsNullOrWhiteSpace(updateRequest.PhoneNumber) && updateRequest.PhoneNumber != user.PhoneNumber)
        //            {
        //                var phoneRegex = new Regex(@"^\+20(10|11|12|15)\d{8}$");

        //                if (!phoneRegex.IsMatch(updateRequest.PhoneNumber))
        //                    return Result.Failuer(userProfileErrors.InvalidPhoneNumber);

        //                var isPhoneTaken = await _context.Users
        //                    .AnyAsync(u => u.PhoneNumber == updateRequest.PhoneNumber && u.Id != user.Id, cancellationToken);

        //                if (isPhoneTaken)
        //                    return Result.Failuer(userProfileErrors.Istaken);
        //            }

        //            user.PhoneNumber = updateRequest.PhoneNumber;
        //        }

        //        // ✅ Image URL
        //        if (updateRequest.ImageUrl != null)
        //        {
        //            if (!string.IsNullOrWhiteSpace(updateRequest.ImageUrl) &&
        //                !updateRequest.ImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        //            {
        //                return Result.Failuer(userProfileErrors.InvalidImageUrl);
        //            }

        //            user.ImageUrl = updateRequest.ImageUrl;
        //        }

        //        // ✅ Country
        //        if (updateRequest.Country != null)
        //        {
        //            user.Country = updateRequest.Country;
        //        }

        //        // ✅ Validate Date of Birth
        //        if (updateRequest.DateOfBirth != null)
        //        {
        //            var currentDate = DateTime.Now;
        //            int userAge = currentDate.Year - updateRequest.DateOfBirth.Year;

        //            if (updateRequest.DateOfBirth.Date > currentDate.AddYears(-userAge))
        //            {
        //                userAge--;
        //            }

        //            if (userAge < 9 || userAge > 55)
        //            {
        //                return Result.Failuer(userProfileErrors.InvalidDateOfBirth);
        //            }

        //            user.DateOfBirth = updateRequest.DateOfBirth;
        //        }

        //        // ✅ Social Media
        //        var validationResults = new List<Result>
        //{
        //    ValidateSocialMediaUrl(updateRequest.Instagram, "instagram", userProfileErrors.InvalidInstagramLink),
        //    ValidateSocialMediaUrl(updateRequest.Facebook, "facebook", userProfileErrors.InvalidFacebookLink),
        //    ValidateSocialMediaUrl(updateRequest.LinkedIn, "linkedin", userProfileErrors.InvalidLinkedInLink),
        //    ValidateSocialMediaUrl(updateRequest.GitHub, "github", userProfileErrors.InvalidGitHubLink)
        //};

        //        var firstFailure = validationResults.FirstOrDefault(r => !r.IsSuccess);
        //        if (firstFailure != null)
        //            return firstFailure;

        //        var existingSocialMedia = await _context.socialMedia
        //            .FirstOrDefaultAsync(sm => sm.ApplicationUserId == user.Id, cancellationToken);

        //        if (existingSocialMedia != null)
        //        {
        //            _context.socialMedia.Remove(existingSocialMedia);
        //            await _context.SaveChangesAsync(cancellationToken);
        //        }

        //        var newSocialMedia = new socialMedia
        //        {
        //            ApplicationUserId = user.Id,
        //            Instagram = updateRequest.Instagram,
        //            Facebook = updateRequest.Facebook,
        //            LinkedIn = updateRequest.LinkedIn,
        //            Github = updateRequest.GitHub
        //        };

        //        _context.socialMedia.Add(newSocialMedia);

        //        await _context.SaveChangesAsync(cancellationToken);
        //        return Result.Success();
        //    }

        //    private Result ValidateSocialMediaUrl(string? url, string platform, Error errorType)
        //    {
        //        if (string.IsNullOrWhiteSpace(url))
        //            return Result.Success();

        //        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        //            return Result.Failuer(errorType);

        //        if (!url.Contains(platform, StringComparison.OrdinalIgnoreCase))
        //            return Result.Failuer(errorType);

        //        return Result.Success();
        //    }


        public async Task<Result> UpdatePassword(Guid id, userPasswordRequest updateRequest, CancellationToken cancellationToken)
        {
            // Fetch the user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);

            if (user == null)
            {
                return Result.Failuer(userProfileErrors.NoInfoFound);
            }
            // Validate Password
            var oldPassword = updateRequest.OldPassword;
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return Result.Failuer(userProfileErrors.PasswordNotSet);
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, oldPassword);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Result.Failuer(userProfileErrors.InvalidOldPassword);
            }

            var password = updateRequest.NewPassword;
            var confirmPassword = updateRequest.ConfirmPassword;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                return Result.Failuer(userProfileErrors.PasswordNotSet);
            }

            if (password.Length < 8)
            {
                return Result.Failuer(userProfileErrors.PasswordTooShort);
            }

            if (!password.Any(char.IsUpper))
            {
                return Result.Failuer(userProfileErrors.MissingUppercase);
            }

            if (!password.Any(char.IsLower))
            {
                return Result.Failuer(userProfileErrors.MissingLowercase);
            }

            if (!password.Any(char.IsDigit))
            {
                return Result.Failuer(userProfileErrors.MissingNumber);
            }

            if (!password.Any(ch => "!@#$%^&*".Contains(ch)))
            {
                return Result.Failuer(userProfileErrors.MissingSpecialCharacter);
            }

            if (password != confirmPassword)
            {
                return Result.Failuer(userProfileErrors.PasswordsDoNotMatch);
            }



            var newPasswordVerification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, updateRequest.NewPassword);

            if (newPasswordVerification == PasswordVerificationResult.Success)
            {
                return Result.Failuer(userProfileErrors.IsDuplicated);
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}


