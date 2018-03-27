using System;
using System.Collections.ObjectModel;
using AMS.Utilities;
using SJBCS.Data;
using SJBCS.GUI.AMS;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Home;
using SJBCS.GUI.Report;
using SJBCS.GUI.SMS;
using SJBCS.GUI.Student;
using SJBCS.Services.Repository;
using Unity;

namespace SJBCS.GUI
{
    class MainWindowViewModel : BindableBase
    {
        private object _currentViewModel;
        private object _menu;
        private IStudentsRepository _studentsRepository;

        #region ViewModel
        public LoginViewModel _loginViewModel;
        public MenuViewModel _menuViewModel;
        public ClockViewModel _clockViewModel;

        public GroupViewModel _groupViewModel;
        public SmsViewModel _smsViewModel;
        public SectionViewModel _sectionViewModel;
        public ReportViewModel _reportViewModel;
        public AttendanceViewModel _attendanceViewModel;

        public AddEditStudentViewModel _addEditStudentViewModel;
        public StudentViewModel _studentViewModel;

        #endregion

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public object Menu
        {
            get { return _menu; }
            set { SetProperty(ref _menu, value); }
        }

        public ClockViewModel ClockViewModel
        {
            get { return _clockViewModel; }
            set { SetProperty(ref _clockViewModel, value); }

        }

        public MainWindowViewModel(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
            _loginViewModel = ContainerHelper.Container.Resolve<LoginViewModel>();
            _menuViewModel = ContainerHelper.Container.Resolve<MenuViewModel>();
            _clockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();
            _attendanceViewModel = ContainerHelper.Container.Resolve<AttendanceViewModel>();
            _sectionViewModel = ContainerHelper.Container.Resolve<SectionViewModel>();
            _groupViewModel = ContainerHelper.Container.Resolve<GroupViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _reportViewModel = ContainerHelper.Container.Resolve<ReportViewModel>();
            _smsViewModel = ContainerHelper.Container.Resolve<SmsViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();

            _currentViewModel = _loginViewModel;
            _menu = null;

            _addEditStudentViewModel.EditMode = false;

            _loginViewModel.LoginRequested += NavToMenu;
            _menuViewModel.NavToAttendanceRequested += NavToAttendance;
            _menuViewModel.NavToSmsRequested += NavToSms;
            _menuViewModel.NavToStudentRequested += NavToStudent;
            _menuViewModel.NavToSectionRequested += NavToSection;
            _menuViewModel.NavToGroupRequested += NavToGroup;
            _menuViewModel.NavToReportRequested += NavToReport;

            _studentViewModel.AddStudentRequested += NavToAddStudent;
            _studentViewModel.EditStudentRequested += NavToEditStudent;

            _addEditStudentViewModel.Done += NavToStudent;
        }

        private void NavToReport()
        {
            CurrentViewModel = _reportViewModel;
        }

        private void NavToGroup()
        {
            CurrentViewModel = _groupViewModel;
        }

        private void NavToSection()
        {
            CurrentViewModel = _sectionViewModel;
        }

        private void NavToStudent()
        {
            _studentViewModel.LoadStudents();
            CurrentViewModel = _studentViewModel;
        }

        private void NavToSms()
        {
            CurrentViewModel = _smsViewModel;
        }

        private void NavToAttendance()
        {
            _attendanceViewModel.SwitchOn();
            CurrentViewModel = _attendanceViewModel;
        }

        private void NavToAddStudent(Data.Student selectedStudent)
        {
            _attendanceViewModel.SwitchOff();
            _addEditStudentViewModel.EditMode = false;
            _addEditStudentViewModel.SetStudent(selectedStudent);
            _addEditStudentViewModel.Initialize();
            CurrentViewModel = _addEditStudentViewModel;
        }

        private void NavToEditStudent(Data.Student selectedStudent)
        {
            _attendanceViewModel.SwitchOff();
            _addEditStudentViewModel.EditMode = true;
            _addEditStudentViewModel.SetStudent(selectedStudent);
            _addEditStudentViewModel.Initialize();
            CurrentViewModel = _addEditStudentViewModel;
        }
        private void NavToMenu(User user)
        {
            if (user.Type.ToLower().Equals("admin"))
            {
                Menu = _menuViewModel;
                CurrentViewModel = _studentViewModel;
            }
            else if (user.Type.ToLower().Equals("user"))
            {
                CurrentViewModel = _attendanceViewModel;
                Menu = null;
            }
            else
            {
                CurrentViewModel = null;
            }

        }
    }
}
