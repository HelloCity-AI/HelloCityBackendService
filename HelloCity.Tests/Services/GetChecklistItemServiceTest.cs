using Moq;
using HelloCity.IRepository;
using HelloCity.Services;
using HelloCity.Models.Entities;
using HelloCity.Models.Enums;
using FluentAssertions;

namespace HelloCity.Tests.Services
{
  public class GetChecklistItemServiceTest
  {
    private readonly Mock<IChecklistItemRepository> _checklistItemRepositoryMock;
    private readonly ChecklistItemService _checklistItemService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public GetChecklistItemServiceTest()
    {
      _checklistItemRepositoryMock = new Mock<IChecklistItemRepository>();
      _userRepositoryMock = new Mock<IUserRepository>();
      _checklistItemService = new ChecklistItemService(
        _checklistItemRepositoryMock.Object,
        _userRepositoryMock.Object
      );
    }

    [Fact]
    public async Task GetChecklistItemsAsync_ShouldReturnItems_WhenUserExists()
    {
      // Arrange
      var userId = Guid.NewGuid();
      var user = new Users { UserId = userId, Username = "john" };

      var items = new List<ChecklistItem>
    {
      new ChecklistItem
      {
        ChecklistItemId = Guid.NewGuid(),
        Title = "Buy groceries",
        Description = "Milk, eggs",
        Importance = ImportanceLevel.Medium,
        IsComplete = false,
        OwnerId = userId,
        UserOwner = user
      },
      new ChecklistItem
      {
        ChecklistItemId = Guid.NewGuid(),
        Title = "Do laundry",
        Description = "Whites",
        Importance = ImportanceLevel.Low,
        IsComplete = true,
        OwnerId = userId,
        UserOwner = user
      }
    };

      _userRepositoryMock
        .Setup(r => r.GetUserByIdAsync(userId))
        .ReturnsAsync(user);

      _checklistItemRepositoryMock
        .Setup(r => r.GetChecklistItemsAsync(userId))
        .ReturnsAsync(items);

      // Act
      var result = await _checklistItemService.GetChecklistItemsAsync(userId);

      // Assert
      result.Should().NotBeNull().And.HaveCount(2);
      result[0].Title.Should().Be("Buy groceries");
      result[1].IsComplete.Should().BeTrue();

      _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
      _checklistItemRepositoryMock.Verify(r => r.GetChecklistItemsAsync(userId), Times.Once);
    }
  }
}