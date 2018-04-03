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
        public SectionViewModel _sectionViewModel;
        public ReportViewModel _reportViewModel;
        public AttendanceViewModel _attendanceViewModel;
        public AddEditStudentViewModel _addEditStudentViewModel;
        public StudentViewModel _studentViewModel;
        public UserManagementViewModel _userManagementViewModel;
        public ConfigManagementViewModel _configManagementViewModel;
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

        private ClockViewModel _clockViewModel1;

        private ClockViewModel _clockViewModel;
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
            _clockViewModel1 = ContainerHelper.Container.Resolve<ClockViewModel>();
            _clockViewModel = _clockViewModel1;
            _attendanceViewModel = ContainerHelper.Container.Resolve<AttendanceViewModel>();
            _sectionViewModel = ContainerHelper.Container.Resolve<SectionViewModel>();
            _groupViewModel = ContainerHelper.Container.Resolve<GroupViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _reportViewModel = ContainerHelper.Container.Resolve<ReportViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();
            _settingsViewModel = ContainerHelper.Container.Resolve<SettingsViewModel>();
            _configManagementViewModel = ContainerHelper.Container.Resolve<ConfigManagementViewModel>();
            _userManagementViewModel = ContainerHelper.Container.Resolve<UserManagementViewModel>();
            _startupConfigWindowViewModel = ContainerHelper.Container.Resolve<StartupConfigWindowViewModel>();

            _currentViewModel = _loginViewModel;
            _menu = null;

            //_currentViewModel = _studentViewModel;
            //_menu = _menuViewModel;

            _addEditStudentViewModel.Done += NavToStudent;
            _addEditStudentViewModel.EditMode = false;
            _loginViewModel.LoginRequested += NavToMenu;
            _loginViewModel.EntryRequested += NavToFreeUser;
            _menuViewModel.NavToAttendanceRequested += NavToAttendance;
            _menuViewModel.NavToSettingsRequested += NavToSettings;
            _menuViewModel.NavToStudentRequested += NavToStudent;
            _menuViewModel.NavToSectionRequested += NavToSection;
            _menuViewModel.NavToGroupRequested += NavToGroup;
            _menuViewModel.NavToReportRequested += NavToReport;
            _studentViewModel.AddRequested += NavToAddStudent;
            _studentViewModel.EditRequested += NavToEditStudent;
            _settingsViewModel.NavToHomeRequested += NavToHome;
            _settingsViewModel.NavToConfigRequested += NavToConfig;
            _settingsViewModel.NavToUserRequested += NavToUser;
        }

        private void NavToUser()
        {
            ClockViewModel = _clockViewModel1;
            CurrentViewModel = _userManagementViewModel;
        }

        private void NavToConfig()
        {
            ClockViewModel = _clockViewModel1;
            CurrentViewModel = _configManagementViewModel;
        }

        private void NavToHome()
        {
            _studentViewModel.LoadStudents();
            Menu = _menuViewModel;
            ClockViewModel = _clockViewModel1;
            CurrentViewModel = _studentViewModel;
        }

        private void NavToReport()
        {
            CurrentViewModel = _reportViewModel;
        }

        private void NavToGroup()
        {
            ClockViewModel = _clockViewModel1;
            CurrentViewModel = _groupViewModel;
        }

        private void NavToSection()
        {
            ClockViewModel = _clockViewModel1;
            CurrentViewModel = _sectionViewModel;
        }

        private void NavToStudent()
        {
            _studentViewModel.LoadStudents();
            ClockViewModel = _clockViewModel1;
            CurrentViewModel = _studentViewModel;
        }

        private void NavToSettings()
        {
            Menu = _settingsViewModel;
            ClockViewModel = _clockViewModel1;
            CurrentViewModel = _userManagementViewModel;
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

        private void NavToFreeUser()
        {
            _studentViewModel.LoadStudents();
            ClockViewModel = _clockViewModel1;
            Menu = _menuViewModel;
            CurrentViewModel = _studentViewModel;
        }

        private void NavToMenu(User user)
        {
            ClockViewModel = _clockViewModel1;
            if (user.Type.ToLower().Equals("admin"))
            {
                _studentViewModel.LoadStudents();
                ClockViewModel = _clockViewModel1;
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
                LoadConfiguration();
                //System.Environment.Exit(0);
            }
            catch (Exception error)
            {
                StartupConfigWindow = new StartupConfigWindow();
                StartupConfigWindow.ShowDialog();
                Logger.Error("ERROR: ", error);
                LoadConfiguration();
                //System.Environment.Exit(0);
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
                            Metadata = "res://*/DataModel.csdl|res://*/DataModel.ssdl|res://*/DataModel.msl",
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
    }
}
