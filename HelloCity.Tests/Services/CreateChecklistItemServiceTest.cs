using Moq;
using FluentAssertions;
using HelloCity.Services;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Models.Enums;

namespace HelloCity.Tests.Services
{
  public class CreateChecklistItemServiceTest
  {
    private readonly Mock<IChecklistItemRepository> _checklistItemRepositoryMock;
    private readonly ChecklistItemService _checklistItemService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public CreateChecklistItemServiceTest()
    {
      _checklistItemRepositoryMock = new Mock<IChecklistItemRepository>();
      _userRepositoryMock = new Mock<IUserRepository>();
      _checklistItemService = new ChecklistItemService(
        _checklistItemRepositoryMock.Object,
        _userRepositoryMock.Object
      );
    }
    [Fact]
    public async Task AddChecklistItemAsync_ShouldThrowKeyNotFoundException_WhenUserNotFound()
    {
      // Arrange
      var userId = Guid.NewGuid();

      var newChecklistItem = new ChecklistItem
      {
        Title = "Buy groceries",
        Description = "Buy milk, eggs, and bread",
        Importance = ImportanceLevel.Medium,
        IsComplete = false,
        OwnerId = userId,
        UserOwner = default!
      };

      // Mock user not found
      _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
          .ReturnsAsync((Users?)null);

      // Act & Assert
      var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
          _checklistItemService.AddChecklistItemAsync(userId, newChecklistItem));

      exception.Message.Should().Be($"User with ID {userId} not found.");
      _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task CreateChecklistItemAsync_ShouldSetCorrectProperties_WhenChecklistItemIsCreated()
    {
      // Arrange
      var userId = Guid.NewGuid();
      var user = new Users
      {
        UserId = userId,
        Username = "john_dev",
        Email = "john@example.com",
        SubId = "P@ssword123",
        Gender = Gender.Male,
        Nationality = "Australia",
        City = "Sydney",
        PreferredLanguage = PreferredLanguage.en,
        ChecklistItems = new List<ChecklistItem>()
      };

      var newChecklistItem = new ChecklistItem
      {
        Title = "Buy groceries",
        Description = "Buy milk, eggs, and bread",
        Importance = ImportanceLevel.Medium,
        IsComplete = false,
        OwnerId = userId,
        UserOwner = user
      };

      _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
          .ReturnsAsync(user);

      _checklistItemRepositoryMock.Setup(r =>
          r.AddChecklistItemAsync(userId, It.IsAny<ChecklistItem>()))
          .ReturnsAsync((Guid _, ChecklistItem item) => item);

      _userRepositoryMock.Setup(r =>
          r.UpdateUserAsync(It.IsAny<Users>()))
          .ReturnsAsync((Users u) => u);

      // Act
      var result = await _checklistItemService.AddChecklistItemAsync(userId, newChecklistItem);

      // Assert
      result.Should().NotBeNull();
      result.Title.Should().Be("Buy groceries");
      result.OwnerId.Should().Be(userId);
      result.UserOwner.Username.Should().Be("john_dev");
      _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
      _checklistItemRepositoryMock.Verify(r => r.AddChecklistItemAsync(userId, It.IsAny<ChecklistItem>()), Times.Once);
      _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<Users>()), Times.Once);
    }


    [Fact]
    public async Task AddChecklistItemAsync_ShouldReturnChecklistItemEntity_WhenChecklistItemIsCreatedSuccessfully()
    {
      // Arrange
      var userId = Guid.NewGuid();
      var user = new Users
      {
        UserId = userId,
        Username = "john_dev",
        Email = "john@example.com",
        SubId = "P@ssword123",
        Gender = Gender.Male,
        Nationality = "Australia",
        City = "Sydney",
        PreferredLanguage = PreferredLanguage.en,
        LastJoinDate = DateTime.UtcNow,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        ChecklistItems = new List<ChecklistItem>()
      };

      var newChecklistItem = new ChecklistItem
      {
        Title = "Buy groceries",
        Description = "Buy milk, eggs, and bread",
        Importance = ImportanceLevel.Medium,
        IsComplete = false,
        OwnerId = userId,
        UserOwner = user
      };

      // Mock repository to return the user
      _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
          .ReturnsAsync(user);

      // Mock checklist item repository to simulate saving
      _checklistItemRepositoryMock.Setup(r =>
          r.AddChecklistItemAsync(userId, It.IsAny<ChecklistItem>()))
          .ReturnsAsync((Guid _, ChecklistItem item) => item);

      _userRepositoryMock.Setup(r =>
          r.UpdateUserAsync(It.IsAny<Users>()))
          .ReturnsAsync(user);

      // Act
      var result = await _checklistItemService.AddChecklistItemAsync(userId, newChecklistItem);

      // Assert
      result.Should().NotBeNull();
      result.Title.Should().Be("Buy groceries");
      result.Description.Should().Be("Buy milk, eggs, and bread");
      result.Importance.Should().Be(ImportanceLevel.Medium);
      result.IsComplete.Should().BeFalse();
      result.OwnerId.Should().Be(userId);
      result.ChecklistItemId.Should().NotBeEmpty();
      result.UserOwner.Should().Be(user);
      result.UserOwner.UserId.Should().Be(userId);

      // Verify calls
      _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
      _checklistItemRepositoryMock.Verify(r => r.AddChecklistItemAsync(userId, It.IsAny<ChecklistItem>()), Times.Once);
      _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<Users>()), Times.Once);
    }
  }
}