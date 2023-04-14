using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskAssignmentService
    {
        Task<IEnumerable<TaskAssignment>> GetAllAsync();
        Task<TaskAssignment> GetByIdAsync(int id);
        Task<TaskAssignment> CreateAsync(TaskAssignment taskAssignment);
        Task UpdateAsync(TaskAssignment taskAssignment);
        Task DeleteAsync(TaskAssignment taskAssignment);
        Task<IEnumerable<TaskAssignment>> GetAssignmentsByUserIdAsync(int userId);
        Task<IEnumerable<TaskAssignment>> GetAssignmentsByTaskIdAsync(int taskId);
        Task<IEnumerable<TaskAssignment>> GetPendingAssignmentsByUserIdAsync(int userId);
        Task<IEnumerable<TaskAssignment>> GetOverdueAssignmentsByUserIdAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
        Task<TaskItem> GetTaskByIdAsync(int taskId);
    }
}
