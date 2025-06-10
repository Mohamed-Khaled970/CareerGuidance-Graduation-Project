namespace CareerGuidance.Api.DTO.Validator
{
    public class PdfSignatureValidator : AbstractValidator<IFormFile>
    {
        public PdfSignatureValidator()
        {
            RuleFor(file => file)
                .Must(file =>
                {
                    try
                    {
                        using var binary = new BinaryReader(file.OpenReadStream());
                        var bytes = binary.ReadBytes(4); // Read first 4 bytes

                        var fileSignature = BitConverter.ToString(bytes);

                        // Expected signature for PDF is: 25-50-44-46 => %PDF
                        return fileSignature == "25-50-44-46";
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage("Invalid file format. Only valid PDF files are allowed.")
                .When(file => file is not null);
        }
    }
}
