using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Services.Repository
{
    public class DistributionListsRepository : IDistributionListsRepository
    {
        AmsModel _context = ConnectionHelper.CreateConnection();

        public DistributionList AddDistributionList(DistributionList DistributionList)
        {
            _context.DistributionLists.Add(DistributionList);
            _context.SaveChanges();
            return DistributionList;
        }

        public void DeleteDistributionList(Guid id)
        {
            var DistributionList = _context.DistributionLists.FirstOrDefault(r => r.DistributionListID == id);
            if (DistributionList != null)
            {
                _context.DistributionLists.Remove(DistributionList);
            }
            _context.SaveChanges();
        }

        public DistributionList GetDistributionList(Guid id)
        {
            return _context.DistributionLists.FirstOrDefault(r => r.DistributionListID == id);
        }

        public List<DistributionList> GetDistributionLists()
        {
            return _context.DistributionLists.ToList();
        }

        public DistributionList UpdateDistributionList(DistributionList DistributionList)
        {
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
