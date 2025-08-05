using HelloCity.Models.Entities;

namespace HelloCity.IServices
{
    public interface IChecklistItemService
    {
        Task<ChecklistItem> CreateChecklistItemAsync(Users user);

    }
}