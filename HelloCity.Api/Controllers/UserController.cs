using HelloCity.IServices;
using HelloCity.Api.DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelloCity.Models.Entities;
using AutoMapper;

namespace HelloCity.Api.Controllers
{
    [Route("api/user-profile")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
            var user = await _userService.GetUserProfileAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found with given ID.");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }


        /// <summary>
        /// post user
        /// Example: POST/api/user-profile/
        /// </summary>
        /// <param name="dto">User creation data</param>
        /// <returns>Returns the created user's basic info</returns>

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = _mapper.Map<Users>(dto);

            var result = await _userService.CreateUserAsync(user);

            var userDto = _mapper.Map<UserDto>(result);

            return Ok(new
            {
                status = 200,
                message = "create user successfully",
                data = new
                {
                    userId = userDto.UserId,
                    username = userDto.Username,
                    email = userDto.Email
                }
            });
        }

    }
}
