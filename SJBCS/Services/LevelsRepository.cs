using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class LevelsRepository : ILevelsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Level> AddLevelAsync(Level level)
        {
            _context.Levels.Add(level);
            await _context.SaveChangesAsync();
            return level;
        }

        public async Task DeleteLevelAsync(Guid levelId)
        {
            var level = _context.Levels.FirstOrDefault(l => l.LevelID == levelId);
            if (level != null)
            {
                _context.Levels.Remove(level);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Level> GetLevelAsync(Guid id)
        {
            return _context.Levels.FirstOrDefaultAsync(l => l.LevelID == id);
        }

        public Task<List<Level>> GetLevelsAsync()
        {
            return _context.Levels.ToListAsync();
        }

        public async Task<Level> UpdateLevelAsync(Level level)
        {
            if (!_context.Levels.Local.Any(l => l.LevelID == level.LevelID))
            {
                _context.Levels.Attach(level);
            }
            _context.Entry(level).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return level;
        }
    }
}
