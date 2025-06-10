namespace CareerGuidance.Api.DTO.Validator
{
    public class ApplyInterviewRequestValidator : AbstractValidator<ApplyInterviewRequest>
    {
        public ApplyInterviewRequestValidator()
        {
            RuleFor(x => x.InterviewId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.UserId)
                  .NotEmpty()
                  .NotNull();

            RuleFor(x => x.CVFile)
                .SetValidator(new BlockedSignaturesValidator()!)
                .SetValidator(new FileSizeValidator()!)
                .SetValidator(new FileExtensionValidator()!)
                .SetValidator(new PdfSignatureValidator()!);
        }
    }
}
