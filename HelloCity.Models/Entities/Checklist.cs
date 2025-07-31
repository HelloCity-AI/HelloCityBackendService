using HelloCity.Models.Enums;
using ImportanceEnum = HelloCity.Models.Enums.ImportanceLevel;


namespace HelloCity.Models.Entities
{
  public class ChecklistItems
  {
    public Guid ChecklistItemId { get; set; } = Guid.NewGuid();
    /*Can change to integer ID depending on implementation*/
    // Foreign key
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    public ImportanceLevel Importance { get; set; } = ImportanceEnum.Low;


  }
}
