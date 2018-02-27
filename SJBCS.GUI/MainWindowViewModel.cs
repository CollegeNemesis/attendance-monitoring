using System;
using AMS.Utilities;
using SJBCS.Data;
using SJBCS.GUI.AMS;
using SJBCS.GUI.Home;
using SJBCS.GUI.Report;
using SJBCS.GUI.SMS;
using SJBCS.GUI.Student;
using Unity;

namespace SJBCS.GUI
{
    class MainWindowViewModel : BindableBase
    {
        private object _currentViewModel;

        #region ViewModel
        public LoginViewModel _loginViewModel;
        public MenuViewModel _menuViewModel;
        public ClockViewModel _clockViewModel;

        public GroupViewModel _groupViewModel;
        public SmsViewModel _smsViewModel;
        public SectionViewModel _sectionViewModel;
        public ReportViewModel _reportViewModel;
        public AttendanceViewModel _attendanceViewModel;

        public StudentViewModel _studenViewModel;
        #endregion

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public ClockViewModel ClockViewModel
        {
            get { return _clockViewModel; }
            set { SetProperty(ref _clockViewModel, value); }

        }

        public MenuViewModel MenuViewModel
        {
            get { return _menuViewModel; }
            set { SetProperty(ref _menuViewModel, value); }

        }

        public MainWindowViewModel()
        {
            _loginViewModel = ContainerHelper.Container.Resolve<LoginViewModel>();
            _menuViewModel = ContainerHelper.Container.Resolve<MenuViewModel>();
            _clockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();
            _attendanceViewModel = ContainerHelper.Container.Resolve<AttendanceViewModel>();
            _sectionViewModel = ContainerHelper.Container.Resolve<SectionViewModel>();
            _groupViewModel = ContainerHelper.Container.Resolve<GroupViewModel>();
            _studenViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _reportViewModel = ContainerHelper.Container.Resolve<ReportViewModel>();
            _smsViewModel = ContainerHelper.Container.Resolve<SmsViewModel>();

            _currentViewModel = _attendanceViewModel;

            _loginViewModel.LoginRequested += NavToMenu;
            _menuViewModel.NavToAttendanceRequested += NavToAttendance;
            _menuViewModel.NavToSmsRequested += NavToSms;
            _menuViewModel.NavToStudentRequested += NavToStudent;
            _menuViewModel.NavToSectionRequested += NavToSection;
            _menuViewModel.NavToGroupRequested += NavToGroup;
            _menuViewModel.NavToReportRequested += NavToReport;
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
            CurrentViewModel = _studenViewModel;
        }

        private void NavToSms()
        {
            CurrentViewModel = _smsViewModel;
        }

        private void NavToAttendance()
        {
            CurrentViewModel = _attendanceViewModel;
        }

        private void NavToMenu(User user)
        {
            if (user.Type.ToLower().Equals("admin"))
            {
                //CurrentViewModel = _adminHomeViewModel;
            }
            else if (user.Type.ToLower().Equals("user"))
            {
                //CurrentViewModel = _userHomeViewModel;
            }
            else
            {
                CurrentViewModel = null;
            }

        }
    }
}
