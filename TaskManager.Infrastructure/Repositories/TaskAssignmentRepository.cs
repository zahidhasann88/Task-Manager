using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.DbContexts;
using TaskManager.Shared.Utilities;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskAssignmentReposioty : ITaskAssignmentService
    {
        private readonly TaskManagerDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        public TaskAssignmentReposioty(TaskManagerDbContext dbContext, IUserService userService, ITaskService taskService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _taskService = taskService;
        }

        public async Task<IEnumerable<TaskAssignment>> GetAllAsync()
        {
            return await _dbContext.TaskAssignments
                .Include(t => t.AssignedToUser)
                .Include(t => t.Task)
                .ToListAsync();
        }

        public async Task<TaskAssignment> GetByIdAsync(int id)
        {
            return await _dbContext.TaskAssignments
                .Include(t => t.AssignedToUser)
                .Include(t => t.Task)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<TaskItem> GetTaskByIdAsync(int taskId)
        {
            return await _dbContext.Tasks.FindAsync(taskId);
        }

        public async Task<TaskAssignment> CreateAsync(TaskAssignment taskAssignment)
        {
            _dbContext.TaskAssignments.Add(taskAssignment);
            await _dbContext.SaveChangesAsync();

            // Send email notification to assigned user
            var assignedUser = await _userService.GetByIdAsync(taskAssignment.AssignedToUserId);
            var subject = $"New task assigned: {taskAssignment.Task.Title}";
            var body = $"Hello {assignedUser.Username},<br><br>You have been assigned a new task:<br><br>Name: {taskAssignment.Task.Title}<br>Description: {taskAssignment.Task.Description}<br>Due Date: {taskAssignment.Task.DueDate.ToString("d")}<br><br>Thank you,<br>Your Task Manager";
            EmailHelper.SendEmail(assignedUser.Email, subject, body);

            return taskAssignment;
        }
        public async Task UpdateAsync(TaskAssignment taskAssignment)
        {
            var existingTaskAssignment = await GetByIdAsync(taskAssignment.Id);
            if (existingTaskAssignment == null)
            {
                return; // Or throw an exception
            }

            // Check if the AssignedToUserId or IsDelegated properties have changed
            var isAssignedToUserIdChanged = taskAssignment.AssignedToUserId != existingTaskAssignment.AssignedToUserId;
            var isDelegatedChanged = taskAssignment.IsDelegated != existingTaskAssignment.IsDelegated;

            if (isAssignedToUserIdChanged)
            {
                existingTaskAssignment.AssignedToUserId = taskAssignment.AssignedToUserId;

                // Send email notification to the new assigned user
                var assignedUser = await _userService.GetByIdAsync(taskAssignment.AssignedToUserId);
                var task = await _taskService.GetByIdAsync(existingTaskAssignment.TaskId);
                var emailSubject = "Task assignment update";
                var emailBody = $"You have been assigned to the task '{task.Title}' by {assignedUser.Username}.";
                EmailHelper.SendEmail(assignedUser.Email, emailSubject, emailBody);
            }

            if (isDelegatedChanged)
            {
                existingTaskAssignment.IsDelegated = taskAssignment.IsDelegated;
            }

            existingTaskAssignment.AssignedDate = taskAssignment.AssignedDate.ToUniversalTime();
            _dbContext.Entry(existingTaskAssignment).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskAssignment taskAssi)
        {
                _dbContext.TaskAssignments.Remove(taskAssi);
                await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskAssignment>> GetAssignmentsByUserIdAsync(int userId)
        {
            return await _dbContext.TaskAssignments
                .Include(t => t.AssignedToUser)
                .Include(t => t.Task)
                .Where(t => t.AssignedToUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskAssignment>> GetAssignmentsByTaskIdAsync(int taskId)
        {
            return await _dbContext.TaskAssignments
                .Include(t => t.AssignedToUser)
                .Include(t => t.Task)
                .Where(t => t.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskAssignment>> GetPendingAssignmentsByUserIdAsync(int userId)
        {
            var currentDate = DateTime.UtcNow.Date;
            return await _dbContext.TaskAssignments
                .Include(t => t.AssignedToUser)
                .Include(t => t.Task)
                .Where(t => t.AssignedToUserId == userId && t.Task.DueDate >= currentDate && t.Task.Status != Domain.Entities.TaskStatus.Completed)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskAssignment>> GetOverdueAssignmentsByUserIdAsync(int userId)
        {
            var currentDate = DateTime.UtcNow.Date;
            return await _dbContext.TaskAssignments
                .Include(t => t.AssignedToUser)
                .Include(t => t.Task)
                .Where(t => t.AssignedToUserId == userId && t.Task.DueDate < currentDate && t.Task.Status != Domain.Entities.TaskStatus.Completed)
                .ToListAsync();
        }
    }

}
