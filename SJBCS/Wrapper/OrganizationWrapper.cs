using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Wrapper
{
    class OrganizationWrapper : EntityModel
    {
        public void Add(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveAll(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<object> RetrieveViaKeyword(AMSEntities dBContext, object obj, string keyword)
        {
            var query = from relOrg in dBContext.RelOrganizations
                        where relOrg.StudentID == keyword
                        select relOrg.OrganizationID;

            List<Guid> tempList = query.ToList();
            List<Object> orgList = new List<Object>();

            foreach(var id in tempList)
            {
                var queryOrg = from org in dBContext.Organizations
                            where org.OrganizationID == id
                            select org;
                orgList.Add(queryOrg.ToList());
            }

            return new ObservableCollection<Object>(orgList);
        }

        public ObservableCollection<object> RetrieveViaSP(AMSEntities dBContext, object obj, string sp, List<string> param)
        {
            throw new NotImplementedException();
        }

        public void Update(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
