using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Services;
using HelloCity.Tests.Helpers;
using Moq;
using FluentAssertions;


namespace HelloCity.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        /// <summary>
        /// test GetUserProfileAsync method in UserService
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserProfileAsync_ReturnUserEntity_WhenUserExists()
        {
            var userId = Guid.NewGuid();

            var testUser = TestUserFactory.CreateTestUser();
            testUser.UserId = userId;

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(testUser);
            
            var result = await _userService.GetUserProfileAsync(userId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(testUser, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// test GetUserProfileAsync method in UserService when user does not exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserProfileAsync_ReturnNull_WhenUserDoesNotFound()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((Users?)null);

            var result = await _userService.GetUserProfileAsync(userId);

            result.Should().BeNull();
        }
    }
}
