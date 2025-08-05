using ImportanceEnum = HelloCity.Models.Enums.ImportanceLevel;

namespace HelloCity.Models.Entities
{
  public class ChecklistItem
  {
    public Guid ChecklistItemId { get; set; } = Guid.NewGuid();
    public Guid UserOwnerId { get; set; }
    public required Users UserOwner { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    public ImportanceEnum Importance { get; set; } = ImportanceEnum.Low;
  }
}

