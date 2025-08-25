using Xunit;
using Moq;
using FluentAssertions;
using HelloCity.Services;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Models.Enums;
using System;
using System.Threading.Tasks;

namespace HelloCity.Tests.Services
{
    public class EditChecklistItemServiceTests
    {
        private readonly Mock<IChecklistItemRepository> _checklistitemRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        private readonly ChecklistItemService _checklistitemService;

        public EditChecklistItemServiceTests()
        {
            _checklistitemRepositoryMock = new Mock<IChecklistItemRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _checklistitemService = new ChecklistItemService(
                _checklistitemRepositoryMock.Object,
                _userRepositoryMock.Object
            );
        }

        [Fact]
        public async Task EditChecklistItemAsync_ShouldReturnUpdatedEditChecklistItem_WhenValidChecklistItemProvided()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var ownerUser = new Users
            {
                UserId = userId,
                Username = "string",
                Email = "string@example.com",
                Gender = Gender.Male,
                Nationality = "Australia",
                City = "Sydney",
                PreferredLanguage = PreferredLanguage.en
            };

            var existing = new ChecklistItem
            {
                ChecklistItemId = itemId,
                UserOwner = ownerUser,
                OwnerId = userId,
                Title = "old-title",
                Description = "old-desc",
                IsComplete = false,
                Importance = ImportanceLevel.Low,
            };

            var editItem = new ChecklistItem
            {
                ChecklistItemId = itemId,
                UserOwner = ownerUser,
                OwnerId = userId,
                Title = "new-title",
                Description = "new-desc",
                IsComplete = true,
                Importance = ImportanceLevel.High,
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(userId))
                .ReturnsAsync(ownerUser);

            _checklistitemRepositoryMock
                .Setup(r => r.GetSingleChecklistItemAsync(userId, itemId))
                .ReturnsAsync(existing);

            _checklistitemRepositoryMock
                .Setup(r => r.EditChecklistItemAsync(It.IsAny<ChecklistItem>()))
                .ReturnsAsync((ChecklistItem i) => i);

            // Act
            var result = await _checklistitemService.EditChecklistItemAsync(userId, itemId, editItem);

            // Assert: 
            result.Should().NotBeNull();
            result.ChecklistItemId.Should().Be(itemId);
            result.OwnerId.Should().Be(userId);
            result.Title.Should().Be("new-title");
            result.Description.Should().Be("new-desc");
            result.IsComplete.Should().BeTrue();
            result.Importance.Should().Be(ImportanceLevel.High);

            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _checklistitemRepositoryMock.Verify(r => r.GetSingleChecklistItemAsync(userId, itemId), Times.Once);
            _checklistitemRepositoryMock.Verify(r => r.EditChecklistItemAsync(It.Is<ChecklistItem>(c =>
                c.ChecklistItemId == itemId &&
                c.Title == editItem.Title &&
                c.Description == editItem.Description &&
                c.IsComplete == editItem.IsComplete &&
                c.Importance == editItem.Importance
            )), Times.Once);
        }
    }
}