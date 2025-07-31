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
                throw new KeyNotFoundException("User not found with given ID.");
            }

            return Ok(userDto);
        }


        /// <summary>
        /// post user
        /// Example: POST/api/user-profile/
        /// </summary>
        /// <param name="dto">User creation data</param>
        /// <returns>Returns the created user's basic info</returns>

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserInfoCollectionDTO dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return Ok(new
            {
                message = "create user successfully",
                data = new
                {
                    userId = result.UserId,
                    username = result.Username,
                    email = result.Email
                }
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromBody] UserInfoCollectionDTO dto, Guid id)
        {
            var result = await _userService.EditUserAsync(id, dto);
            return Ok(new
            {
                message = "edit user successfully",
                data = new
                {
                    userId = result.UserId,
                    username = result.Username,
                    email = result.Email
                }
            });
        }

    }
}
