using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Entities;

namespace TaskManager.DTOs
{
    public class DeleteTaskDTO
    {
        public int Id { get; set; }
    }
    public class UpdateTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Domain.Entities.TaskStatus Status { get; set; }
    }
    public class GetByIdTaskDto
    {
        public int Id { get; set; }
    }

}
