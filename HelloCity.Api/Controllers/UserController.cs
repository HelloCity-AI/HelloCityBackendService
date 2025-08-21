using HelloCity.IServices;
using HelloCity.Api.DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelloCity.Models.Entities;
using AutoMapper;
using Microsoft.JSInterop.Infrastructure;
using HelloCity.Api.DTOs.ChecklistItem;

namespace HelloCity.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IChecklistItemService _checklistItemService;
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger, IChecklistItemService checklistItemService)
        {
            _checklistItemService = checklistItemService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get user profile by user ID
        /// Example: GET/api/user-profile/(UUID, e.g. 123e4567-e89b-12d3-a456-426614174000)
        /// </summary>
        /// <param name="id">User ID (Guid)</param>
        /// <returns>User profile info</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserProfile(Guid id)
        {
            _logger.LogInformation("Getting user profile for ID: {UserId}", id);

            var user = await _userService.GetUserProfileAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                throw new KeyNotFoundException("User not found with given ID.");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }


        /// <summary>
        /// Create a new user
        /// Example: POST/api/user-profile/
        /// </summary>
        /// <param name="dto">User creation data</param>
        /// <returns>Basic info of the created user</returns>

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            _logger.LogInformation("Creating user with email: {Email}", dto.Email);

            var user = _mapper.Map<Users>(dto);
            var result = await _userService.CreateUserAsync(user);
            var userDto = _mapper.Map<UserDto>(result);

            return CreatedAtAction(
                nameof(GetUserProfile),
                new { id = userDto.UserId },
                new
                {
                    message = "create user successfully",
                    data = new
                    {
                        userId = userDto.UserId,
                        username = userDto.Username,
                        email = userDto.Email
                    }
                });
        }

        /// <summary>
        /// Edit an existing user
        /// </summary>
        /// <param name="dto">Fields to update</param>
        /// <param name="id">User Id</param>
        /// <returns>Updated user info</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromBody] EditUserDto dto, Guid id)
        {
            _logger.LogInformation("Editing user with ID: {UserId}", id);

            var updatedUser = _mapper.Map<Users>(dto);
            var result = await _userService.EditUserAsync(id, updatedUser);
            var userDto = _mapper.Map<UserDto>(result);

            return Ok(new
            {
                message = "edit user successfully",
                data = new
                {
                    userId = userDto.UserId,
                    username = userDto.Username,
                    email = userDto.Email
                }
            });
        }
        [HttpPost("{userId}/checklist-item")]
        public async Task<ActionResult<ChecklistItem>> CreateChecklistItem(Guid userId, CreateChecklistItemDto newChecklistItemDto)
        {
            _logger.LogInformation("Creating checklist for User with ID: {userId}", userId);
            try
            {
                var newChecklistItem = _mapper.Map<ChecklistItem>(newChecklistItemDto);
                var savedChecklistItem = await _checklistItemService.AddChecklistItemAsync(userId, newChecklistItem);
                var checklistItemDto = _mapper.Map<ChecklistItemDto>(savedChecklistItem);
                return Created("/api/checklist-item/" + checklistItemDto.ChecklistItemId, checklistItemDto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found with ID: {userId}", userId);
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating checklist item for User with ID: {userId}", userId);
                return Problem("An unexpected error occurred.");
            }
        }
        [HttpGet("{userId}/checklist-item")]
        public async Task<ActionResult<List<ChecklistItem>>> GetChecklistItems(Guid userId)
        {
            _logger.LogInformation("Getting checklist for User with ID: {userId}", userId);
            try
            {
                var userChecklistItems = await _checklistItemService.GetChecklistItemsAsync(userId);
                var checklistItemDtos = _mapper.Map<List<ChecklistItemDto>>(userChecklistItems);
                return Ok(checklistItemDtos);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found with ID: {userId}", userId);
                return NotFound("User not found");
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditCheckListItem(Guid userId, Guid itemId, EditCheckListItemDto editChecklistItemDto)
        {
            _logger.LogInformation("Editing checklist for User with ID: {userId}", userId);
            try
            {
                var editChecklistItem = _mapper.Map<ChecklistItem>(editChecklistItemDto);
                var savedChecklistItem = await _checklistItemService.EditChecklistItemAsync(userId, itemId, editChecklistItem);
                var checklistItemDto = _mapper.Map<ChecklistItemDto>(savedChecklistItem);
                return Created("/api/checklist-item/" + checklistItemDto.ChecklistItemId, checklistItemDto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found with ID: {userId}", userId);
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while editing checklist item for User with ID: {userId}", userId);
                return Problem("An unexpected error occurred.");
            }
        }


    }
}
