using Xunit;
using Moq;
using FluentAssertions;
using HelloCity.Services;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Models.Enums;
using System;
using System.Threading.Tasks;



public class CreateUserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public CreateUserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnUserEntity_WhenUserIsCreatedSuccessfully()
    {
        // Arrange

        var user = new Users
        {
            Username = "john_dev",
            Email = "john@example.com",
            Password = "P@ssword123",
            Gender = Gender.Male,
            Nationality = "Australia",
            City = "Sydney",
            PreferredLanguage = PreferredLanguage.en,
            LastJoinDate = DateTime.UtcNow
        };

        // mock Repository
        _userRepositoryMock.Setup(r => r.AddUserAsync(It.IsAny<Users>()))
        .ReturnsAsync((Users u) => u);

        // Act
        var result = await _userService.CreateUserAsync(user);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be(user.Username);
        result.Email.Should().Be(user.Email);
        result.Gender.Should().Be(user.Gender);
        _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<Users>()), Times.Once);
    }
}