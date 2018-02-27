using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(String username);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(String username);
    }
}
