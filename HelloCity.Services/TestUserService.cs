using HelloCity.IServices;
using HelloCity.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace HelloCity.Services;
// Only for test purpose
public class TestUserService: ITestUserService
{
    private readonly ApiConfigs _apiConfigs;

    public TestUserService(IOptions<ApiConfigs> apiConfigs)
    {
        _apiConfigs = apiConfigs.Value;
    }
    
    public async Task<List<object?>> TestGetAllUserAsync()
    {
        var users = new List<Object>();
        await using (var connection = new NpgsqlConnection(_apiConfigs.DBConnection))
        {
            await connection.OpenAsync();
            var sql = $"SELECT * FROM test";
            
            await using (var command = new NpgsqlCommand(sql, connection))
            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var user = new 
                    {
                        id = reader.GetValue(0),
                        email = reader.GetValue(1),
                    };
                   
                    users.Add(user);
                }
            }
        };
        return users;
    }
}