using HelloCity.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloCity.Api.Controllers;
// Only for test purpose
[ApiController]
[Route("/api/[controller]")]
public class TestUserController: ControllerBase
{
    private readonly ITestUserService _testUserService;

    public TestUserController(ITestUserService testUserService)
    {
        _testUserService = testUserService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> TestAsync()
    {
        var data =await _testUserService.TestGetAllUserAsync();
        return Ok(data);
    }
}