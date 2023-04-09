using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.DbContexts
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerDbContext).Assembly);

            var taskPriorityConverter = new EnumToStringConverter<TaskPriority>();
            var taskStatusConverter = new EnumToStringConverter<Domain.Entities.TaskStatus>();

            modelBuilder.Entity<TaskItem>()
                .Property(e => e.Priority)
                .HasConversion(taskPriorityConverter);

            modelBuilder.Entity<TaskItem>()
                .Property(e => e.Status)
                .HasConversion(taskStatusConverter);
        }
    }
}
