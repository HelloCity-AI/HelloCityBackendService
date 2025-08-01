using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using HelloCity.Services;
using HelloCity.IRepository;
using HelloCity.Models.Entities;
using HelloCity.Models.DTOs.Users;
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
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public EditUserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            //learn how to write mapper unit test
            var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UserTestProfile());
        }, new LoggerFactory());
            _mapper = config.CreateMapper();

            _userService = new UserService(_userRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task EditUserAsync_ShouldReturnUserDto_WhenValidDtoProvided()
        {
            // Arrange
            var guid = Guid.Parse("cb2b4fff-adc0-4515-aebb-1b46f19bf33e");
            var dto = new UserInfoCollectionDTO
            {
                Username = "string222",
                Email = "string222333@example.com",
                Password = "P@ssword123",
                Gender = Gender.Male,
                Nationality = "Australia",
                City = "Sydney",
                PreferredLanguage = PreferredLanguage.en,
                LastJoinDate = DateTime.UtcNow
            };

            var user = new Users
            {
                UserId = guid,
                Username = "string",
                Email = "string@example.com",
                Gender = Gender.Male,
                Nationality = "Australia",
                City = "Sydney",
                PreferredLanguage = PreferredLanguage.en,
                LastJoinDate = DateTime.UtcNow
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(guid)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<Users>()))
                               .ReturnsAsync((Users u) => u);

            // Act
            var result = await _userService.EditUserAsync(guid, dto);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(dto.Username);
            result.Email.Should().Be(dto.Email);
            result.Gender.Should().Be(dto.Gender);
            _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<Users>()), Times.Once);
        }
    }
}