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
    public class UserRepository : IUserService
    {
        private readonly TaskManagerDbContext _context;

        public UserRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            // TODO: Hash the password and set the PasswordHash property on the user object
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user, string password = null)
        {
            // TODO: Update the password hash if a new password was provided
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await UpdateAsync(user);
            }
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // TODO: Hash the password and compare it to the stored password hash
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.PasswordHash == password);
        }
    }

}
