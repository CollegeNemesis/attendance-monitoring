using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public class SectionsRepository : ISectionsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public Section AddSection(Section Section)
        {
            _context.Sections.Add(Section);
            _context.SaveChanges();
            return Section;
        }

        public void DeleteSection(Guid id)
        {
            var Section = _context.Sections.FirstOrDefault(r => r.SectionID == id);
            if (Section != null)
            {
                _context.Sections.Remove(Section);
            }
            _context.SaveChanges();
        }

        public Section GetSection(Guid id)
        {
            return _context.Sections.FirstOrDefault(r => r.SectionID == id);
        }

        public List<Section> GetSections(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Sections.Where(r => r.LevelID == id).ToList();
        }

        public Section UpdateSection(Section Section)
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
