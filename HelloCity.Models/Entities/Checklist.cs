using ImportanceEnum = HelloCity.Models.Enums.ImportanceLevel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloCity.Models.Entities
{
  [Table("ChecklistItems")]
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

