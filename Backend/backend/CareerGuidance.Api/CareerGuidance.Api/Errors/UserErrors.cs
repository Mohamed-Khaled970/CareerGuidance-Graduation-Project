using CareerGuidance.Api.Abstractions;

namespace CareerGuidance.Api.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials =
            new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidJwtToken =
               new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidRefreshToken =
            new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
        public static readonly Error DublicatedEmail =
         new("User.DublicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);
        public static readonly Error EmailNotConfirmed =
          new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidCode =
         new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedConfirmation =
          new("User.DuplicatedInformation", "Email Already Confirmed", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidGoogleIdToken =
          new("User.InvalidGoogleIdToken", "Failed to get a response.", StatusCodes.Status400BadRequest);

        public static readonly Error FaildToCreatUserByGoogle =
          new("User.FaildToCreatUser", "Faild To CreatUser", StatusCodes.Status400BadRequest);

        public static readonly Error NoUsersFound =
          new("User.NoUsersFound", "There is no Users Right now", StatusCodes.Status400BadRequest);

        public static readonly Error UserNotFound =
             new("User.UserNotFound", "This User Is Not Found", StatusCodes.Status404NotFound);

        public static readonly Error InterviewerNotFound =
     new("User.InterviewerNotFound", "This Interviewer Is Not Found", StatusCodes.Status404NotFound);

        public static readonly Error DublicatedUserName =
         new("User.DublicatedUserName", "Another User Has the same UserName", StatusCodes.Status400BadRequest);

        public static readonly Error RefusedUser =
        new("User.RefusedUser", "this user can not be deleted", StatusCodes.Status400BadRequest);
        public static readonly Error FailedToUpdateProfile =
       new("User.FailedToUpdateProfile", "Failed to update admin profile.", StatusCodes.Status400BadRequest);

        public static readonly Error UnauthorizedAccess =
  new("User.UnauthorizedAccess", "You are not allowed to Access", StatusCodes.Status403Forbidden);
    }
}
