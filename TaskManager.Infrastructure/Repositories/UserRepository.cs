using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.DbContexts;
using TaskManager.Shared.Helpers;

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
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash != null ? Convert.ToBase64String(passwordHash) : null;
            user.PasswordSalt = passwordSalt != null ? Convert.ToBase64String(passwordSalt) : null;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task UpdateAsync(User user, string password = null)
        {
            var userToUpdate = await _context.Users.FindAsync(user.Id);
            if (userToUpdate == null)
                throw new AppException("User not found");

            // Update the user properties
            userToUpdate.Username = user.Username;
            userToUpdate.Email = user.Email;
            userToUpdate.Role = user.Role;

            // Update the password hash if a new password was provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                userToUpdate.PasswordHash = Convert.ToBase64String(passwordHash); //dynamic hash - bycrpt ( sha/rsa/md5)
                userToUpdate.PasswordSalt = Convert.ToBase64String(passwordSalt);
            }

            // Exclude the Id property from being modified
            _context.Entry(userToUpdate).Property(x => x.Id).IsModified = false;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == passwordHash;
            }
        }

    }

}
