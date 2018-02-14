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

        public void Add(string studentID, string contact)
        {
            _contact = new Contact();
            _contact.ContactID = Guid.NewGuid();
            _contact.ContactNumber = contact;
            _contact.StudentID = studentID;
            DBContext.Contacts.Add(_contact);
            DBContext.SaveChanges();
        }

        public override ObservableCollection<object> RetrieveViaKey(object obj)
        {
            if (obj is ListStudent_Result)
            {
                ListStudent_Result student = (ListStudent_Result)obj;
                var query = from contact in DBContext.Contacts
                            where contact.StudentID == student.StudentID
                            select contact;

                return new ObservableCollection<Object>(query.ToList());
            }
            return null;
        }
    }
}
