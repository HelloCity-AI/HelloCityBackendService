using HelloCity.IServices;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Models.Utils;

namespace HelloCity.Services
{
  public class ChecklistItemService : IChecklistItemService
  {
    private readonly IChecklistItemRepository _checklistItemRepository;
    private readonly IUserRepository _userRepository;
    public ChecklistItemService(IChecklistItemRepository checklistItemRepository, IUserRepository userRepository)
    {
      _checklistItemRepository = checklistItemRepository;
      _userRepository = userRepository;
    }
    public async Task<ChecklistItem> AddChecklistItemAsync(Guid userId, ChecklistItem newChecklistItem)
    {
      var existingUser = await _userRepository.GetUserByIdAsync(userId);
      if (existingUser == null)
      {
        throw new KeyNotFoundException($"User with ID {userId} not found.");
      }
      newChecklistItem.ChecklistItemId = Guid.NewGuid();
      newChecklistItem.OwnerId = userId;
      newChecklistItem.UserOwner = existingUser;
      existingUser.ChecklistItems.Add(newChecklistItem);
      await _checklistItemRepository.AddChecklistItemAsync(userId, newChecklistItem);
      await _userRepository.UpdateUserAsync(existingUser);
      return newChecklistItem;
    }
    public async Task<List<ChecklistItem>> GetChecklistItemsAsync(Guid userId)
    {
      var existingUser = await _userRepository.GetUserByIdAsync(userId);
      if (existingUser == null)
      {
        throw new KeyNotFoundException($"User with ID {userId} not found.");
      }
      var userChecklistItems = await _checklistItemRepository.GetChecklistItemsAsync(userId);
      return userChecklistItems ?? new List<ChecklistItem>();
    }
  }
}