namespace HelloCity.Api.DTOs.Users
{
    public class UploadImageRequest
    {
        public IFormFile File { get; set; } = default!;
    }
}
