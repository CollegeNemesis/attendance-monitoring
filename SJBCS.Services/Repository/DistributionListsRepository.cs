using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SJBCS.Services.Repository
{
    public class DistributionListsRepository : IDistributionListsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public DistributionList AddDistributionList(DistributionList DistributionList)
        {
            //_context = ConnectionHelper.CreateConnection();
            _context.DistributionLists.Add(DistributionList);
            _context.SaveChanges();
            return DistributionList;
        }

        public void DeleteDistributionList(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            var DistributionList = _context.DistributionLists.FirstOrDefault(r => r.DistributionListID == id);
            if (DistributionList != null)
            {
                _context.DistributionLists.Remove(DistributionList);
            }
            _context.SaveChanges();
        }

        public DistributionList GetDistributionList(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.DistributionLists.FirstOrDefault(r => r.DistributionListID == id);
        }

        public List<DistributionList> GetDistributionLists()
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.DistributionLists.ToList();
        }

        public DistributionList UpdateDistributionList(DistributionList DistributionList)
        {
            //_context = ConnectionHelper.CreateConnection();
            if (!_context.DistributionLists.Local.Any(r => r.DistributionListID == DistributionList.DistributionListID))
            {
                _context.DistributionLists.Attach(DistributionList);
            }
            _context.Entry(DistributionList).State = EntityState.Modified;
            _context.SaveChanges();
            return DistributionList;
        }
    }
}
