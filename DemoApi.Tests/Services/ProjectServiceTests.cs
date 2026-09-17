using DemoApi.Models;
using DemoApi.Repositories;
using DemoApi.Services;
using Moq;
using Xunit;

namespace DemoApi.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _repositoryMock = new();
        private readonly ProjectService _sut;

        public ProjectServiceTests()
        {
            _sut = new ProjectService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_WithEmptyName_ThrowsArgumentException()
        {
            var project = new Project { Name = "   " };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateProjectAsync(project));
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task CreateProjectAsync_WithValidName_AddsToRepository()
        {
            var project = new Project { Name = "New Project" };
            _repositoryMock.Setup(r => r.AddAsync(project)).ReturnsAsync(project);

            var result = await _sut.CreateProjectAsync(project);

            Assert.Same(project, result);
            _repositoryMock.Verify(r => r.AddAsync(project), Times.Once);
        }

        [Fact]
        public async Task UpdateProjectAsync_WithEmptyName_ThrowsArgumentException()
        {
            var project = new Project { Name = "" };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateProjectAsync(1, project));
        }

        [Fact]
        public async Task DeleteProjectAsync_WhenNotFound_ReturnsFalse()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync(false);

            var result = await _sut.DeleteProjectAsync(42);

            Assert.False(result);
        }
    }
}
