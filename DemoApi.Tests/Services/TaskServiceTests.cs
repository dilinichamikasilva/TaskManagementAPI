using DemoApi.Models;
using DemoApi.Repositories;
using DemoApi.Services;
using Moq;
using Xunit;

namespace DemoApi.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock = new();
        private readonly TaskService _sut;

        public TaskServiceTests()
        {
            _sut = new TaskService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_WithEmptyTitle_ThrowsArgumentException()
        {
            var task = new TaskItem { Title = "  ", ProjectId = 1 };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateTaskAsync(task));
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task CreateTaskAsync_WithPastDueDate_ThrowsArgumentException()
        {
            var task = new TaskItem { Title = "Task", ProjectId = 1, DueDate = DateTime.Now.AddDays(-1) };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateTaskAsync(task));
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task CreateTaskAsync_WithValidTask_AddsToRepository()
        {
            var task = new TaskItem { Title = "Task", ProjectId = 1 };
            _repositoryMock.Setup(r => r.AddAsync(task)).ReturnsAsync(task);

            var result = await _sut.CreateTaskAsync(task);

            Assert.Same(task, result);
            _repositoryMock.Verify(r => r.AddAsync(task), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_WithEmptyTitle_ThrowsArgumentException()
        {
            var task = new TaskItem { Title = "", ProjectId = 1 };

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateTaskAsync(1, task));
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task GetTaskByIdAsync_WhenNotFound_ReturnsNull()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TaskItem?)null);

            var result = await _sut.GetTaskByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTaskAsync_DelegatesToRepository()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _sut.DeleteTaskAsync(1);

            Assert.True(result);
        }
    }
}
