using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.DbContexts;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskService
    {
        private readonly TaskManagerDbContext _dbContext;

        public TaskRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateAsync(TaskItem taskItem)
        {
            _dbContext.Tasks.Add(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            try
            {
                var existingTask = await _dbContext.Tasks.FindAsync(taskItem.Id);

                if (existingTask == null)
                {
                    throw new ArgumentException("Task not found");
                }

                existingTask.Title = taskItem.Title;
                existingTask.Description = taskItem.Description;
                existingTask.DueDate = taskItem.DueDate;
                existingTask.Priority = taskItem.Priority;
                existingTask.Status = taskItem.Status;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or inspect its details for debugging purposes
                throw new ArgumentException("Error updating task: " + ex.Message);
            }
        }

        public async Task DeleteAsync(TaskItem taskItem)
        {
            _dbContext.Tasks.Remove(taskItem);
            await _dbContext.SaveChangesAsync();
        }

    }
}
