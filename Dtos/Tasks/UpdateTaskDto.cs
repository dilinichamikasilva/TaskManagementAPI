using System.ComponentModel.DataAnnotations;
using DemoApi.Models;

namespace DemoApi.Dtos.Tasks
{
    public class UpdateTaskDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public TaskItemStatus Status { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
