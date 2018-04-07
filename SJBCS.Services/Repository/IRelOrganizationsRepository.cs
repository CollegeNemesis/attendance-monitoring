using SJBCS.Data;
using System;
using System.Collections.Generic;

namespace SJBCS.Services.Repository
{
    public interface IRelOrganizationsRepository
    {
        List<RelOrganization> GetRelOrganizations();
        RelOrganization GetRelOrganization(Guid id);
        RelOrganization AddRelOrganization(RelOrganization RelOrganization);
        RelOrganization UpdateRelOrganization(RelOrganization RelOrganization);
        void DeleteRelOrganization(Guid id);
    }
}
