using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class LevelsRepository : ILevelsRepository
    {
        AmsModel _context;

        public Level AddLevel(Level Level)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Levels.Add(Level);
                _context.SaveChanges();

                return Level;
            }
        }

        public void DeleteLevel(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Level = _context.Levels.FirstOrDefault(r => r.LevelID == id);
                _context.Entry(Level).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public Level GetLevel(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Level = _context.Levels
                    .Include(level => level.Students)
                    .Include(level => level.Sections)
                    .FirstOrDefault(level => level.LevelID == id);

                return Level;
            }
        }

        public List<Level> GetLevels()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Levels = _context.Levels
                    .Include(level => level.Students)
                    .Include(level => level.Sections)
                    .AsEnumerable().OrderBy(level => level.LevelOrder)
                    .ToList();

                return Levels;
            }
        }

        public Level UpdateLevel(Level Level)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.Levels.Local.Any(r => r.LevelID == Level.LevelID))
                {
                    _context.Levels.Attach(Level);
                }
                _context.Entry(Level).State = EntityState.Modified;
                _context.SaveChanges();

                return Level;
            }
        }
    }
}
