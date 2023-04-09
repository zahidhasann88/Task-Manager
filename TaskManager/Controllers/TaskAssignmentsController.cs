using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskAssignment>> GetTaskAssignment(int id)
        {
            var taskAssignment = await _taskAssignmentService.GetByIdAsync(id);

            if (taskAssignment == null)
            {
                return NotFound();
            }

            return taskAssignment;
        }


        //[HttpPost]
        //public async Task<ActionResult<TaskAssignment>> Create(TaskAssignment taskAssignment)
        //{
        //    var createdTaskAssignment = await _taskAssignmentService.CreateAsync(taskAssignment);
        //    return CreatedAtAction(nameof(GetById), new { id = createdTaskAssignment.Id }, createdTaskAssignment);
        //}
        [HttpPost]
        public async Task<ActionResult<TaskAssignment>> CreateTaskAssignment(TaskAssignmentDto taskAssignmentDto)
        {
            var taskAssignment = new TaskAssignment
            {
                TaskId = taskAssignmentDto.TaskId,
                AssignedToUserId = taskAssignmentDto.AssignedToUserId,
                AssignedDate = DateTime.UtcNow,
                IsDelegated = false
            };

            var createdTaskAssignment = await _taskAssignmentService.CreateAsync(taskAssignment);

            // Send email notification to the assigned user
            var assignedUser = await _taskAssignmentService.GetUserByIdAsync(createdTaskAssignment.AssignedToUserId);
            var task = await _taskAssignmentService.GetTaskByIdAsync(createdTaskAssignment.TaskId);
            var subject = $"You have been assigned a new task: {task.Title}";
            var body = $"Dear {assignedUser.Username},<br><br>You have been assigned a new task:<br><br>Name: {task.Title}<br>Description: {task.Description}<br>Due Date: {task.DueDate.ToString("d")}<br><br>Best regards,<br>Your Task Manager";
            EmailHelper.SendEmail(assignedUser.Email, subject, body);

            return CreatedAtAction(nameof(GetTaskAssignment), new { id = createdTaskAssignment.Id }, createdTaskAssignment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, TaskAssignmentUpdateDto taskAssignmentUpdateDto)
        {
            var taskAssignment = await _taskAssignmentService.GetByIdAsync(id);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var taskAssignment = await _taskAssignmentService.GetByIdAsync(id);
            if (taskAssignment == null)
            {
                return NotFound();
            }

            await _taskAssignmentService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("byUser/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetAssignmentsByUserId(int userId)
        {
            var taskAssignments = await _taskAssignmentService.GetAssignmentsByUserIdAsync(userId);
            return Ok(taskAssignments);
        }

        [HttpGet("byTask/{taskId}")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetAssignmentsByTaskId(int taskId)
        {
            var taskAssignments = await _taskAssignmentService.GetAssignmentsByTaskIdAsync(taskId);
            return Ok(taskAssignments);
        }

        [HttpGet("pendingByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetPendingAssignmentsByUserId(int userId)
        {
            var taskAssignments = await _taskAssignmentService.GetPendingAssignmentsByUserIdAsync(userId);
            return Ok(taskAssignments);
        }

        [HttpGet("overdueByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskAssignment>>> GetOverdueAssignmentsByUserId(int userId)
        {
            var taskAssignments = await _taskAssignmentService.GetOverdueAssignmentsByUserIdAsync(userId);
            return Ok(taskAssignments);
        }
    }
}
