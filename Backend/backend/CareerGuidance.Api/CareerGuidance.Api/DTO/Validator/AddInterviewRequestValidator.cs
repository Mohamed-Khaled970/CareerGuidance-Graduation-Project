namespace CareerGuidance.Api.DTO.Validator
{
    public class AddInterviewRequestValidator : AbstractValidator<AddInterviewRequest>
    {
        public AddInterviewRequestValidator()
        {
            RuleFor(x => x.InterviewerId)
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
