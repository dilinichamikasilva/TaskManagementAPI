namespace DemoApi.Models
{
    public enum TaskItemStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
    }
}
