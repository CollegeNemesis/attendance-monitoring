using SJBCS.Data;
using System;
using System.Collections.Generic;

namespace SJBCS.Services.Repository
{
    public interface IOrganizationsRepository
    {
        List<Organization> GetOrganizations();
        Organization GetOrganization(Guid id);
        Organization AddOrganization(Organization Organization);
        Organization UpdateOrganization(Organization Organization);
        void DeleteOrganization(Guid id);
    }
}
