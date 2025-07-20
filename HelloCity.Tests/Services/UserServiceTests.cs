using AutoMapper;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Services;
using HelloCity.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;


namespace HelloCity.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserTestProfile());
            }, new LoggerFactory());

            _mapper = config.CreateMapper();
            _userService = new UserService(_userRepositoryMock.Object, _mapper);
        }

        /// <summary>
        /// test GetUserProfileAsync method in UserService
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserProfileAsync_ReturnMappedUserDto_WhenUserExists()
        {
            var userId = Guid.NewGuid();

            var testUser = TestUserFactory.CreateTestUser();
            testUser.UserId = userId;

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(testUser);
            
            var result = await _userService.GetUserProfileAsync(testUser.UserId);

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
