using SJBCS.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class UsersRepository : IUsersRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public User AddUser(User User)
        {
            //_context = ConnectionHelper.CreateConnection();
            _context.Users.Add(User);
            _context.SaveChanges();
            return User;
        }

        public void DeleteUser(string Username)
        {
            _context = ConnectionHelper.CreateConnection();
            var User = _context.Users.FirstOrDefault(r => r.Username == Username);
            if (User != null)
            {
                _context.Users.Remove(User);
            }
            _context.SaveChanges();
        }

        public User GetUser(string Username)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Users.FirstOrDefault(r => r.Username == Username);
        }

        public List<User> GetUsers()
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Users.ToList();
        }

        public User UpdateUser(User User)
        {
            //_context = ConnectionHelper.CreateConnection();
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
