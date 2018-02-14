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
        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            Student student = (Student)obj;
            var query = from relOrg in DBContext.RelOrganizations
                        where relOrg.StudentID == student.StudentID
                        select relOrg.OrganizationID;

            List<Guid> tempList = query.ToList();
            List<Object> orgList = new List<Object>();

            foreach(var id in tempList)
            {
                var queryOrg = from org in DBContext.Organizations
                            where org.OrganizationID == id
                            select org;
                orgList.Add(queryOrg.ToList());
            }

            return new ObservableCollection<Object>(orgList);
        }
    }
}
