using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using HelloCity.Services;            
using HelloCity.IServices;           
using HelloCity.IRepository;         
using UserEntity = HelloCity.Models.Entities.Users;
using ChecklistItemEntity = HelloCity.Models.Entities.ChecklistItem;

namespace HelloCity.Tests.Services
{
    public class DeleteChecklistItemTest
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IChecklistItemRepository> _itemRepo = new();

        private IChecklistItemService CreateService()
            => new ChecklistItemService(_itemRepo.Object, _userRepo.Object);

        [Fact]
        public async Task Delete_Success_When_User_And_Item_Exist()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            _userRepo.Setup(r => r.GetUserByIdAsync(userId))
                     .ReturnsAsync(new UserEntity
                     {
                         UserId = userId,

                     });

            _itemRepo.Setup(r => r.GetSingleChecklistItemAsync(userId, itemId))
                     .ReturnsAsync(new ChecklistItemEntity
                     {
                         ChecklistItemId = itemId,
                         OwnerId = userId,
                         UserOwner = null!
                     });

            var svc = CreateService();

            await svc.DeleteChecklistItemAsync(userId, itemId);

            _itemRepo.Verify(r => r.DeleteChecklistItemAsync(
                It.Is<ChecklistItemEntity>(c => c.ChecklistItemId == itemId && c.OwnerId == userId)),
                Times.Once);
        }

        [Fact]
        public async Task Delete_Throws_When_User_Not_Found()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            _userRepo.Setup(r => r.GetUserByIdAsync(userId))
                     .ReturnsAsync((UserEntity?)null);

            var svc = CreateService();

            await FluentActions
                .Invoking(() => svc.DeleteChecklistItemAsync(userId, itemId))
                .Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*User*not found*");
        }

        [Fact]
        public async Task Delete_Throws_When_Item_Not_Found()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            _userRepo.Setup(r => r.GetUserByIdAsync(userId))
                     .ReturnsAsync(new UserEntity { UserId = userId });

            _itemRepo.Setup(r => r.GetSingleChecklistItemAsync(userId, itemId))
                     .ReturnsAsync((ChecklistItemEntity?)null);

            var svc = CreateService();

            await FluentActions
                .Invoking(() => svc.DeleteChecklistItemAsync(userId, itemId))
                .Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*ChecklistItem*not found*");
        }
    }
}
