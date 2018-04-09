using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class SectionsRepository : ISectionsRepository
    {
        AmsModel _context;

        public Section AddSection(Section Section)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Sections.Add(Section);
                _context.SaveChanges();
            }
            return Section;
        }

        public void DeleteSection(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Section = _context.Sections.FirstOrDefault(r => r.SectionID == id);
                _context.Entry(Section).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public Section GetSection(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Section = _context.Sections
                    .Include(section => section.Students)
                    .Include(section => section.Level)
                    .FirstOrDefault(r => r.SectionID == id);

                return Section;
            }
        }

        public List<Section> GetSections(Guid id)
        {
          using (_context = ConnectionHelper.CreateConnection())
            {
                var Sections = _context.Sections
                    .Where(r => r.LevelID == id)
                    .Include(section => section.Students)
                    .Include(section => section.Level)
                    .OrderBy(section => section.SectionName)
                    .ToList();

                return Sections;
            }
        }

        public List<Section> GetSections()
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Sections = _context.Sections
                    .Include(section => section.Students)
                    .Include(section => section.Level)
                    .OrderBy(section => section.SectionName)
                    .ToList();

                return Sections;
            }
        }

        public Section UpdateSection(Section Section)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.Sections.Local.Any(r => r.SectionID == Section.SectionID))
                {
                    _context.Sections.Attach(Section);
                }
                _context.Entry(Section).State = EntityState.Modified;
                _context.SaveChanges();

                return Section;
            }
        }
    }
}
