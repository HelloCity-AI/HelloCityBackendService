using Xunit;
using Moq;
using FluentAssertions;
using HelloCity.Services;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Models.Enums;
using System;
using System.Threading.Tasks;
using HelloCity.Tests.Helpers;
using Microsoft.Extensions.Logging;

namespace HelloCity.Tests.Services
{
    public class EditUserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public EditUserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task EditUserAsync_ShouldReturnUpdatedUser_WhenValidUserProvided()
        {
            var userId = Guid.NewGuid();

            var existingUser = new Users
            {
                UserId = userId,
                Username = "string222",
                Email = "string222333@example.com",
                Password = "P@ssword123",
                Gender = Gender.Male,
                Nationality = "Australia",
                City = "Sydney",
                PreferredLanguage = PreferredLanguage.en,
                LastJoinDate = DateTime.UtcNow
            };

            var updatedUser = new Users
            {
                UserId = userId,
                Username = "string",
                Email = "string@example.com",
                Gender = Gender.Male,
                Nationality = "Australia",
                City = "Sydney",
                PreferredLanguage = PreferredLanguage.en
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<Users>()))
                               .ReturnsAsync((Users u) => u);

            // Act
            var result = await _userService.EditUserAsync(userId, updatedUser);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("string");
            result.Gender.Should().Be(Gender.Male);
            result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<Users>()), Times.Once);
        }

        [Fact]
        public async Task EditUserAsync_ShouldThrowException_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            var updatedUser = new Users
            {
                UserId = userId,
                Username = "string",
                Email = "string@example.com",
                Gender = Gender.Male,
                Nationality = "Australia",
                City = "Sydney",
                PreferredLanguage = PreferredLanguage.en
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync((Users?)null);

            Func<Task> act = async() => await _userService.EditUserAsync(userId, updatedUser);

            //assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("User not found");

            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<Users>()), Times.Never);
        }
    }
}