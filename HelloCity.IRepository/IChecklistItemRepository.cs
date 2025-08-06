
using HelloCity.Models.Entities;

namespace HelloCity.IRepository {
  public interface IChecklistItemRepository
  {
    Task<ChecklistItem> AddChecklistItemAsync(Guid userId, ChecklistItem newChecklistItem);
  }
}