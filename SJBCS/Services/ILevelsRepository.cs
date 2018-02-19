using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SJBCS.Services
{
    interface ILevelsRepository
    {
        Task<List<Level>> GetLevelsAsync();
        Task<Level> GetLevelAsync(Guid id);
        Task<Level> AddLevelAsync(Level level);
        Task<Level> UpdateLevelAsync(Level level);
        Task DeleteLevelAsync(Guid levelId);
    }
}
