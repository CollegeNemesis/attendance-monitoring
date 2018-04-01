using AMS.Utilities;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using SJBCS.Data;
using SJBCS.GUI.AMS;
using SJBCS.GUI.Home;
using SJBCS.GUI.Report;
using SJBCS.GUI.Settings;
using SJBCS.GUI.SMS;
using SJBCS.GUI.Student;
using SJBCS.Services.Repository;
using System;
using System.Configuration;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace SJBCS.GUI
{
    class MainWindowViewModel : BindableBase
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ResourceDictionary DialogDictionary = new ResourceDictionary() { Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml") };

        public LoginViewModel _loginViewModel;
        public MenuViewModel _menuViewModel;
        public GroupViewModel _groupViewModel;
        public SmsManagementViewModel _smsViewModel;
        public SectionViewModel _sectionViewModel;
        public ReportViewModel _reportViewModel;
        public AttendanceViewModel _attendanceViewModel;
        public AddEditStudentViewModel _addEditStudentViewModel;
        public StudentViewModel _studentViewModel;
        public UserManagementViewModel _userManagementViewModel;
        public DbManagementViewModel _dbManagementViewModel;
        public SmsManagementViewModel _smsManagementViewModel;
        public SettingsViewModel _settingsViewModel;
        public StartupConfigWindow StartupConfigWindow;
        public StartupConfigWindowViewModel _startupConfigWindowViewModel;

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

        public MainWindowViewModel()
        {
            LoadConfiguration();
            _loginViewModel = ContainerHelper.Container.Resolve<LoginViewModel>();
            _menuViewModel = ContainerHelper.Container.Resolve<MenuViewModel>();
            _clockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();
            _attendanceViewModel = ContainerHelper.Container.Resolve<AttendanceViewModel>();
            _sectionViewModel = ContainerHelper.Container.Resolve<SectionViewModel>();
            _groupViewModel = ContainerHelper.Container.Resolve<GroupViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _reportViewModel = ContainerHelper.Container.Resolve<ReportViewModel>();
            _smsViewModel = ContainerHelper.Container.Resolve<SmsManagementViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();
            _settingsViewModel = ContainerHelper.Container.Resolve<SettingsViewModel>();
            _dbManagementViewModel = ContainerHelper.Container.Resolve<DbManagementViewModel>();
            _userManagementViewModel = ContainerHelper.Container.Resolve<UserManagementViewModel>();
            _smsManagementViewModel = ContainerHelper.Container.Resolve<SmsManagementViewModel>();
            _startupConfigWindowViewModel = ContainerHelper.Container.Resolve<StartupConfigWindowViewModel>();

            //_currentViewModel = _loginViewModel;
            //_menu = null;

            _currentViewModel = _studentViewModel;
            _menu = _menuViewModel;

            _addEditStudentViewModel.Done += NavToStudent;
            _addEditStudentViewModel.EditMode = false;
            _loginViewModel.LoginRequested += NavToMenu;
            _menuViewModel.NavToAttendanceRequested += NavToAttendance;
            _menuViewModel.NavToSettingsRequested += NavToSettings;
            _menuViewModel.NavToStudentRequested += NavToStudent;
            _menuViewModel.NavToSectionRequested += NavToSection;
            _menuViewModel.NavToGroupRequested += NavToGroup;
            _menuViewModel.NavToReportRequested += NavToReport;
            _studentViewModel.AddStudentRequested += NavToAddStudent;
            _studentViewModel.EditStudentRequested += NavToEditStudent;
            _settingsViewModel.NavToDbManagementRequested += NavToDbManagement;
            _settingsViewModel.NavToUserManagementRequested += NavToUserManagement;
            _settingsViewModel.NavToSmsManagementRequested += NavToSmsManagement;
        }

        private void NavToSmsManagement()
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            LoadConfiguration();
            CurrentViewModel = ContainerHelper.Container.Resolve<SmsManagementViewModel>();
        }

        private void NavToUserManagement()
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            LoadConfiguration();
            CurrentViewModel = ContainerHelper.Container.Resolve<UserManagementViewModel>();
        }

        private void NavToDbManagement()
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            LoadConfiguration();
            CurrentViewModel = ContainerHelper.Container.Resolve<DbManagementViewModel>();
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

        private void NavToSettings()
        {
            if (ClockViewModel == null)
                ClockViewModel = ContainerHelper.Container.Resolve<ClockViewModel>();

            CurrentViewModel = _settingsViewModel;
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
                string json = File.ReadAllText(ConfigurationManager.AppSettings["configPath"]);
                ConnectionHelper.Config = JsonConvert.DeserializeObject<Config>(json);

                //Test connection
                
                using (AmsModel dbContext = ConnectionHelper.CreateConnection())
                {
                    new LevelsRepository().GetLevels().ToList();
                }

                if (string.IsNullOrEmpty(ConnectionHelper.Config.AppConfiguration.Settings.SmsService.Url))
                    throw new ArgumentNullException();
            }
            catch (FileNotFoundException error)
            {
                CreateConfig();
                StartupConfigWindow = new StartupConfigWindow();
                StartupConfigWindow.ShowDialog();
                Logger.Error("File not found: ", error);
                System.Environment.Exit(0);
            }
            catch (Exception error)
            {
                StartupConfigWindow = new StartupConfigWindow();
                StartupConfigWindow.ShowDialog();
                Logger.Error("ERROR: ", error);
                System.Environment.Exit(0);
            }
        }

        private void CreateConfig()
        {
            var config = new Config()
            {
                AppConfiguration = new AppConfiguration()
                {
                    Settings = new SJBCS.Data.Settings()
                    {
                        DataSource = new DataSource()
                        {
                            Metadata = "res://*/AmsModel.csdl|res://*/AmsModel.ssdl|res://*/AmsModel.msl",
                            Hostname = "",
                            InitialCatalog = "AMS",
                            Username = "",
                            Password = "",
                        },
                        SmsService = new SmsService()
                        {
                            Url = "http://192.168.43.1:8080/"
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(config);
            ConnectionHelper.Config = config;
            File.WriteAllText(ConfigurationManager.AppSettings["configPath"], json);
        }

        private async void ProgressDialog()
        {
            var metroDialogSettings = new MetroDialogSettings
            {
                CustomResourceDictionary = DialogDictionary,
                NegativeButtonText = "CANCEL",
                SuppressDefaultResources = true
            };

            var controller = await DialogCoordinator.Instance.ShowProgressAsync(this, "MahApps Dialog", "Using Material Design Themes (WORK IN PROGRESS)", true, metroDialogSettings);
            controller.SetIndeterminate();
            await Task.Delay(3000);
            await controller.CloseAsync();
        }

    }
}
