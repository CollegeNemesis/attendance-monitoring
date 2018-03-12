using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SJBCS.Data;

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
