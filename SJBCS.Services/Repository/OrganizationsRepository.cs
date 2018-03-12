﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Data;

namespace SJBCS.Services.Repository
{
    public class OrganizationsRepository : IOrganizationsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public Organization AddOrganization(Organization Organization)
        {
            _context.Organizations.Add(Organization);
            _context.SaveChanges();
            return Organization;
        }

        public void DeleteOrganization(Guid id)
        {
            var Organization = _context.Organizations.FirstOrDefault(r => r.OrganizationID == id);
            if (Organization != null)
            {
                _context.Organizations.Remove(Organization);
            }
            _context.SaveChanges();
        }

        public Organization GetOrganization(Guid id)
        {
            return _context.Organizations.FirstOrDefault(r => r.OrganizationID == id);
        }

        public List<Organization> GetOrganizations()
        {
            return _context.Organizations.ToList();
        }

        public Organization UpdateOrganization(Organization Organization)
        {
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