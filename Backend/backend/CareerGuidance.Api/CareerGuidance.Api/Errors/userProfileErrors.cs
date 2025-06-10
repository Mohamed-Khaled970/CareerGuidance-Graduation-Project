
﻿namespace CareerGuidance.Api.Errors
{
    public class userProfileErrors
    {
        public static readonly Error NoInfoFound =
                new("User.NoInfoFound", "There is no Information  Right now", StatusCodes.Status400BadRequest);


        public static readonly Error NoUsersFound =
             new("User.NoUsersFound", "There is no Users Right now", StatusCodes.Status400BadRequest);




        public static readonly Error IsEmpty =
                new("User.IsEmpty", "this field can not be empty", StatusCodes.Status400BadRequest);


        public static readonly Error InvalidPhoneNumber =
             new("User.InvalidPhoneNumber",
                 "Invalid phone number. It must start with +20 + => 10, 11, 12, or 15 and exactly 8 digits.",
                 StatusCodes.Status400BadRequest);



        public static readonly Error Istaken =
        new("User.PhoneNumberTaken",
            "Invalid phone number. this phone number is already exist",
            StatusCodes.Status400BadRequest);

        public static readonly Error IsDuplicated =
           new("User.Invalidpassword",
               "Invalid password. the old password can not be the new password ",
               StatusCodes.Status400BadRequest);

        public static readonly Error InvalidDateOfBirth =
                new("User.InvalidDateOfBirth",
                    "Invalid date of birth. Sorry age must be between 9 to 55 only",
                    StatusCodes.Status400BadRequest);


        public static readonly Error NameEmpty =
        new("User.NameEmpty", "Name cannot be empty.", StatusCodes.Status400BadRequest);

        public static readonly Error NameTooShort =
        new("User.NameTooShort", "Name must be at least 3 characters long.", StatusCodes.Status400BadRequest);

        public static readonly Error NameTooLong =
            new("User.NameTooLong", "Name must be less than 30 characters.", StatusCodes.Status400BadRequest);

        public static readonly Error NameContainsNumber =
            new("User.NameContainsNumber", "Name must not contain numbers.", StatusCodes.Status400BadRequest);

        public static readonly Error NameContainsSpecialCharacter =
            new("User.NameContainsSpecialCharacter", "Name must not contain special characters.", StatusCodes.Status400BadRequest);



        public static readonly Error PhoneNumberEmpty =
            new("User.PhoneNumberEmpty", "Phone number cannot be empty.", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidImageUrl =
        new("User.InvalidImageUrl", "Image URL is not valid it must start with https.", StatusCodes.Status400BadRequest);

        public static readonly Error ImageUrlEmpty =
            new("User.ImageUrlEmpty", "Image URL cannot be empty.", StatusCodes.Status400BadRequest);


        public static readonly Error CountryEmpty =
            new("User.CountryEmpty", "Country cannot be empty.", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidPassword =
                new("User.Instructions", "Password does not meet the following criteria ", StatusCodes.Status400BadRequest);

        public static readonly Error Missmatch =
            new("User.mis match", " please check your password and confirm your password  ", StatusCodes.Status400BadRequest);


        public static readonly Error PasswordNotSet =
                 new("User.PasswordNotSet", "The password has not been set for this user.", StatusCodes.Status400BadRequest);



        public static readonly Error InvalidPasswordHash =
             new("User.incorrect hash", "The hash is not correct.", StatusCodes.Status400BadRequest);



        public static readonly Error InvalidOldPassword =
                new("User.InvalidOldPassword", "The provided old password is incorrect.", StatusCodes.Status400BadRequest);
        public static readonly Error PasswordTooShort =
        new("User.PasswordTooShort", "Password must be at least 8 characters long.", StatusCodes.Status400BadRequest);

        public static readonly Error MissingUppercase =
            new("User.MissingUppercase", "Password must contain at least one uppercase letter.", StatusCodes.Status400BadRequest);

        public static readonly Error MissingLowercase =
            new("User.MissingLowercase", "Password must contain at least one lowercase letter.", StatusCodes.Status400BadRequest);

        public static readonly Error MissingNumber =
            new("User.MissingNumber", "Password must contain at least one numeric digit.", StatusCodes.Status400BadRequest);

        public static readonly Error MissingSpecialCharacter =
            new("User.MissingSpecialCharacter", "Password must contain at least one special character (!@#$%^&*).", StatusCodes.Status400BadRequest);

        public static readonly Error PasswordsDoNotMatch =
            new("User.PasswordsDoNotMatch", "Please check your password and confirm password. They do not match.", StatusCodes.Status400BadRequest);



        public static readonly Error InvalidInstagramLink =
        new("SocialMedia.InvalidInstagramLink", "The Instagram link must contain 'instagram.com'.", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidFacebookLink =
            new("SocialMedia.InvalidFacebookLink", "The Facebook link must contain 'facebook.com'.", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidGitHubLink =
            new("SocialMedia.InvalidGitHubLink", "The GitHub link must contain 'github.com'.", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidLinkedInLink =
            new("SocialMedia.InvalidLinkedInLink", "The LinkedIn link must contain 'linkedin.com'.", StatusCodes.Status400BadRequest);


        public static readonly Error InvalidLink =
            new("SocialMedia.InvalidLink", "The  link must contain 'platform name.com'.", StatusCodes.Status400BadRequest);



    }
}

