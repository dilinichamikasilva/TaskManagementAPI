using DemoApi.Dtos.Projects;
using DemoApi.Dtos.Tasks;
using DemoApi.Dtos.Users;
using DemoApi.Models;

namespace DemoApi.Mapping
{
    public static class MappingExtensions
    {
        public static TaskDto ToDto(this TaskItem task) => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            AssignedUserId = task.AssignedUserId
        };

        public static TaskItem ToEntity(this CreateTaskDto dto) => new()
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId,
            AssignedUserId = dto.AssignedUserId
        };

        public static TaskItem ToEntity(this UpdateTaskDto dto) => new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId,
            AssignedUserId = dto.AssignedUserId
        };

        public static ProjectDto ToDto(this Project project) => new()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedDate = project.CreatedDate
        };

        public static Project ToEntity(this CreateProjectDto dto) => new()
        {
            Name = dto.Name,
            Description = dto.Description
        };

        public static Project ToEntity(this UpdateProjectDto dto) => new()
        {
            Name = dto.Name,
            Description = dto.Description
        };

        public static UserDto ToDto(this User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };

        public static User ToEntity(this UpdateUserDto dto) => new()
        {
            Name = dto.Name,
            Email = dto.Email
        };
    }
}
