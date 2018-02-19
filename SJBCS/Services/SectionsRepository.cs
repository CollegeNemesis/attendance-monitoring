using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class SectionsRepository : ISectionsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Section> AddSectionAsync(Section section)
        {
            _context.Sections.Add(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task DeleteSectionAsync(Guid sectionID)
        {
            var section = _context.Sections.FirstOrDefault(s => s.SectionID == sectionID);
            if (section != null)
            {
                _context.Sections.Remove(section);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Section> GetSectionAsync(Guid id)
        {
            return _context.Sections.FirstOrDefaultAsync(s => s.SectionID == id);
        }

        public Task<List<Section>> GetSectionsAsync()
        {
            return _context.Sections.ToListAsync();
        }

        public async Task<Section> UpdateSectionAsync(Section section)
        {
            if (!_context.Sections.Local.Any(s => s.SectionID == section.SectionID))
            {
                _context.Sections.Attach(section);
            }
            _context.Entry(section).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return section;
        }
    }
}
