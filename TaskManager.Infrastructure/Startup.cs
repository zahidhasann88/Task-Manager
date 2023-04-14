using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
            services.AddScoped<IJwtService, JwtRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]))
        };
        });


            // Add other required services, e.g. DbContext
            services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("TaskManagerDatabase")));
        }
    }
}
