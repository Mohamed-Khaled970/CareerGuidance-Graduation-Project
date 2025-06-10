namespace CareerGuidance.Api.DTO.Validator
{
    public class GoogleSignUpRequestValidator : AbstractValidator<GoogleSignUpRequest>
    {
        public GoogleSignUpRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
