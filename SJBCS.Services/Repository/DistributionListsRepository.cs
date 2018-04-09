using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class DistributionListsRepository : IDistributionListsRepository
    {
        AmsModel _context;

        public DistributionList AddDistributionList(DistributionList DistributionList)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                _context.DistributionLists.Add(DistributionList);
                _context.SaveChanges();
            }
            return DistributionList;
        }

        public void DeleteDistributionList(Guid id)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                var DistributionList = _context.DistributionLists.FirstOrDefault(r => r.DistributionListID == id);
                _context.Entry(DistributionList).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public DistributionList GetDistributionList(Guid id)
        {
            DistributionList DistributionList;

            using (_context = ConnectionHelper.CreateConnection())
            {
                DistributionList = _context.DistributionLists.FirstOrDefault(r => r.DistributionListID == id);
            }
            return DistributionList;
        }

        public List<DistributionList> GetDistributionLists()
        {
            List<DistributionList> DistributionLists;

            using (_context = ConnectionHelper.CreateConnection())
            {
                DistributionLists = _context.DistributionLists.ToList();
            }
            return DistributionLists;
        }

        public DistributionList UpdateDistributionList(DistributionList DistributionList)
        {
            using (_context = ConnectionHelper.CreateConnection())
            {
                if (!_context.DistributionLists.Local.Any(r => r.DistributionListID == DistributionList.DistributionListID))
                {
                    _context.DistributionLists.Attach(DistributionList);
                }
                _context.Entry(DistributionList).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return DistributionList;
        }
    }
}
