namespace CareerGuidance.Api.DTO.Validator
{
    public class RejectApplicantRequestValidator : AbstractValidator<RejectApplicantRequest>
    {
        public RejectApplicantRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

        }
    }
}
