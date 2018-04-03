using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Services.Repository
{
    public interface ISectionsRepository
    {
        List<Section> GetSections(Guid id);
        List<Section> GetSections();
        Section GetSection(Guid id);
        Section AddSection(Section Section);
        Section UpdateSection(Section Section);
        void DeleteSection(Guid id);
    }
}
