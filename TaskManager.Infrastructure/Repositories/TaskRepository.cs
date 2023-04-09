using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return await _dbContext.Tasks.FindAsync(id);
        }

        public async Task CreateAsync(TaskItem taskItem)
        {
            _dbContext.Tasks.Add(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            _dbContext.Entry(taskItem).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem taskItem)
        {
            _dbContext.Tasks.Remove(taskItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
