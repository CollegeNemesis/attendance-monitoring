using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class OrganizationsRepository : IOrganizationsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public Organization AddOrganization(Organization Organization)
        {
            //_context = ConnectionHelper.CreateConnection();
            _context.Organizations.Add(Organization);
            _context.SaveChanges();
            return Organization;
        }

        public void DeleteOrganization(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            var Organization = _context.Organizations.FirstOrDefault(r => r.OrganizationID == id);
            if (Organization != null)
            {
                _context.Organizations.Remove(Organization);
            }
            _context.SaveChanges();
        }

        public Organization GetOrganization(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Organizations.FirstOrDefault(r => r.OrganizationID == id);
        }

        public List<Organization> GetOrganizations()
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.Organizations.ToList();
        }

        public Organization UpdateOrganization(Organization Organization)
        {
            //_context = ConnectionHelper.CreateConnection();
            if (!_context.Organizations.Local.Any(r => r.OrganizationID == Organization.OrganizationID))
            {
                _context.Organizations.Attach(Organization);
            }
            _context.Entry(Organization).State = EntityState.Modified;
            _context.SaveChanges();
            return Organization;
        }
    }
}
