using MaterialDesignThemes.Wpf;
using SJBCS.Model;
using SJBCS.View;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SJBCS.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged
    {

        #region Model Properties
        private static AMSEntities DBContext = new AMSEntities();
        private MenuItem[] _fingerPrintList;

        private Student _student;
        private Object _content;
        private ListStudent_Result _selectedStudent;
        private ContactWrapper _contactWrapper;
        private ObservableCollection<Object> _contactList;
        private ObservableCollection<Object> _groupList;
        private OrganizationWrapper _groupWrapper;
        private int _studentInfoPanelWidth;
        private ObservableCollection<Object> _studentList;

        private StudentWrapper _studentWrapper;
        #endregion

        #region View Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Object> ContactList => _contactList;
        public ObservableCollection<Object> GroupList => _groupList;

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
        
        public Object Student
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                _selectedStudent = (ListStudent_Result)value;
                _selectedStudent.GradeLevel = _selectedStudent.GradeLevel.Trim();
                _studentInfoPanelWidth = 400;
                _contactList = _contactWrapper.RetrieveViaKeyword(DBContext, _selectedStudent, _selectedStudent.StudentID);
                _groupList = _groupWrapper.RetrieveViaKeyword(DBContext, _selectedStudent, _selectedStudent.StudentID);
                

                if (String.IsNullOrEmpty(_selectedStudent.ImageData))
                {
                    _selectedStudent.ImageData = "/SJBCS;component/Resources/Images/default-user-image.png";
                }
                RaisePropertyChanged(null);
            }
        }
        public String ImageData
        {
            get
            {
                return _selectedStudent.ImageData;
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
        public String GradeLevel
        {
            get
            {
                return _selectedStudent.GradeLevel;
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
        public int StudentInfoPanelWidth
        {
            get
            {
                return _studentInfoPanelWidth;
            }
        }
        public object Content
        {
            get { return _content; }
            set { this.MutateVerbose(ref _content, value, RaisePropertyChanged()); }
        }

        public ICommand OpenAddStudentViewCommand => new CommandImplementation(OpenAddStudentView);
        public ICommand OpenUpdateStudentViewCommand => new CommandImplementation(OpenUpdateStudentView);
        public ICommand OpenEnrollBiometricsViewCommand => new CommandImplementation(OpenEnrollBiometricsViewAsync);

        #endregion

        public StudentViewModel()
        {
            Default();
        }
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new AddStudentView());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        private async void OpenAddStudentView(Object obj)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddStudentView
            {
                DataContext = new AddStudentViewModel(DBContext)
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            Default();
            RaisePropertyChanged(null);
        }
        private async void OpenUpdateStudentView(Object obj)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UpdateStudentView
            {
                DataContext = new UpdateStudentViewModel(DBContext, _selectedStudent)
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            Default();
            RaisePropertyChanged(null);
        }

        private async void OpenEnrollBiometricsViewAsync(Object obj)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UpdateStudentView
            {
                DataContext = new UpdateStudentViewModel(DBContext, _selectedStudent)
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            Default();
            RaisePropertyChanged(null);
        }

        private void Default()
        {
            _student = new Student();
            _selectedStudent = new ListStudent_Result();
            _studentWrapper = new StudentWrapper();
            _studentList = _studentWrapper.RetrieveAll(DBContext, _student);
            _contactWrapper = new ContactWrapper();
            _contactList = _contactWrapper.RetrieveViaKeyword(DBContext, _selectedStudent, _selectedStudent.StudentID);
            _groupWrapper = new OrganizationWrapper();
            _groupList = _groupWrapper.RetrieveViaKeyword(DBContext, _selectedStudent, _selectedStudent.StudentID);

            //Default Value
            _selectedStudent = (ListStudent_Result)_studentList.FirstOrDefault();
            _studentInfoPanelWidth = 400;
        }

    }
}
