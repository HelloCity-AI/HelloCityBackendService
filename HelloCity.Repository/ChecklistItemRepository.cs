using HelloCity.IRepository;
using HelloCity.Models;
using HelloCity.Models.Entities;

namespace HelloCity.Repository{
  public class ChecklistItemRepository : IChecklistItemRepository
  {
    private readonly AppDbContext _context;
    public ChecklistItemRepository(AppDbContext context)
    {
      _context = context;
    }
    public async Task<ChecklistItem> AddChecklistItemAsync(Guid id, ChecklistItem newChecklistItem)
    {
      _context.ChecklistItems.Add(newChecklistItem);
      await _context.SaveChangesAsync();
      return newChecklistItem;
    }
  }
}