namespace CareerGuidance.Api.DTO.Validator
{
    public class UpdateInterviewRequestValidator : AbstractValidator<UpdateInterviewRequest>
    {
        public UpdateInterviewRequestValidator()
        {
            RuleFor(x => x.InterviewerId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.InterviewId)
               .NotEmpty()
               .NotNull();

            RuleFor(x => x.Description)
                 .NotEmpty()
                .NotNull();

            RuleFor(x => x.Duration)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull();

        }
    }
}
