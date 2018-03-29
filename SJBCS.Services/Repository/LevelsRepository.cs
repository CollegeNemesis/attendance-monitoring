using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public class LevelsRepository : ILevelsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public Level AddLevel(Level Level)
        {
            _context.Levels.Add(Level);
            _context.SaveChanges();
            return Level;
        }

        public void DeleteLevel(Guid id)
        {
            var Level = _context.Levels.FirstOrDefault(r => r.LevelID == id);
            if (Level != null)
            {
                _context.Levels.Remove(Level);
            }
            _context.SaveChanges();
        }

        public Level GetLevel(Guid id)
        {
            return _context.Levels.FirstOrDefault(r => r.LevelID == id);
        }

        public List<Level> GetLevels()
        {
            _context = ConnectionHelper.CreateConnection();            
            return _context.Levels.AsEnumerable().OrderBy(level => level.GradeLevel, new NaturalSortComparer<string>()).ToList();
        }

        public Level UpdateLevel(Level Level)
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
