using HelloCity.IServices;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3.Model;

namespace HelloCity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase
{
    private readonly IS3Service _s3Service;
    private readonly ILogger<S3Controller> _logger;

    public S3Controller(IS3Service s3Service, ILogger<S3Controller> logger)
    {
        _s3Service = s3Service;
        _logger = logger;
    }

    /// <summary>
    /// 创建S3存储桶
    /// </summary>
    /// <param name="bucketName">存储桶名称</param>
    /// <returns>创建结果</returns>
    [HttpPost("create-bucket")]
    public async Task<IActionResult> CreateBucket([FromBody] CreateBucketRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.BucketName))
            {
                return BadRequest("存储桶名称不能为空");
            }

            var response = await _s3Service.CreateBucketAsync(request.BucketName);
            
            return Ok(new
            {
                success = true,
                message = $"存储桶 '{request.BucketName}' 创建成功",
                bucketName = request.BucketName,
                requestId = response.ResponseMetadata?.RequestId
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建存储桶时发生错误: {BucketName}", request.BucketName);
            return StatusCode(500, new { success = false, message = "创建存储桶时发生内部错误" });
        }
    }

    /// <summary>
    /// 检查存储桶是否存在
    /// </summary>
    /// <param name="bucketName">存储桶名称</param>
    /// <returns>是否存在</returns>
    [HttpGet("bucket-exists/{bucketName}")]
    public async Task<IActionResult> BucketExists(string bucketName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(bucketName))
            {
                return BadRequest("存储桶名称不能为空");
            }

            var exists = await _s3Service.BucketExistsAsync(bucketName);
            
            return Ok(new
            {
                bucketName = bucketName,
                exists = exists
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "检查存储桶是否存在时发生错误: {BucketName}", bucketName);
            return StatusCode(500, new { success = false, message = "检查存储桶时发生内部错误" });
        }
    }

    /// <summary>
    /// 列出所有存储桶
    /// </summary>
    /// <returns>存储桶列表</returns>
    [HttpGet("list-buckets")]
    public async Task<IActionResult> ListBuckets()
    {
        try
        {
            var response = await _s3Service.ListBucketsAsync();
            
            var buckets = response.Buckets.Select(b => new
            {
                name = b.BucketName,
                creationDate = b.CreationDate
            }).ToList();

            return Ok(new
            {
                success = true,
                count = buckets.Count,
                buckets = buckets
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "列出存储桶时发生错误");
            return StatusCode(500, new { success = false, message = "列出存储桶时发生内部错误" });
        }
    }

    /// <summary>
    /// 删除存储桶
    /// </summary>
    /// <param name="bucketName">存储桶名称</param>
    /// <returns>删除结果</returns>
    [HttpDelete("delete-bucket/{bucketName}")]
    public async Task<IActionResult> DeleteBucket(string bucketName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(bucketName))
            {
                return BadRequest("存储桶名称不能为空");
            }

            var response = await _s3Service.DeleteBucketAsync(bucketName);
            
            return Ok(new
            {
                success = true,
                message = $"存储桶 '{bucketName}' 删除成功",
                bucketName = bucketName,
                requestId = response.ResponseMetadata?.RequestId
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除存储桶时发生错误: {BucketName}", bucketName);
            return StatusCode(500, new { success = false, message = "删除存储桶时发生内部错误" });
        }
    }
}

public class CreateBucketRequest
{
    public string BucketName { get; set; } = string.Empty;
}