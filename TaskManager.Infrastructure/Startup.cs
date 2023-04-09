using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.DbContexts;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register your services here
            services.AddScoped<IUserService, UserRepository>();
            services.AddScoped<ITaskService, TaskRepository>();
            services.AddScoped<ITaskAssignmentService, TaskAssignmentReposioty>();

            // Add other required services, e.g. DbContext
            services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("TaskManagerDatabase")));
        }
    }
}
