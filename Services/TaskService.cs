using DemoApi.Models;
using DemoApi.Repositories;

namespace DemoApi.Services
{
    public class TaskService : ITaskService 
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Task title cannot be empty.");
            }

            if (task.DueDate.HasValue && task.DueDate < DateTime.Now)
            {
                throw new ArgumentException("Due date cannot be in the past.");
            }

            return await _taskRepository.AddAsync(task);
        }

        public async Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Task title cannot be empty.");
            }

            return await _taskRepository.UpdateAsync(id, task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }
    }
   
}
