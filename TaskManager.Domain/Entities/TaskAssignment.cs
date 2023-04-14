using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    [Table("TaskAssignment")]
    public class TaskAssignment
    {
        [Key]
        public int Id { get; set; }

        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public virtual TaskItem Task { get; set; }

        [Column("AssignedToUserId")]
        public int AssignedToUserId { get; set; }

        [ForeignKey(nameof(AssignedToUserId))]
        public virtual User AssignedToUser { get; set; }

        [Column("AssignedDate")]
        public DateTime AssignedDate { get; set; }

        [Column("IsDelegated")]
        public bool IsDelegated { get; set; }
    }
}
