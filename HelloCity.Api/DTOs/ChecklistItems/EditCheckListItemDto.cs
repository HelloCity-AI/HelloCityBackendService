using ImportanceEnum = HelloCity.Models.Enums.ImportanceLevel;


namespace HelloCity.Api.DTOs.ChecklistItem
{
    public class EditCheckListItemDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsComplete { get; set; }
        public ImportanceEnum Importance { get; set; }
    }
}