using DemoApi.Dtos.Auth;
using DemoApi.Models;
using DemoApi.Repositories;
using DemoApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace DemoApi.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _repositoryMock = new();
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "unit-test-signing-key-thats-long-enough-1234567890",
                    ["Jwt:Issuer"] = "DemoApi.Tests",
                    ["Jwt:Audience"] = "DemoApi.Tests",
                    ["Jwt:ExpiryMinutes"] = "60"
                })
                .Build();

            _sut = new AuthService(_repositoryMock.Object, configuration);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ThrowsArgumentException()
        {
            _repositoryMock.Setup(r => r.GetByEmailAsync("taken@example.com"))
                .ReturnsAsync(new User { Id = 1, Email = "taken@example.com" });

            var dto = new RegisterDto { Name = "Name", Email = "taken@example.com", Password = "Passw0rd!" };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.RegisterAsync(dto));
        }

        [Fact]
        public async Task RegisterAsync_WithNewEmail_HashesPasswordAndReturnsToken()
        {
            _repositoryMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null);
            User? savedUser = null;
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => { u.Id = 1; savedUser = u; })
                .ReturnsAsync(() => savedUser!);

            var dto = new RegisterDto { Name = "New User", Email = "new@example.com", Password = "Passw0rd!" };

            var result = await _sut.RegisterAsync(dto);

            Assert.NotNull(savedUser);
            Assert.NotEqual(dto.Password, savedUser!.PasswordHash);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
            Assert.Equal("new@example.com", result.Email);
        }

        [Fact]
        public async Task LoginAsync_WithCorrectPassword_ReturnsToken()
        {
            var user = new User
            {
                Id = 1,
                Name = "Existing",
                Email = "existing@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct1!")
            };
            _repositoryMock.Setup(r => r.GetByEmailAsync("existing@example.com")).ReturnsAsync(user);

            var result = await _sut.LoginAsync(new LoginDto { Email = "existing@example.com", Password = "Correct1!" });

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result!.Token));
        }

        [Fact]
        public async Task LoginAsync_WithWrongPassword_ReturnsNull()
        {
            var user = new User
            {
                Id = 1,
                Email = "existing@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct1!")
            };
            _repositoryMock.Setup(r => r.GetByEmailAsync("existing@example.com")).ReturnsAsync(user);

            var result = await _sut.LoginAsync(new LoginDto { Email = "existing@example.com", Password = "Wrong" });

            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WithUnknownEmail_ReturnsNull()
        {
            _repositoryMock.Setup(r => r.GetByEmailAsync("nobody@example.com")).ReturnsAsync((User?)null);

            var result = await _sut.LoginAsync(new LoginDto { Email = "nobody@example.com", Password = "whatever" });

            Assert.Null(result);
        }
    }
}
