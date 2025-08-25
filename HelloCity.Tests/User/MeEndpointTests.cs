using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;

using HelloCity.Api;                    
using HelloCity.Models;
using HelloCity.Models.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; 
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace HelloCity.Tests.User
{
	public class MeEndpointTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;
		private static readonly ServiceProvider InMemoryEfProvider =
			new ServiceCollection()
				.AddEntityFrameworkInMemoryDatabase()
				.BuildServiceProvider();

		public MeEndpointTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					// 1) only for testing, remove existing DbContext registration
					services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
					services.RemoveAll<AppDbContext>();

					// 2) let the DbContext use InMemory database
					services.AddDbContext<AppDbContext>(opt =>
					{
						opt.UseInMemoryDatabase("UserDb_Test");
						// let the DbContext use the InMemoryEfProvider
						opt.UseInternalServiceProvider(InMemoryEfProvider);
					});

					// 3) test authentication handler
					services.AddAuthentication(options =>
					{
						options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
						options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
					})
					.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
						TestAuthHandler.SchemeName, _ => { });

					// 4) data seeding
					using var sp = services.BuildServiceProvider();
					using var scope = sp.CreateScope();
					var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
					db.Database.EnsureCreated();

					db.Users.Add(new Users
					{
						Username = "Alice",
						Email = "alice@example.com",
						SubId = TestAuthHandler.ExistingSub,
						Avatar = "https://via.placeholder.com/150",
						Gender = HelloCity.Models.Enums.Gender.PreferNotToSay,
						PreferredLanguage = HelloCity.Models.Enums.PreferredLanguage.en,
						LastJoinDate = DateTime.UtcNow
					});
					db.SaveChanges();
				});
			});
		}

		[Fact]
		public async Task Me_WhenUserExists_Returns200_AndUserDto()
		{
			var client = _factory.CreateClient();

			var res = await client.GetAsync("/api/user/me");
			Assert.Equal(HttpStatusCode.OK, res.StatusCode);

			var dto = await res.Content.ReadFromJsonAsync<UserDtoLike>();
			Assert.NotNull(dto);
			Assert.Equal("Alice", dto!.Username);
			Assert.Equal("alice@example.com", dto.Email);
		}

		[Fact]
		public async Task Me_WhenUserNotExists_Returns404()
		{
			// use a different factory with a handler that simulates a user not found
			var factoryNoUser = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					services.AddAuthentication(options =>
					{
						options.DefaultAuthenticateScheme = TestAuthHandler_NoUser.SchemeName;
						options.DefaultChallengeScheme = TestAuthHandler_NoUser.SchemeName;
					})
					.AddScheme<AuthenticationSchemeOptions, TestAuthHandler_NoUser>(
						TestAuthHandler_NoUser.SchemeName, _ => { });
				});
			});

			var client = factoryNoUser.CreateClient();
			var res = await client.GetAsync("/api/user/me");

			Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
		}

		// ！！ only for testing, a simplified UserDto for assertions ！！ //
		private class UserDtoLike
		{
			public Guid UserId { get; set; }
			public string Username { get; set; } = "";
			public string Email { get; set; } = "";
		}

		// ！！ test custom authentication handler (exists user) ！！ //
#pragma warning disable CS0618
		private class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
		{
			public const string SchemeName = "TestAuth";
			public const string ExistingSub = "auth0|test-sub-123";

			public TestAuthHandler(
				IOptionsMonitor<AuthenticationSchemeOptions> options,
				ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
				: base(options, logger, encoder, clock) { }

			protected override Task<AuthenticateResult> HandleAuthenticateAsync()
			{
				var claims = new[] { new Claim("sub", ExistingSub) };
				var identity = new ClaimsIdentity(claims, SchemeName);
				var principal = new ClaimsPrincipal(identity);
				var ticket = new AuthenticationTicket(principal, SchemeName);
				return Task.FromResult(AuthenticateResult.Success(ticket));
			}
		}

		// ！！ test custom authentication handler (no user found) ！！ //
		private class TestAuthHandler_NoUser : AuthenticationHandler<AuthenticationSchemeOptions>
		{
			public const string SchemeName = "TestAuth_NoUser";

			public TestAuthHandler_NoUser(
				IOptionsMonitor<AuthenticationSchemeOptions> options,
				ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
				: base(options, logger, encoder, clock) { }

			protected override Task<AuthenticateResult> HandleAuthenticateAsync()
			{
				var claims = new[] { new Claim("sub", "auth0|not-exists-999") };
				var identity = new ClaimsIdentity(claims, SchemeName);
				var principal = new ClaimsPrincipal(identity);
				var ticket = new AuthenticationTicket(principal, SchemeName);
				return Task.FromResult(AuthenticateResult.Success(ticket));
			}
		}
		#pragma warning restore CS0618
	}
}
