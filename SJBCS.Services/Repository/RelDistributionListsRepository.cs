using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Services.Repository
{
    public class RelDistributionListsRepository : IRelDistributionListsRepository
    {
        AmsDbContext _context = new AmsDbContext();

        public RelDistributionList AddRelDistributionList(RelDistributionList RelDistributionList)
        {
            _context.RelDistributionLists.Add(RelDistributionList);
            _context.SaveChanges();
            return RelDistributionList;
        }

        public void DeleteRelDistributionList(Guid id)
        {
            var RelDistributionList = _context.RelDistributionLists.FirstOrDefault(r => r.RelDistributionListID == id);
            if (RelDistributionList != null)
            {
                _context.RelDistributionLists.Remove(RelDistributionList);
            }
            _context.SaveChanges();
        }

        public RelDistributionList GetRelDistributionList(Guid id)
        {
            return _context.RelDistributionLists.FirstOrDefault(r => r.RelDistributionListID == id);
        }

        public List<RelDistributionList> GetRelDistributionLists()
        {
            return _context.RelDistributionLists.ToList();
        }

        public RelDistributionList UpdateRelDistributionList(RelDistributionList RelDistributionList)
        {
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
