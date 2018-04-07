using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class RelOrganizationsRepository : IRelOrganizationsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public RelOrganization AddRelOrganization(RelOrganization RelOrganization)
        {
            //_context = ConnectionHelper.CreateConnection();
            _context.RelOrganizations.Add(RelOrganization);
            _context.SaveChanges();
            return RelOrganization;
        }

        public void DeleteRelOrganization(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            var RelOrganization = _context.RelOrganizations.FirstOrDefault(r => r.RelOrganizationID == id);
            if (RelOrganization != null)
            {
                _context.RelOrganizations.Remove(RelOrganization);
            }
            _context.SaveChanges();
        }

        public RelOrganization GetRelOrganization(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.RelOrganizations.FirstOrDefault(r => r.RelOrganizationID == id);
        }

        public List<RelOrganization> GetRelOrganizations()
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.RelOrganizations.ToList();
        }

        public RelOrganization UpdateRelOrganization(RelOrganization RelOrganization)
        {
            //_context = ConnectionHelper.CreateConnection();
            if (!_context.RelOrganizations.Local.Any(r => r.RelOrganizationID == RelOrganization.RelOrganizationID))
            {
                _context.RelOrganizations.Attach(RelOrganization);
            }
            _context.Entry(RelOrganization).State = EntityState.Modified;
            _context.SaveChanges();
            return RelOrganization;
        }
    }
}
