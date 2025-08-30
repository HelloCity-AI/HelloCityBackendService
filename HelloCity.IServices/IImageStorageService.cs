namespace HelloCity.IServices
{
    public record UploadResult(string Key, string GetUrl);
    public interface IImageStorageService
    {
            Task<UploadResult> UploadProfileImageAsync(Stream fileStream, string fileExtension, string userId);
            string GeneratePresignedUrl(string key);
    }
}
