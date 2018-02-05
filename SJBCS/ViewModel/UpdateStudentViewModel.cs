using SJBCS.Model;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SJBCS.ViewModel
{
    class UpdateStudentViewModel : INotifyPropertyChanged
    {
        #region Model Properties
        private AMSEntities DBContext;
        private ListStudent_Result _student;
        private String _contact;
        private String _selectedContact;
        private Section _section;
        private Level _level;
        private Organization _group;
        private StudentWrapper _studentWrapper;
        private ContactWrapper _contactWrapper;
        private SectionWrapper _sectionWrapper;
        private LevelWrapper _levelWrapper;
        private OrganizationWrapper _organizationWrapper;

        private ObservableCollection<Object> _contactList;
        private ObservableCollection<Object> _groupList;
        private ObservableCollection<Object> _levelList;
        private ObservableCollection<Object> _sectionList;


        #endregion

        #region View Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddStudentCommand => new CommandImplementation(AddStudent);
        public ICommand AddNumberCommand => new CommandImplementation(AddNumber);
        public ICommand DeleteNumberCommand => new CommandImplementation(DeleteNumber);
        public ICommand OpenFileDialogCommand => new CommandImplementation(OpenFileDialog);
        public String StudentID
        {
            get
            {
                return _student.StudentID;
            }
            set
            {
                _student.StudentID = value;
                RaisePropertyChanged(null);
            }
        }
        public String FirstName
        {
            get
            {
                return _student.FirstName;
            }
            set
            {
                _student.FirstName = value;
                RaisePropertyChanged(null);
            }
        }
        public String LastName
        {
            get
            {
                return _student.LastName;
            }
            set
            {
                _student.LastName = value;
                RaisePropertyChanged(null);
            }
        }
        public String BirthDate
        {
            get
            {
                return Convert.ToDateTime(_student.BirthDate).ToString("MM/dd/yyyy");
            }
            set
            {
                _student.BirthDate = Convert.ToDateTime(value);
            }
        }
        public String Street
        {
            get
            {
                return _student.Street;
            }
            set
            {
                _student.Street = value;
            }
        }
        public String City
        {
            get
            {
                return _student.City;
            }
            set
            {
                _student.City = value;
            }
        }
        public String State
        {
            get
            {
                return _student.State;
            }
            set
            {
                _student.State = value;
            }
        }
        public String Gender
        {
            get
            {
                return _student.Gender;
            }
            set
            {
                _student.Gender = value;
            }
        }
        public String ImageData
        {
            get
            {
                return _student.ImageData;
            }
        }
        public Level Level
        {
            set
            {
                if (_level != value)
                {
                    _level = value;
                    _student.LevelID = _level.LevelID;
                    _sectionList = _sectionWrapper.RetrieveViaKeyword(DBContext, _level, _level.LevelID.ToString());
                    _section = (Section)_sectionList.FirstOrDefault();
                    RaisePropertyChanged(null);
                }
            }
        }
        public Section Section
        {
            set
            {
                if (_section != value)
                {
                    _section = value;
                    RaisePropertyChanged(null);
                }
            }
        }
        public String GradeLevel
        {
            get
            {
                return _student.GradeLevel;
            }
            set
            {
                _student.GradeLevel = value;
            }
        }
        public String SectionName
        {
            get
            {
                return _student.SectionName;
            }
            set
            {
                _student.SectionName = value;
            }
        }
        public String SelectedContact
        {
            get
            {
                return _selectedContact;
            }
            set
            {
                _selectedContact = value;
            }
        }
        public ObservableCollection<Object> ContactList => _contactList;
        public ObservableCollection<Object> GroupList => _groupList;
        public ObservableCollection<Object> LevelList
        {
            get
            {
                return _levelList;
            }
            set
            {
                _levelList = value;
            }
        }
        public ObservableCollection<Object> SectionList
        {
            get
            {
                return _sectionList;
            }
            set
            {
                _sectionList = value;
            }
        }
        public String Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
            }
        }
        #endregion

        public UpdateStudentViewModel(AMSEntities dbContext, ListStudent_Result student)
        {
            DBContext = dbContext;
            _student = student;
            _level = new Level();
            _section = new Section();
            _studentWrapper = new StudentWrapper();
            _contactWrapper = new ContactWrapper();
            _sectionWrapper = new SectionWrapper();
            _levelWrapper = new LevelWrapper();
            _organizationWrapper = new OrganizationWrapper();
            _contactList = _contactWrapper.RetrieveViaKeyword(DBContext, _student, _student.StudentID);
            _levelList = _levelWrapper.RetrieveAll(DBContext, _level);
            _sectionList = _sectionWrapper.RetrieveViaKeyword(DBContext, _level, _level.LevelID.ToString());

            _level.LevelID = _student.LevelID;
            _level.GradeLevel = _student.GradeLevel;
            _section.LevelID = _student.LevelID;
            _section.SectionID = _student.SectionID;
            _section.SectionName = _student.SectionName;

            RaisePropertyChanged(null);
            Console.WriteLine(_level.GradeLevel);
            Console.WriteLine(_section.SectionName);
        }

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        private void AddStudent(Object obj)
        {
            _studentWrapper.Add(DBContext, _student);
            foreach (string contact in _contactList)
            {
                _contactWrapper.Add(DBContext, _student.StudentID, contact);
            }
            Default();
            RaisePropertyChanged(null);
        }
        private void DeleteNumber(Object obj)
        {
            _contactList.Remove(_selectedContact);
            Console.WriteLine("Deleting");
            if (_contactList.FirstOrDefault() == null)
            {
                Console.WriteLine("No item.");
                _contactList = null;
            }
            RaisePropertyChanged(null);
        }
        private void AddNumber(Object obj)
        {
            
        }
        private void OpenFileDialog(Object obj)
        {

        }

        private void Default()
        {
            _level = new Level();
            _studentWrapper = new StudentWrapper();
            _contactWrapper = new ContactWrapper();
            _sectionWrapper = new SectionWrapper();
            _levelWrapper = new LevelWrapper();
            _organizationWrapper = new OrganizationWrapper();
            _levelList = _levelWrapper.RetrieveAll(DBContext, _level);
            _level = (Level)_levelList.FirstOrDefault();
            _sectionList = _sectionWrapper.RetrieveViaKeyword(DBContext, _level, _level.LevelID.ToString());
            _section = (Section)_sectionList.FirstOrDefault();

            //Setting Default Value for Students
            _student.SectionID = _section.SectionID;
            _student.LevelID = _level.LevelID;
            _student.MiddleName = null;
            _student.Street = null;
            _student.City = null;
            _student.State = null;
            _student.Gender = "Male";
            _student.BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _student.ImageData = "/SJBCS;component/Resources/Images/default-user-image.png";
        }

    }
}
