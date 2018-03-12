using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Services.Repository
{
    public interface IDistributionListsRepository
    {
        List<DistributionList> GetDistributionLists();
        DistributionList GetDistributionList(Guid id);
        DistributionList AddDistributionList(DistributionList DistributionList);
        DistributionList UpdateDistributionList(DistributionList DistributionList);
        void DeleteDistributionList(Guid id);
    }
}
