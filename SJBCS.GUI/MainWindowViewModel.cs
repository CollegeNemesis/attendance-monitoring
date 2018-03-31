using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;
using AMS.Utilities;
using MaterialDesignThemes.Wpf;
using SJBCS.Data;
using SJBCS.GUI.AMS;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Home;
using SJBCS.GUI.Report;
using SJBCS.GUI.Settings;
using SJBCS.GUI.SMS;
using SJBCS.GUI.Student;
using SJBCS.Services.Repository;
using Unity;

namespace SJBCS.GUI
{
    class MainWindowViewModel : BindableBase
    {

        private IStudentsRepository _studentsRepository;
        public LoginViewModel _loginViewModel;
        public MenuViewModel _menuViewModel;
        public GroupViewModel _groupViewModel;
        public SmsViewModel _smsViewModel;
        public SectionViewModel _sectionViewModel;
        public ReportViewModel _reportViewModel;
        public AttendanceViewModel _attendanceViewModel;
        public AddEditStudentViewModel _addEditStudentViewModel;
        public StudentViewModel _studentViewModel;
        ConfigurationWindow ConfigurationWindow;

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private object _menu;
        public object Menu
        {
            get { return _menu; }
            set { SetProperty(ref _menu, value); }
        }

        public ClockViewModel _clockViewModel;
        public ClockViewModel ClockViewModel
        {
            get { return _clockViewModel; }
            set { SetProperty(ref _clockViewModel, value); }

        }

        public MainWindowViewModel(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
            LoadConfiguration();

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
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _reportViewModel;
        }

        private void NavToGroup()
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _groupViewModel;
        }

        private void NavToSection()
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _sectionViewModel;
        }

        private void NavToStudent()
        {
            _studentViewModel.LoadStudents();

            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _studentViewModel;
        }

        private void NavToSms()
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _smsViewModel;
        }

        private void NavToAttendance()
        {
            _attendanceViewModel.SwitchOn();
            ClockViewModel = null;
            CurrentViewModel = _attendanceViewModel;
        }

        private void NavToAddStudent(Data.Student selectedStudent)
        {
            _attendanceViewModel.SwitchOff();
            _addEditStudentViewModel.EditMode = false;
            _addEditStudentViewModel.SetStudent(selectedStudent);
            _addEditStudentViewModel.Initialize();

            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _addEditStudentViewModel;
        }

        private void NavToEditStudent(Data.Student selectedStudent)
        {
            _attendanceViewModel.SwitchOff();
            _addEditStudentViewModel.EditMode = true;
            _addEditStudentViewModel.SetStudent(selectedStudent);
            _addEditStudentViewModel.Initialize();

            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _addEditStudentViewModel;
        }

        private void NavToMenu(User user)
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            if (user.Type.ToLower().Equals("admin"))
            {
                _studentViewModel.LoadStudents();

                if (ClockViewModel == null)
                    ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

                Menu = _menuViewModel;
                CurrentViewModel = _studentViewModel;
            }
            else if (user.Type.ToLower().Equals("user"))
            {
                _attendanceViewModel.SwitchOn();
                ClockViewModel = null;
                CurrentViewModel = _attendanceViewModel;
                Menu = null;
            }
            else
            {
                CurrentViewModel = null;
            }

        }

        private void LoadConfiguration()
        {
            try
            {
                _studentsRepository.GetStudents();
            }
            catch (Exception error)
            {
                ConfigurationWindow = new ConfigurationWindow();
                ConfigurationWindow.ShowDialog();
            }
        }
    }
}
