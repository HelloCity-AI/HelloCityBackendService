namespace HelloCity.Api.DTOs.Users
{
    public class UploadProfileImageRequest
    {
        public IFormFile File { get; set; } = default!;
    }
}
