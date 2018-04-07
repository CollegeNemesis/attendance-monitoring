using SJBCS.Data;
using System.Collections.Generic;

namespace SJBCS.Services.Repository
{
    public interface IUsersRepository
    {
        List<User> GetUsers();
        User GetUser(string Username);
        User AddUser(User User);
        User UpdateUser(User User);
        void DeleteUser(string Username);
    }
}
