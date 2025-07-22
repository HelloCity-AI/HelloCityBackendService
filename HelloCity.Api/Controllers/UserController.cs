using HelloCity.IServices;
using HelloCity.Models.DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloCity.Api.Controllers
{
    [Route("api/user-profile")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// get user profile by user Id
        /// Example: GET/api/user-profile/(UUID, e.g. 123e4567-e89b-12d3-a456-426614174000)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserProfile(Guid id)
        {
            var userDto = await _userService.GetUserProfileAsync(id);

            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return Ok(new { message = "create user successfully", data = result });
        }

    }
}
