using HelloCity.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelloCity.Models.Entities;
using AutoMapper;
using HelloCity.Api.DTOs.Users;

namespace HelloCity.Api.Controllers
{
  [Route("/api/[ChecklistItem]")]
  [ApiController]
  public class ChecklistItemController : ControllerBase
  {
    private readonly IChecklistItemService _checklistItemService;
    private readonly IMapper _mapper;
    private readonly ILogger<ChecklistItemController> _logger;

    public ChecklistItemController(IChecklistItemService checklistItemrService, IMapper mapper, ILogger<ChecklistItemController> logger)
    {
      _checklistItemService = checklistItemrService;
      _mapper = mapper;
      _logger = logger;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateUserDto>> getChecklistItem(Guid id)
    {
      _logger.LogInformation("Getting user profile for ID: {UserId}", id);

      //var user = await UserController._userService.GetUserProfileAsync(id);

      return Ok();
    }
  }
}