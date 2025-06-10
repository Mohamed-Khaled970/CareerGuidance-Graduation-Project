namespace CareerGuidance.Api.DTO.Validator
{
    public class AddQuestionRequestValidator : AbstractValidator<QuestionRequest>
    {
        public AddQuestionRequestValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty();
            RuleFor(x => x.Answer)
                .NotEmpty();
                
        }
    }
}
