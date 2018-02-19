using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SJBCS.Services
{
    interface ISectionsRepository
    {
        Task<List<Section>> GetSectionsAsync();
        Task<Section> GetSectionAsync(Guid id);
        Task<Section> AddSectionAsync(Section section);
        Task<Section> UpdateSectionAsync(Section section);
        Task DeleteSectionAsync(Guid sectionID);
    }
}
