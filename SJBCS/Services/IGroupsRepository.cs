using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SJBCS.Services
{
    interface IGroupsRepository
    {
        Task<List<Organization>> GetOrganizationsAsync();
        Task<Organization> GetOrganizationAsync(Guid id);
        Task<Organization> AddOrganizationAsync(Organization organization);
        Task<Organization> UpdateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(Guid organizationId);
    }
}
