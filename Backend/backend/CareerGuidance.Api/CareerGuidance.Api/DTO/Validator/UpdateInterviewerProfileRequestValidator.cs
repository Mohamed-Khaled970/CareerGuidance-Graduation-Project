namespace CareerGuidance.Api.DTO.Validator
{
    public class UpdateInterviewerProfileRequestValidator : AbstractValidator<UpdateInterviewerProfileRequest>
    {
        public UpdateInterviewerProfileRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Email)
               .NotEmpty()
               .NotNull();

            RuleFor(x => x.ImageUrl)
                 .NotEmpty()
                .NotNull();

            RuleFor(x => x.About)
                .NotEmpty()
                .NotNull();

        }
    }
}
