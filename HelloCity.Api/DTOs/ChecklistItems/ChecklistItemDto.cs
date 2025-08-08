using ImportanceEnum = HelloCity.Models.Enums.ImportanceLevel;

namespace HelloCity.Api.DTOs.ChecklistItem
{
  public class ChecklistItemDto
  {
    public Guid ChecklistItemId { get; set; }
    public Guid OwnerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    public ImportanceEnum Importance { get; set; } = ImportanceEnum.Low;

  }
}