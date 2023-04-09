using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    [Table("TaskItem")]
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("DueDate")]
        public DateTime DueDate { get; set; }
        [Column("Priority")]
        public TaskPriority Priority { get; set; }
        [Column("Status")]
        public TaskStatus Status { get; set; }
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Overdue
    }
}
