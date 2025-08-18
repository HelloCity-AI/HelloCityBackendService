using HelloCity.Models.Entities;

namespace HelloCity.IServices
{
    public interface IChecklistItemService
    {
        Task<ChecklistItem> AddChecklistItemAsync(Guid userId, ChecklistItem newChecklistItem);
        Task<List<ChecklistItem>> GetChecklistItemsAsync(Guid userId);
        Task<ChecklistItem> EditChecklistItemAsync(Guid userId, Guid itemId, ChecklistItem newChecklistItem);
    }
}