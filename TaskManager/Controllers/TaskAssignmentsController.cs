using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.DTOs;
using TaskManager.Shared.Utilities;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAssignmentsController : ControllerBase
    {
        private readonly ITaskAssignmentService _taskAssignmentService;
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        public TaskAssignmentsController(ITaskAssignmentService taskAssignmentService, IUserService userService, ITaskService taskService)
        {
            _taskAssignmentService = taskAssignmentService;
            _userService = userService;
            _taskService = taskService;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetAll()
        {
            var taskAssignments = await _taskAssignmentService.GetAllAsync();
            return Ok(taskAssignments);
        }

        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetById([FromBody] TaskAssignmentIdDto taskAssignmentIdDto)
        {
            var taskAssignment = await _taskAssignmentService.GetByIdAsync(taskAssignmentIdDto.Id);

            if (taskAssignment == null)
            {
                return NotFound();
            }

            return Ok(taskAssignment);
        }

        [HttpPost("create-taskassignment")]
        public async Task<ActionResult<TaskAssignment>> CreateTaskAssignment(TaskAssignmentDto taskAssignmentDto)
        {
            if (taskAssignmentDto == null)
            {
                return BadRequest("Task assignment cannot be null.");
            }

            if (taskAssignmentDto.TaskId == 0)
            {
                return BadRequest("Task Id cannot be zero.");
            }

            if (taskAssignmentDto.AssignedToUserId == null)
            {
                return BadRequest("Assigned user Id cannot be null.");
            }

            var task = await _taskAssignmentService.GetTaskByIdAsync(taskAssignmentDto.TaskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            var assignedUser = await _taskAssignmentService.GetUserByIdAsync(taskAssignmentDto.AssignedToUserId);
            if (assignedUser == null)
            {
                return NotFound("Assigned user not found.");
            }

            var taskAssignment = new TaskAssignment
            {
                TaskId = taskAssignmentDto.TaskId,
                AssignedToUserId = taskAssignmentDto.AssignedToUserId,
                AssignedDate = DateTime.UtcNow,
                IsDelegated = false
            };

            var createdTaskAssignment = await _taskAssignmentService.CreateAsync(taskAssignment);

            if (createdTaskAssignment == null)
            {
                return BadRequest("Could not create task assignment.");
            }

            // Send email notification to the assigned user
            var subject = $"You have been assigned a new task: {task.Title}";
            var body = $"Dear {assignedUser.Username},<br><br>You have been assigned a new task:<br><br>Name: {task.Title}<br>Description: {task.Description}<br>Due Date: {task.DueDate.ToString("d")}<br><br>Best regards,<br>Your Task Manager";
            EmailHelper.SendEmail(assignedUser.Email, subject, body);

            return CreatedAtAction("GetTaskAssignment", new { id = createdTaskAssignment.Id }, createdTaskAssignment);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] TaskAssignmentUpdateDto taskAssignmentUpdateDto)
        {
            var taskAssignment = await _taskAssignmentService.GetByIdAsync(taskAssignmentUpdateDto.Id);
            if (taskAssignment == null)
            {
                return NotFound();
            }

            taskAssignment.AssignedToUserId = taskAssignmentUpdateDto.AssignedToUserId;
            taskAssignment.IsDelegated = taskAssignmentUpdateDto.IsDelegated;

            await _taskAssignmentService.UpdateAsync(taskAssignment);

            // Send email notification to the assigned user
            var assignedUser = await _userService.GetByIdAsync(taskAssignment.AssignedToUserId);
            var task = await _taskService.GetByIdAsync(taskAssignment.TaskId);
            var emailSubject = "Task updated";
            var emailBody = $"The task '{task.Title}' assigned to you has been updated by {User.Identity.Name}.";

            EmailHelper.SendEmail(assignedUser.Email, emailSubject, emailBody);

            return NoContent();
        }

        [HttpDelete("delete-task-assignment")]
        public async Task<IActionResult> Delete([FromBody] TaskAssignDeleteDto taskassigndeleteDto)
        {
            try
            {
                var taskAssignment = await _taskAssignmentService.GetByIdAsync(taskassigndeleteDto.Id);
                if (taskAssignment == null)
                {
                    return NotFound();
                }
                await _taskAssignmentService.DeleteAsync(taskAssignment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("by-user/user-id")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetAssignmentsByUserId([FromBody] TaskAssignUserIdDto taskAssignUserIdDto)
        {
            var taskAssignments = await _taskAssignmentService.GetAssignmentsByUserIdAsync(taskAssignUserIdDto.userId);
            return Ok(taskAssignments);
        }

        [HttpPost("by-ask/task-id")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetAssignmentsByTaskId([FromBody] TaskAssignTaskIdDto taskAssignTaskIdDto)
        {
            var taskAssignments = await _taskAssignmentService.GetAssignmentsByTaskIdAsync(taskAssignTaskIdDto.taskId);
            return Ok(taskAssignments);
        }

        [HttpPost("pending-by-user/user-id")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetPendingAssignmentsByUserId([FromBody] TaskAssiPendingUserIdDto taskAssiPendingUserIdDto)
        {
            var taskAssignments = await _taskAssignmentService.GetPendingAssignmentsByUserIdAsync(taskAssiPendingUserIdDto.userId);
            return Ok(taskAssignments);
        }

        [HttpPost("overdue-by-user/user-id")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetOverdueAssignmentsByUserId([FromBody] TaskAssiOverdueUserIdDto taskAssiOverdueUserIdDto)
        {
            var taskAssignments = await _taskAssignmentService.GetOverdueAssignmentsByUserIdAsync(taskAssiOverdueUserIdDto.userId);
            return Ok(taskAssignments);
        }
    }
}
