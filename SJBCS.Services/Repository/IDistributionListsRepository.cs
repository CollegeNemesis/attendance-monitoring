using SJBCS.Data;
using System;
using System.Collections.Generic;

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
