namespace TaskManager.DTOs
{
    public class TaskAssignmentDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int AssignedToUserId { get; set; }
        public UserCreateDto AssignedToUser { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool IsDelegated { get; set; }
    }
    public class TaskAssignmentUpdateDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int AssignedToUserId { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool IsDelegated { get; set; }
    }

    public class TaskAssignmentIdDto
    {
        public int Id { get; set; }
    }
    public class TaskAssignDeleteDto
    {
        public int Id { get; set; }
    }
    public class TaskAssignUserIdDto
    {
        public int userId { get; set; }
    }

    public class TaskAssignTaskIdDto
    {
        public int taskId { get; set; }
    }
    public class TaskAssiPendingUserIdDto
    {
        public int userId { get; set; }
    }
    public class TaskAssiOverdueUserIdDto
    {
        public int userId { get; set; }
    }


}
