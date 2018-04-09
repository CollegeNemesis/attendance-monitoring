using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class OrganizationsRepository : IOrganizationsRepository
    {
        AmsModel _context;

        public Organization AddOrganization(Organization Organization)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.Organizations.Add(Organization);
                _context.SaveChanges();
            }
            return Organization;
        }

        public void DeleteOrganization(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var Organization = _context.Organizations.FirstOrDefault(r => r.OrganizationID == id);
                _context.Entry(Organization).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public Organization GetOrganization(Guid id)
        {
            Organization Organization;

            using (_context = ConnectionHelper.CreateConnection())
            {
                Organization = _context.Organizations.FirstOrDefault(r => r.OrganizationID == id);
            }
            return Organization;
        }

        public List<Organization> GetOrganizations()
        {
            List<Organization> Organizations;

            using (_context = ConnectionHelper.CreateConnection())
            {
                Organizations = _context.Organizations.ToList();
            }
            return Organizations;
        }

        public Organization UpdateOrganization(Organization Organization)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.Organizations.Local.Any(r => r.OrganizationID == Organization.OrganizationID))
                {
                    _context.Organizations.Attach(Organization);
                }
                _context.Entry(Organization).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Organization;
        }
    }
}
