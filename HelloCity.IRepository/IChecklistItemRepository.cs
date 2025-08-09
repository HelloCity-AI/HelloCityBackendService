using HelloCity.Models.Entities;

namespace HelloCity.IRepository {
  public interface IChecklistItemRepository
  {
    Task<List<ChecklistItem>?> GetChecklistItemsAsync(Guid userId);
    Task<ChecklistItem> AddChecklistItemAsync(Guid userId, ChecklistItem newChecklistItem);
  }
}