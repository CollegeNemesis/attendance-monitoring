using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class RelDistributionListsRepository : IRelDistributionListsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public RelDistributionList AddRelDistributionList(RelDistributionList RelDistributionList)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.RelDistributionLists.Add(RelDistributionList);
                _context.SaveChanges();
            }
            return RelDistributionList;
        }

        public void DeleteRelDistributionList(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var RelDistributionList = _context.RelDistributionLists.FirstOrDefault(r => r.RelDistributionListID == id);
                _context.Entry(RelDistributionList).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public RelDistributionList GetRelDistributionList(Guid id)
        {
            RelDistributionList RelDistributionList;

            using (_context = ConnectionHelper.CreateConnection())
            {
                RelDistributionList = _context.RelDistributionLists.FirstOrDefault(r => r.RelDistributionListID == id);
            }
            return RelDistributionList;
        }

        public List<RelDistributionList> GetRelDistributionLists()
        {
            List<RelDistributionList> RelDistributionLists;

            using (_context = ConnectionHelper.CreateConnection())
            {
                RelDistributionLists = _context.RelDistributionLists.ToList();
            }
            return RelDistributionLists;
        }

        public RelDistributionList UpdateRelDistributionList(RelDistributionList RelDistributionList)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.RelDistributionLists.Local.Any(r => r.RelDistributionListID == RelDistributionList.RelDistributionListID))
                {
                    _context.RelDistributionLists.Attach(RelDistributionList);
                }
                _context.Entry(RelDistributionList).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RelDistributionList;
        }
    }
}
