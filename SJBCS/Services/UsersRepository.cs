using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class UsersRepository : IUsersRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            await _context.SaveChangesAsync();
        }

        public Task<User> GetUserAsync(string username)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (!_context.Users.Local.Any(u => u.Username == user.Username))
            {
                _context.Users.Attach(user);
            }
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
