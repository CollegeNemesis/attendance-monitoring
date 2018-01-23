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

        private Student _student;
        private Object _content;
        private ListStudent_Result _selectedStudent;
        private int _studentInfoPanelWidth;
        private ObservableCollection<Object> _studentList;

        private StudentWrapper _studentWrapper;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region View Properties
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
                _studentInfoPanelWidth = 400;
                _content = new StudentInfoView { DataContext = new StudentInfoViewModel(DBContext,_selectedStudent) };
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


        public ICommand OpenStudentInfoViewCommand => new CommandImplementation(OpenStudentInfoView);
        public ICommand OpenContactInfoViewCommand => new CommandImplementation(OpenContactInfoView);
        public ICommand OpenGroupInfoViewCommand => new CommandImplementation(OpenGroupInfoView);
        #endregion

        public StudentViewModel()
        {
            _student = new Student();
            _selectedStudent = new ListStudent_Result();
            _studentInfoPanelWidth = 0;
            _studentWrapper = new StudentWrapper();
            _studentList = _studentWrapper.RetrieveAll(DBContext, _student);
        }
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
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
        private void OpenStudentInfoView(Object obj)
        {
            _content = new StudentInfoView { DataContext = new StudentInfoViewModel(DBContext,_selectedStudent) };
            RaisePropertyChanged(null);
        }
        private void OpenContactInfoView(Object obj)
        {
            _content = new ContactInfoView { DataContext = new ContactInfoViewModel(DBContext, _selectedStudent) };
            RaisePropertyChanged(null);
        }
        private void OpenGroupInfoView(Object obj)
        {
            _content = new GroupInfoView { DataContext = new GroupInfoViewModel(DBContext, _selectedStudent) };
            RaisePropertyChanged(null);
        }

    }
}
