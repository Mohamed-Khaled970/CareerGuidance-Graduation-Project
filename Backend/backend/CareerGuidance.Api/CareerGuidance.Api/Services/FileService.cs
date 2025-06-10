namespace CareerGuidance.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadsPath;
        private readonly string _CVPath;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _uploadsPath = $"{_webHostEnvironment.WebRootPath}/uploads";
            _CVPath = $"{_webHostEnvironment.WebRootPath}/CVs";
        }
        public async Task<string> SaveFileAsync(IFormFile CvFile)
        {
            if (CvFile is null)
                throw new ArgumentNullException(nameof(CvFile));

            var path = Path.Combine(_CVPath);

            // Ensure the directory exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Get the file extension from the uploaded file
            var extension = Path.GetExtension(CvFile.FileName);

            // Generate a unique name for the file
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fileNamePath = Path.Combine(path, fileName);

            // Save the file to the uploads directory
            using var stream = new FileStream(fileNamePath, FileMode.Create);
            await CvFile.CopyToAsync(stream);

            return fileName;
        }


        public void DeleteFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException(nameof(File));


            var path = Path.Combine(_CVPath, file);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Invalid File Path");

            File.Delete(path);
        }

    }
}
