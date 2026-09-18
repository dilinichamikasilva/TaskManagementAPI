using DemoApi.Models;
using DemoApi.Repositories;
using DemoApi.Services;
using Moq;
using Xunit;

namespace DemoApi.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repositoryMock = new();
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _sut = new UserService(_repositoryMock.Object);
        }

        [Fact]
        public async Task UpdateUserAsync_WithEmptyName_ThrowsArgumentException()
        {
            var user = new User { Name = "", Email = "a@b.com" };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateUserAsync(1, user));
        }

        [Fact]
        public async Task UpdateUserAsync_WithInvalidEmail_ThrowsArgumentException()
        {
            var user = new User { Name = "Name", Email = "not-an-email" };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateUserAsync(1, user));
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidData_DelegatesToRepository()
        {
            var user = new User { Name = "Name", Email = "a@b.com" };
            _repositoryMock.Setup(r => r.UpdateAsync(1, user)).ReturnsAsync(user);

            var result = await _sut.UpdateUserAsync(1, user);

            Assert.Same(user, result);
            _repositoryMock.Verify(r => r.UpdateAsync(1, user), Times.Once);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsRepositoryResult()
        {
            var users = new List<User> { new() { Id = 1, Name = "A", Email = "a@b.com" } };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _sut.GetAllUsersAsync();

            Assert.Same(users, result);
        }
    }
}
