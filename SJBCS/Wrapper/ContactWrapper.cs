using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Wrapper
{
    class ContactWrapper : EntityModel
    {
        private Contact _contact;

        public void Add(AMSEntities dBContext, object obj)
        {
            throw new NotImplementedException();
        }

        public void Add(AMSEntities dBContext, string studentID, string contact)
        {
            _contact = new Contact();
            _contact.ContactID = Guid.NewGuid();
            _contact.ContactNumber = contact;
            _contact.StudentID = studentID;
            dBContext.Contacts.Add(_contact);
            dBContext.SaveChanges();
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
            var query = from contact in dBContext.Contacts
                        where contact.StudentID == keyword
                        select contact;

            return new ObservableCollection<Object>(query.ToList());
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
