using HelloCity.IRepository;
using HelloCity.Models;
using HelloCity.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloCity.Repository{
  public class ChecklistItemRepository : IChecklistItemRepository
  {
    private readonly AppDbContext _context;
    public ChecklistItemRepository(AppDbContext context)
    {
      _context = context;
    }
    public async Task<List<ChecklistItem>?> GetChecklistItemsAsync(Guid id)
    {
      var user = await _context.Users
        .Include(u => u.ChecklistItems) 
        .FirstOrDefaultAsync(u => u.UserId == id);
      if (user == null) return null;
      return user.ChecklistItems;
    }
    public async Task<ChecklistItem> AddChecklistItemAsync(Guid id, ChecklistItem newChecklistItem)
    {
      _context.ChecklistItems.Add(newChecklistItem);
      await _context.SaveChangesAsync();
      return newChecklistItem;
    }
  }
}