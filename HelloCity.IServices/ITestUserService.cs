namespace HelloCity.IServices;
// Only for test purpose
public interface ITestUserService
{
    Task<List<object?>> TestGetAllUserAsync();
}