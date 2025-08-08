using HelloCity.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelloCity.Models.Entities;
using AutoMapper;
using HelloCity.Api.DTOs.ChecklistItem;
using System.Diagnostics;

namespace HelloCity.Api.Controllers
{
  [Route("api/users/{userId}/checklist-item")]
  [ApiController]
  public class ChecklistItemController : ControllerBase
  {
      private readonly IChecklistItemService _checklistItemService;
      private readonly IMapper _mapper;
      private readonly ILogger<ChecklistItemController> _logger;

      public ChecklistItemController(
        IChecklistItemService checklistItemService, 
        IMapper mapper,
        ILogger<ChecklistItemController> logger)
      {
        _checklistItemService = checklistItemService;
        _mapper = mapper;
        _logger = logger;
      }

    [HttpPost]
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
  }
}