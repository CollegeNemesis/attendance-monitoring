using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SJBCS.Model;
using SJBCS.Wrapper;

namespace SJBCS.ViewModel
{
    class ContactInfoViewModel : INotifyPropertyChanged
    {
        private AMSEntities DBContext;
        private ContactWrapper _contactWrapper;
        private ListStudent_Result _selectedStudent;
        private ObservableCollection<Object> _contactList;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Object> ContactList => _contactList;

        public ContactInfoViewModel(AMSEntities dBContext, ListStudent_Result selectedStudent)
        {
            DBContext = dBContext;
            _contactWrapper = new ContactWrapper();
            _selectedStudent = selectedStudent;
            _contactList = _contactWrapper.RetrieveViaKeyword(DBContext, _selectedStudent, _selectedStudent.StudentID);
        }


        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}
