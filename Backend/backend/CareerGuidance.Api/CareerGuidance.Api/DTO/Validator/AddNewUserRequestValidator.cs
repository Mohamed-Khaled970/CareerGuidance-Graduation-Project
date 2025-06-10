namespace CareerGuidance.Api.DTO.Validator
{
    public class AddNewUserRequestValidator : AbstractValidator<AddNewUserRequest>
    {
        public AddNewUserRequestValidator()
        {
            RuleFor(x => x.UserName)
           .NotEmpty()
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
                .NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
                .WithMessage("Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.");

            RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress().WithMessage("Invalid email address format.")
               .MinimumLength(16).WithMessage("Email must be at least 16 characters long.")
               .MaximumLength(40).WithMessage("Email must not exceed 40 characters.");
        }
    }
}
