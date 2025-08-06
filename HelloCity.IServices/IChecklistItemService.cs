using HelloCity.Models.Entities;

namespace HelloCity.IServices
{
    public interface IChecklistItemService
    {
        Task<ChecklistItem> AddChecklistItemAsync(Guid userId, ChecklistItem newChecklistItem);
    }
}