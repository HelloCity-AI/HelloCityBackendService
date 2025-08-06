using HelloCity.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelloCity.Models.Entities;
using AutoMapper;
using HelloCity.Api.DTOs.Users;
using HelloCity.Api.DTOs.ChecklistItem;
using System.Diagnostics;

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
      public async Task<ActionResult<ChecklistItem>> CreateChecklistItem(Guid id, CreateChecklistItemDto newChecklistItemDto)
      {
          _logger.LogInformation("Creating checklist for User with ID: {id}", id);
          var newChecklistItem = _mapper.Map<ChecklistItem>(newChecklistItemDto);
          var savedChecklistItem = await _checklistItemService.AddChecklistItemAsync(id, newChecklistItem);
          if (savedChecklistItem == null)
          {
            _logger.LogWarning("Failed to create checklist item for User with ID: {id}", id);
            return NotFound("User not found");
          }
          var checlistItemDto = _mapper.Map<ChecklistItemDto>(savedChecklistItem);
          return Ok(checlistItemDto);
      }
  }

}