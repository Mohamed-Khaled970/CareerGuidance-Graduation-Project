namespace CareerGuidance.Api.DTO.Validator
{
    public class ScheduleInterviewRequestValidator : AbstractValidator<ScheduleInterviewRequest>
    {
        public ScheduleInterviewRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.ScheduledDate)
               .NotEmpty()
               .NotNull();

            RuleFor(x => x.MeetingLink)
                 .NotEmpty()
                .NotNull();

        }
    }
}
