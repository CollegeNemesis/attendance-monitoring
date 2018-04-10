using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class RelOrganizationsRepository : IRelOrganizationsRepository
    {
        AmsModel _context;

        public RelOrganization AddRelOrganization(RelOrganization RelOrganization)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.RelOrganizations.Add(RelOrganization);
                _context.SaveChanges();
            }
            return RelOrganization;
        }

        public void DeleteRelOrganization(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var RelOrganization = _context.RelOrganizations.FirstOrDefault(r => r.RelOrganizationID == id);
                _context.Entry(RelOrganization).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public RelOrganization GetRelOrganization(Guid id)
        {
            RelOrganization RelOrganization;

            using (_context = ConnectionHelper.CreateConnection())
            {
                RelOrganization = _context.RelOrganizations.FirstOrDefault(r => r.RelOrganizationID == id);
            }
            return RelOrganization;
        }

        public List<RelOrganization> GetRelOrganizations()
        {
            List<RelOrganization> RelOrganizations;

            using (_context = ConnectionHelper.CreateConnection())
            {
                RelOrganizations = _context.RelOrganizations.ToList();
            }
            return RelOrganizations;
        }

        public RelOrganization UpdateRelOrganization(RelOrganization RelOrganization)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.RelOrganizations.Local.Any(r => r.RelOrganizationID == RelOrganization.RelOrganizationID))
                {
                    _context.RelOrganizations.Attach(RelOrganization);
                }
                _context.Entry(RelOrganization).State = EntityState.Modified;
                _context.SaveChanges();
            }                
            return RelOrganization;
        }
    }
}
