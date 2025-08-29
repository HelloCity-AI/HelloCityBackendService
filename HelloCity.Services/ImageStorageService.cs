using HelloCity.IServices;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using HelloCity.Services.Options;
using Microsoft.Extensions.Options;



namespace HelloCity.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IAmazonS3 _s3;
        private readonly S3ClientOptions _opt;

        public ImageStorageService (IOptions<S3ClientOptions> opt)
        {
            _opt = opt.Value;

            _s3 = new AmazonS3Client(RegionEndpoint.GetBySystemName(_opt.Region)); 
        }

        public async Task<UploadResult> UploadProfileImageAsync(Stream fileStream, string fileExtension, string userId, CancellationToken ct = default)
        {

            //This is only for code test to print out which S3 bucket is connected
            //var sts = new AmazonSecurityTokenServiceClient();
            //var ident = await sts.GetCallerIdentityAsync(new GetCallerIdentityRequest());
            //Console.WriteLine($"Caller ARN: {ident.Arn}");

            var objectName = $"{Guid.NewGuid()}{fileExtension}";
            var objectKey = $"{_opt.KeyPrefix}{userId}{objectName}";

            var s3BucketPutRequest = new PutObjectRequest
            {
                BucketName = _opt.Bucket,
                Key = objectKey,
                InputStream = fileStream,
                
            };

            var s3BucketResponse = await _s3.PutObjectAsync(s3BucketPutRequest, ct);

            var presignedURL = GeneratePresignedUrl(objectKey);

            return new UploadResult(objectKey, presignedURL);
        }

        public string GeneratePresignedUrl(string key)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _opt.Bucket,
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(_opt.PresignExpiryMinutes)
            };
            return _s3.GetPreSignedURL(request);
        }
    }
}
