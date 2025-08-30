using AutoMapper;
using HelloCity.Api.DTOs.ChecklistItem;
using HelloCity.Api.DTOs.Users;
using HelloCity.IServices;
using HelloCity.Models.Entities;
using HelloCity.Models.Migrations;
using HelloCity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop.Infrastructure;
using System.IO;
using System.IO.Pipes;
using System.Net.Mime;

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
        private readonly IImageStorageService _imageStorageService;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger, IChecklistItemService checklistItemService, IImageStorageService ImageStorageService)
        {
            _checklistItemService = checklistItemService;
            _userService = userService;
            _imageStorageService = ImageStorageService;
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
        /// Get current user by Auth0 sub (from access token).
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> Me()
        {
            var sub = User.FindFirst("sub")?.Value
                      ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrWhiteSpace(sub))
            {
                _logger.LogWarning("GET /api/user/me missing 'sub' claim");
                return Unauthorized("Missing 'sub' claim.");
            }

            var user = await _userService.GetUserByAuthSubIdAsync(sub);
            if (user is null)
            {
                _logger.LogInformation("No user found for sub {sub}", sub);
                return NotFound($"User with sub '{sub}' not found.");
            }

            var dto = _mapper.Map<UserDto>(user);
            return Ok(dto);
        }

        /// <summary>
        /// Create a new user
        /// Example: POST/api/user-profile/
        /// </summary>
        /// <param name="dto">User creation data</param>
        /// <returns>Basic info of the created user</returns>

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto dto)
        {
            _logger.LogInformation("Creating user with email: {Email}", dto.Email);
            
            var imageFile = dto.File;
            var user = _mapper.Map<Users>(dto);
            string? PresignedUrl = null;

            if (imageFile != null)
            {
                using Stream fileStream = imageFile.OpenReadStream();
                var fileExtension = Path.GetExtension(imageFile.FileName);
                var imageResult = await _imageStorageService.UploadProfileImageAsync(fileStream, fileExtension, user.UserId.ToString());
                user.AvatarKey = imageResult.Key;
                PresignedUrl = imageResult.GetUrl;
            }

            var result = await _userService.CreateUserAsync(user);
            var userDto = _mapper.Map<UserDto>(result);
            userDto.AvatarUrl = PresignedUrl;

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
                        email = userDto.Email,
                        avatarURL = userDto.AvatarUrl
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditUser([FromForm] EditUserDto dto, Guid id)
        {
            _logger.LogInformation("Editing user with ID: {UserId}", id);
            var updatedUser = _mapper.Map<Users>(dto);
            var imageFile = dto.File;
            string? PresignedUrl = null;

            if (imageFile != null)
            {
                using Stream fileStream = imageFile.OpenReadStream();
                var fileExtension = Path.GetExtension(imageFile.FileName);
                var imageResult = await _imageStorageService.UploadProfileImageAsync(fileStream, fileExtension, updatedUser.UserId.ToString());
                updatedUser.AvatarKey = imageResult.Key;
                PresignedUrl = imageResult.GetUrl;
            }

            var result = await _userService.EditUserAsync(id, updatedUser);
            var userDto = _mapper.Map<UserDto>(result);
            userDto.AvatarUrl = PresignedUrl;

            return Ok(new
            {
                message = "edit user successfully",
                data = new
                {
                    userId = userDto.UserId,
                    username = userDto.Username,
                    email = userDto.Email,
                    avatarURL = userDto.AvatarUrl
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
        [HttpPut("{userId}/checklist-item")]
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

        [HttpPost("{id}/profile-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar(Guid id, [FromForm] UploadImageRequest req)
        {
            _logger.LogInformation("Uploading profile image for user with ID: {UserId}", id);

            await using var fileStream = req.File.OpenReadStream();
            var fileExtension = Path.GetExtension(req.File.FileName);

            var result = await _imageStorageService.UploadProfileImageAsync(fileStream, fileExtension, id.ToString());
            var UpdatedAvatarKey = result.Key;

            await _userService.EditUserAvatarKeyAsync(id, UpdatedAvatarKey);

            return Ok(new
            {
                message = "Avatar uploaded",
                data = new
                {
                    s3Key = result.Key,
                    presignedGetUrl = result.GetUrl
                }
            });
        }

        [HttpGet("{id}/profile-image/url")]
        public async Task<ActionResult> GetAvatarUrl(Guid id)
        {
            _logger.LogInformation("Profile Image is required for ID: {UserId}", id);

            var user = await _userService.GetUserProfileAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                throw new KeyNotFoundException("User not found with given ID.");
            }

            var avatarS3Key = user.AvatarKey;

            //This is for existing user who has never uploaded any profile image before.
            //So frontend will only show default avatar.
            if (string.IsNullOrEmpty(avatarS3Key))
            {
                return NotFound(new { message = "No profile image uploaded." });
            }

            var url = _imageStorageService.GeneratePresignedUrl(avatarS3Key);
            return Ok(new { url });
        }

    }
}
