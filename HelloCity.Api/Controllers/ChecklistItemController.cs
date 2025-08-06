using HelloCity.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelloCity.Models.Entities;
using AutoMapper;
using HelloCity.Api.DTOs.Users;

namespace HelloCity.Api.Controllers
{
  [Route("/api/checklist-item")]
  [ApiController]
  public class ChecklistItemController : ControllerBase
  {
      private readonly IChecklistItemService _checklistItemService;
      private readonly IUserService _userService;
      private readonly IMapper _mapper;
      private readonly ILogger<ChecklistItemController> _logger;

      public ChecklistItemController(
          IChecklistItemService checklistItemService,
          IUserService userService, 
          IMapper mapper,
          ILogger<ChecklistItemController> logger)
      {
          _checklistItemService = checklistItemService;
          _userService = userService; 
          _mapper = mapper;
          _logger = logger;
      }

      [HttpPost("{id}")]
      public async Task<ActionResult<UserDto>> CreateChecklistItem(Guid userId, ChecklistItem newChecklistItem)
      {
          _logger.LogInformation("Getting user profile for ID: {UserId}", userId);

          var user = await _checklistItemService.AddChecklistItemAsync(userId, newChecklistItem);

          if (user == null)
          {
              _logger.LogWarning("User not found with ID: {UserId}", userId);
              throw new KeyNotFoundException("User not found with given ID.");
          }

          //var userDto = _mapper.Map<UserDto>(user);

          return Ok(newChecklistItem);
      }
  }

}