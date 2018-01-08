using SJBCS.Model;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static AMSEntities DBContext = new AMSEntities();
        private ObservableCollection<Object> _levelList;
        private ObservableCollection<Object> _sectionList;
        private ObservableCollection<Object> _studentList;
        private Student _student;
        private StudentWrapper _studentWrapper;

        public StudentViewModel()
        {
            _student = new Student();
            _studentWrapper = new StudentWrapper();
            _studentList = _studentWrapper.RetrieveAll(DBContext,_student);
        }
        public ObservableCollection<Object> StudentList
        {
            get
            {
                return _studentList;
            }
            set
            {
                _studentList = value;
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

        public String StudentID
        {
            get
            {
                return _student.StudentID;
            }
            set
            {
                _student.StudentID = value;
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
            }
        }
        public String MiddleName
        {
            get
            {
                return _student.MiddleName;
            }
            set
            {
                _student.MiddleName = value;
            }
        }
        public String BirthDate
        {
            get
            {
                return _student.BirthDate.ToString();
            }
            set
            {
                _student.BirthDate = Convert.ToDateTime(value);
            }
        }
        public String ImageData
        {
            get
            {
                return _student.ImageData;
            }
            set
            {
                _student.ImageData = value;
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
        public Level GradeLevel
        {
            set { _student.LevelID = value.LevelID; }
        }

        public Section SectionName
        {
            set { _student.SectionID = value.SectionID; }
        }

    }
}
