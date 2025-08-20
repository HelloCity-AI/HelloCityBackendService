using Amazon.S3;
using Amazon.S3.Model;
using HelloCity.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelloCity.Services;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3Service> _logger;
    private readonly IConfiguration _configuration;

    public S3Service(IAmazonS3 s3Client, ILogger<S3Service> logger, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<PutBucketResponse> CreateBucketAsync(string bucketName)
    {
        try
        {
            _logger.LogInformation("开始创建S3存储桶: {BucketName}", bucketName);
            
            // 检查存储桶是否已存在
            if (await BucketExistsAsync(bucketName))
            {
                _logger.LogWarning("存储桶 {BucketName} 已存在", bucketName);
                throw new InvalidOperationException($"存储桶 '{bucketName}' 已存在");
            }

            var request = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            var response = await _s3Client.PutBucketAsync(request);
            
            _logger.LogInformation("成功创建S3存储桶: {BucketName}", bucketName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建S3存储桶失败: {BucketName}", bucketName);
            throw;
        }
    }

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        try
        {
            await _s3Client.GetBucketLocationAsync(bucketName);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "检查存储桶是否存在时发生错误: {BucketName}", bucketName);
            throw;
        }
    }

    public async Task<ListBucketsResponse> ListBucketsAsync()
    {
        try
        {
            _logger.LogInformation("开始列出所有S3存储桶");
            var response = await _s3Client.ListBucketsAsync();
            _logger.LogInformation("成功列出 {Count} 个存储桶", response.Buckets.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "列出S3存储桶失败");
            throw;
        }
    }

    public async Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName)
    {
        try
        {
            _logger.LogInformation("开始删除S3存储桶: {BucketName}", bucketName);
            
            // 检查存储桶是否存在
            if (!await BucketExistsAsync(bucketName))
            {
                _logger.LogWarning("存储桶 {BucketName} 不存在", bucketName);
                throw new InvalidOperationException($"存储桶 '{bucketName}' 不存在");
            }

            var request = new DeleteBucketRequest
            {
                BucketName = bucketName
            };

            var response = await _s3Client.DeleteBucketAsync(request);
            
            _logger.LogInformation("成功删除S3存储桶: {BucketName}", bucketName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除S3存储桶失败: {BucketName}", bucketName);
            throw;
        }
    }
}