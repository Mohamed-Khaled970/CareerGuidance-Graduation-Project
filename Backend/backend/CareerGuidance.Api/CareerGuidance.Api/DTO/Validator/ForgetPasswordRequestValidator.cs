namespace CareerGuidance.Api.DTO.Validator
{
    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                  .NotEmpty()
                  .EmailAddress();
        }
    }
}
