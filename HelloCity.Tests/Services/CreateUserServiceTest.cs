using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using HelloCity.Services;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Api.DTOs.Users;
using HelloCity.Models.Enums;
using System;
using System.Threading.Tasks;
using HelloCity.Tests.Helpers;
using Microsoft.Extensions.Logging;


public class CreateUserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IMapper _mapper;
    private readonly UserService _userService;

    public CreateUserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UserTestProfile());
        }, new LoggerFactory());

        _mapper = config.CreateMapper();
        _userService = new UserService(_userRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnUserDto_WhenValidDtoProvided()
    {
        // Arrange

        var dto = new CreateUserDto
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

        var _userService = new UserService(_userRepositoryMock.Object, _mapper);

        // Act
        var result = await _userService.CreateUserAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be(dto.Username);
        result.Email.Should().Be(dto.Email);
        result.Gender.Should().Be(dto.Gender);
        _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<Users>()), Times.Once);
    }
}