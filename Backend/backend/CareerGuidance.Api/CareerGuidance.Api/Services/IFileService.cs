namespace CareerGuidance.Api.Services
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile CvFile);
        public void DeleteFile(string file);
    }
}
