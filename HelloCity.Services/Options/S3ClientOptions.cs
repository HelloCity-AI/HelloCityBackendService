namespace HelloCity.Services.Options
{
    public class S3ClientOptions
    {
        public string Bucket { get; set; } = default!;
        public string Region { get; set; } = "ap-southeast-2";
        public string KeyPrefix { get; set; } = "ProfileImage";
        public int PresignExpiryMinutes { get; set; } = 60;

    }
}
