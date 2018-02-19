using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services
{
    public class GroupsRepository : IGroupsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public async Task<Organization> AddOrganizationAsync(Organization organization)
        {
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task DeleteOrganizationAsync(Guid organizationId)
        {
            var organization = _context.Organizations.FirstOrDefault(o => o.OrganizationID == organizationId);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            await _context.SaveChangesAsync();
        }

        public Task<Organization> GetOrganizationAsync(Guid id)
        {
            return _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationID == id);
        }

        public Task<List<Organization>> GetOrganizationsAsync()
        {
            return _context.Organizations.ToListAsync();
        }

        public async Task<Organization> UpdateOrganizationAsync(Organization organization)
        {
            if (!_context.Organizations.Local.Any(o => o.OrganizationID == organization.OrganizationID))
            {
                _context.Organizations.Attach(organization);
            }
            _context.Entry(organization).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return organization;
        }
    }
}
