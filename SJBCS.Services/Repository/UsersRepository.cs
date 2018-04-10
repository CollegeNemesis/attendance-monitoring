using SJBCS.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class UsersRepository : IUsersRepository
    {
        AmsModel _context;

        public User AddUser(User User)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Users.Add(User);
                _context.SaveChanges();

                return User;
            }
        }

        public void DeleteUser(string Username)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var User = _context.Users.FirstOrDefault(r => r.Username == Username);
                _context.Entry(User).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public User GetUser(string Username)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var User = _context.Users.FirstOrDefault(r => r.Username == Username);
                return User;
            }
        }

        public List<User> GetUsers()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Users = _context.Users.ToList();

                return Users;
            }
        }

        public User UpdateUser(User User)
        {
            using (_context = ConnectionHelper.CreateConnection())
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
}
