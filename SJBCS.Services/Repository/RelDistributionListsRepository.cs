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
            //_context = ConnectionHelper.CreateConnection();
            _context.RelDistributionLists.Add(RelDistributionList);
            _context.SaveChanges();
            return RelDistributionList;
        }

        public void DeleteRelDistributionList(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            var RelDistributionList = _context.RelDistributionLists.FirstOrDefault(r => r.RelDistributionListID == id);
            if (RelDistributionList != null)
            {
                _context.RelDistributionLists.Remove(RelDistributionList);
            }
            _context.SaveChanges();
        }

        public RelDistributionList GetRelDistributionList(Guid id)
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.RelDistributionLists.FirstOrDefault(r => r.RelDistributionListID == id);
        }

        public List<RelDistributionList> GetRelDistributionLists()
        {
            _context = ConnectionHelper.CreateConnection();
            return _context.RelDistributionLists.ToList();
        }

        public RelDistributionList UpdateRelDistributionList(RelDistributionList RelDistributionList)
        {
            //_context = ConnectionHelper.CreateConnection();
            if (!_context.RelDistributionLists.Local.Any(r => r.RelDistributionListID == RelDistributionList.RelDistributionListID))
            {
                _context.RelDistributionLists.Attach(RelDistributionList);
            }
            _context.Entry(RelDistributionList).State = EntityState.Modified;
            _context.SaveChanges();
            return RelDistributionList;
        }
    }
}
