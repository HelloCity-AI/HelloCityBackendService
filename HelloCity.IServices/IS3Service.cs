using Amazon.S3.Model;

namespace HelloCity.IServices;

public interface IS3Service
{
    /// <summary>
    /// 创建S3存储桶
    /// </summary>
    /// <param name="bucketName">存储桶名称</param>
    /// <returns>创建结果</returns>
    Task<PutBucketResponse> CreateBucketAsync(string bucketName);
    
    /// <summary>
    /// 检查存储桶是否存在
    /// </summary>
    /// <param name="bucketName">存储桶名称</param>
    /// <returns>是否存在</returns>
    Task<bool> BucketExistsAsync(string bucketName);
    
    /// <summary>
    /// 列出所有存储桶
    /// </summary>
    /// <returns>存储桶列表</returns>
    Task<ListBucketsResponse> ListBucketsAsync();
    
    /// <summary>
    /// 删除存储桶
    /// </summary>
    /// <param name="bucketName">存储桶名称</param>
    /// <returns>删除结果</returns>
    Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName);
}