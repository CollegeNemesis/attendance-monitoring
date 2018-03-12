using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public class UsersRepository : IUsersRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public User AddUser(User User)
        {
            _context.Users.Add(User);
            _context.SaveChanges();
            return User;
        }

        public void DeleteUser(string Username)
        {
            var User = _context.Users.FirstOrDefault(r => r.Username == Username);
            if (User != null)
            {
                _context.Users.Remove(User);
            }
            _context.SaveChanges();
        }

        public User GetUser(string Username)
        {
            return _context.Users.FirstOrDefault(r => r.Username == Username);
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User UpdateUser(User User)
        {
            if (!_context.Users.Local.Any(r => r.UserID == User.UserID))
            {
                _context.Users.Attach(User);
            }
            _context.Entry(User).State = EntityState.Modified;
            _context.SaveChanges();
            return User;
        }
    }
}
