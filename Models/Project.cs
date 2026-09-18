namespace DemoApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property - one Project has many Tasks
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
