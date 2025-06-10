namespace CareerGuidance.Api.DTO.Validator
{
    /*
     * File Name: RegisterUserDtoValidator.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies:
     *      - FluentValidation
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     *
     * Description:
     *      This class provides validation rules for the RegisterUserDto record.
     *      It uses FluentValidation to ensure that the provided registration details
     *      meet the required criteria before further processing.
     *      - Validates Name to be non-empty, between 8 and 50 characters, and contain only letters.
     *      - Validates UserName to be between 5 and 20 characters long, start with at least 3 letters, 
     *        and can include letters, numbers, and underscores.
     *      - Validates Password to be at least 8 characters long and include at least one uppercase letter,
     *        one lowercase letter, one digit, and one special character. Ensures the password does not 
     *        contain any part of the name or phone number.
     *      - Validates ConfirmPassword to match the Password.
     *      - Validates Email to be a valid email address with a length between 16 and 40 characters.
     *      - Validates PhoneNumber to start with +20 followed by exactly 10 digits.
     *      - Validates Country and City to be non-empty.
     *      - Validates Role to be either 'Student' or 'Instructor'.
     *      - Validates Age to be between 12 and 50.
     */
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            /*
             * Rule: Name
             * Description: Validates that the Name property is not empty, between 8 and 50 characters,
             * and contains only letters.
             */
            //RuleFor(x => x.Name)
            //    .NotEmpty()
            //    .Length(8, 50)
            //    .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name must contain only letters.");

            /*
             * Rule: UserName
             * Description: Validates that the UserName property is 5 to 20 characters long, starts with 
             * at least 3 letters, and can include letters, numbers, and underscores.
             */
            RuleFor(x => x.UserName)
               .Matches(@"^[a-zA-Z]{3}[a-zA-Z0-9_]{2,17}$")
               .WithMessage("UserName must be 5 to 20 characters long, start with at least 3 letters, and can include letters, numbers, and underscores.");

            /*
             * Rule: Password
             * Description: Validates that the Password property meets the complexity requirements:
             * at least 8 characters long, includes at least one uppercase letter, one lowercase letter,
             * one digit, and one special character. Also ensures the password does not contain any part 
             * of the name or the phone number.
             */
            RuleFor(x => x.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
                .WithMessage("Password must be at least 8 characters long and include at least one uppercase letter, " +
                "one lowercase letter, one digit, and one special character.");
            //.Must((user, password) =>
            //{
            //    var lowerPassword = password.ToLower();
            //    var lowerNameParts = user.Name.ToLower().Split(' ');
            //    foreach (var part in lowerNameParts)
            //    {
            //        if (lowerPassword.Contains(part))
            //        {
            //            return false;
            //        }
            //    }
            //    return true;
            //})
            //.WithMessage("Password cannot contain any part of the name.")
            //.Must((user, password) => !password.Contains(user.PhoneNumber))
            //.WithMessage("Password cannot contain the phone number.");

            /*
             * Rule: ConfirmPassword
             * Description: Validates that the ConfirmPassword property matches the Password property.
             */
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            /*
             * Rule: Email
             * Description: Validates that the Email property is a valid email address and its length is 
             * between 16 and 40 characters.
             */
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address format.")
                .MinimumLength(16).WithMessage("Email must be at least 16 characters long.")
                .MaximumLength(40).WithMessage("Email must not exceed 40 characters.");

            /*
             * Rule: PhoneNumber
             * Description: Validates that the PhoneNumber property is not empty and starts with +20 
             * followed by exactly 10 digits.
             */
            //RuleFor(x => x.PhoneNumber)
            //    .NotEmpty().WithMessage("Phone number is required.")
            //    .Matches(@"^\+20\d{10}$")
            //    .WithMessage("Phone number must start with +20 followed by exactly 10 digits.");

            ///*
            // * Rule: Country
            // * Description: Validates that the Country property is not empty.
            // */
            //RuleFor(x => x.Country)
            //    .NotEmpty();

            ///*
            // * Rule: City
            // * Description: Validates that the City property is not empty.
            // */
            //RuleFor(x => x.City)
            //    .NotEmpty();

            ///*
            // * Rule: Role
            // * Description: Validates that the Role property is either 'Student' or 'Instructor'.
            // */
            //RuleFor(x => x.Role)
            //   .Must(role => role == "Student" || role == "Instructor")
            //   .WithMessage("Role must be either 'Student' or 'Instructor'.");

            ///*
            // * Rule: Age
            // * Description: Validates that the Age property is between 12 and 50.
            // */
            //RuleFor(x => x.Age)
            //  .InclusiveBetween(12, 50)
            //  .WithMessage("Age must be between 12 and 50.");
        }
    }
}
