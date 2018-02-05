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
    class StudentInfoViewModel : INotifyPropertyChanged
    {
        #region Model Properties
        private AMSEntities DBContext;
        private ListStudent_Result _selectedStudent;
        private ContactWrapper _contactWrapper;
        private ObservableCollection<Object> _contactList;
        private ObservableCollection<Object> _groupList;
        private OrganizationWrapper _groupWrapper;
        #endregion

        #region View Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Object> ContactList => _contactList;
        public ObservableCollection<Object> GroupList => _groupList;

        public String StudentID
        {
            get
            {
                return _selectedStudent.StudentID;
            }
            set
            {
                _selectedStudent.StudentID = value;
                RaisePropertyChanged(null);
            }
        }
        public String FirstName
        {
            get
            {
                return _selectedStudent.FirstName;
            }
            set
            {
                _selectedStudent.FirstName = value;
            }
        }
        public String MiddleName
        {
            get
            {
                return _selectedStudent.MiddleName;
            }
            set
            {
                _selectedStudent.MiddleName = value;
            }
        }
        public String LastName
        {
            get
            {
                return _selectedStudent.LastName;
            }
            set
            {
                _selectedStudent.LastName = value;
            }
        }
        public String GradeLevel
        {
            get
            {
                return _selectedStudent.GradeLevel.Trim();
            }
            set
            {
                _selectedStudent.GradeLevel = value;

            }
        }
        public String SectionName
        {
            get
            {
                return _selectedStudent.SectionName;
            }
            set
            {
                _selectedStudent.SectionName = value;
            }
        }
        public String BirthDate
        {
            get
            {
                return Convert.ToDateTime(_selectedStudent.BirthDate).ToString("MM/dd/yyyy");
            }
            set
            {
                _selectedStudent.BirthDate = Convert.ToDateTime(value);
            }
        }
        public String Street
        {
            get
            {
                return _selectedStudent.Street;
            }
            set
            {
                _selectedStudent.Street = value;
            }
        }
        public String City
        {
            get
            {
                return _selectedStudent.City;
            }
            set
            {
                _selectedStudent.City = value;
            }
        }
        public String State
        {
            get
            {
                return _selectedStudent.State;
            }
            set
            {
                _selectedStudent.State = value;
            }
        }
        public String Gender
        {
            get
            {
                return _selectedStudent.Gender;
            }
            set
            {
                _selectedStudent.Gender = value;
            }
        }
        #endregion

        public StudentInfoViewModel(AMSEntities dBContext,  ListStudent_Result selectedStudent)
        {
            DBContext = dBContext;
            _selectedStudent = selectedStudent;
            _contactWrapper = new ContactWrapper();
            _contactList = _contactWrapper.RetrieveViaKeyword(DBContext, _selectedStudent, _selectedStudent.StudentID);
            _groupWrapper = new OrganizationWrapper();
            _groupList = _groupWrapper.RetrieveViaKeyword(DBContext, _selectedStudent, _selectedStudent.StudentID);
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
