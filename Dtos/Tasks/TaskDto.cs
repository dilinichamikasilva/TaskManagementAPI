using DemoApi.Models;

namespace DemoApi.Dtos.Tasks
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public int? AssignedUserId { get; set; }
    }
}
