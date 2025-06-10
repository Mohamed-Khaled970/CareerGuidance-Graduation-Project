using CareerGuidance.Api.Abstractions.Const;

namespace CareerGuidance.Api.DTO.Validator
{
    public class FileExtensionValidator : AbstractValidator<IFormFile>
    {
        public FileExtensionValidator()
        {
            RuleFor(file => file.FileName)
                .Must(fileName =>
                {
                    var fileExtension = Path.GetExtension(fileName);
                    return FileSettings.AllowedExtensions
                        .Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
                })
                .WithMessage("Only PDF files are allowed.")
                .When(file => file is not null);
        }
    }
}
