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
    class AddStudentViewModel : INotifyPropertyChanged
    {
        #region Model Properties
        private AMSEntities DBContext;
        private Student _student;
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

        private ObservableCollection<String> _contactList;
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
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                _student.LevelID = _level.LevelID;
                Console.WriteLine(_student.LevelID);
                _sectionList = _sectionWrapper.RetrieveViaKeyword(DBContext, _level, _level.LevelID.ToString());
                _section = (Section)_sectionList.FirstOrDefault();
                Console.WriteLine(_student.SectionID);
                RaisePropertyChanged(null);
            }
        }
        public Section Section
        {
            get
            {
                return _section;
            }
            set
            {
                _section = value;
                _student.SectionID = _section.SectionID;
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
        public ObservableCollection<String> ContactList => _contactList;
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

        public AddStudentViewModel(AMSEntities dbContext)
        {
            DBContext = dbContext;
            _student = new Student();
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

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        private void Default()
        {
            _student = new Student();
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
            _contactList = null;

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
        private void AddStudent(Object obj)
        {
            _studentWrapper.Add(DBContext, _student);
            foreach(string contact in _contactList)
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
            Console.WriteLine("Adding");
            if (_contactList == null)
            {
                _contactList = new ObservableCollection<string>();
                _contactList.Add("+63" + Contact);
            }
            else
            {
                foreach (String contact in _contactList)
                {
                    if (!contact.Equals(_contact))
                    {
                        _contactList.Add("+63" + Contact);
                        break;
                    }

                }
            }
            _contact = "";
            RaisePropertyChanged(null);
        }
        private void OpenFileDialog(Object obj)
        {
            
        }


    }
}
