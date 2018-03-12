using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Services.Repository
{
    public interface IRelDistributionListsRepository
    {
        List<RelDistributionList> GetRelDistributionLists();
        RelDistributionList GetRelDistributionList(Guid id);
        RelDistributionList AddRelDistributionList(RelDistributionList RelDistributionList);
        RelDistributionList UpdateRelDistributionList(RelDistributionList RelDistributionList);
        void DeleteRelDistributionList(Guid id);
    }
}
