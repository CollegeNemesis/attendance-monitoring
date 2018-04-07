using Newtonsoft.Json;
using SJBCS.Data;
using SJBCS.GUI.AMS;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Home;
using SJBCS.GUI.Report;
using SJBCS.GUI.Settings;
using SJBCS.GUI.Student;
using SJBCS.GUI.Utilities;
using SJBCS.Services.Repository;
using SJBCS.SMS;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using Unity;

namespace SJBCS.GUI
{
    class MainWindowViewModel : BindableBase
    {
        #region Properties
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private WebServiceHost serviceHost;

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

        public StartupConfigManagementWindow _startupConfigManagementWindow;
        public StartupConfigManagementWindow StartupConfigManagementWindow
        {
            get { return _startupConfigManagementWindow; }
            set { SetProperty(ref _startupConfigManagementWindow, value); }
        }

        private bool _adminMode;

        public bool AdminMode
        {
            get { return _adminMode; }
            set { SetProperty(ref _adminMode, value); }
        }

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

        private MainClockViewModel _mainClockViewModel;
        public MainClockViewModel MainClockViewModel
        {
            get { return _mainClockViewModel; }
            set { SetProperty(ref _mainClockViewModel, value); }
        }

        private ClockViewModel _clockViewModel;
        public ClockViewModel ClockViewModel
        {
            get { return _clockViewModel; }
            set { SetProperty(ref _clockViewModel, value); }
        }

        public RelayCommand LogoutCommand { get; private set; }
        #endregion

        public MainWindowViewModel()
        {
            LoadConfiguration();
            StartSMSService();
            _loginViewModel = ContainerHelper.Container.Resolve<LoginViewModel>();
            _menuViewModel = ContainerHelper.Container.Resolve<MenuViewModel>();
            _mainClockViewModel = ContainerHelper.Container.Resolve<MainClockViewModel>();
            _attendanceViewModel = ContainerHelper.Container.Resolve<AttendanceViewModel>();
            _sectionViewModel = ContainerHelper.Container.Resolve<SectionViewModel>();
            _groupViewModel = ContainerHelper.Container.Resolve<GroupViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _reportViewModel = ContainerHelper.Container.Resolve<ReportViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();
            _settingsViewModel = ContainerHelper.Container.Resolve<SettingsViewModel>();
            _configManagementViewModel = ContainerHelper.Container.Resolve<ConfigManagementViewModel>();
            _userManagementViewModel = ContainerHelper.Container.Resolve<UserManagementViewModel>();

            LogoutCommand = new RelayCommand(OnLogout);

            //Setup Window Content
            _currentViewModel = _loginViewModel;
            _menu = null;
            _adminMode = false;

            _loginViewModel.LoginRequested += NavToMenu;
            _loginViewModel.EntryRequested += NavToFreeLogin;

            _menuViewModel.NavToAttendanceRequested += NavToAttendance;
            _menuViewModel.NavToSettingsRequested += NavToSettings;
            _menuViewModel.NavToStudentRequested += NavToStudent;
            _menuViewModel.NavToSectionRequested += NavToSection;
            _menuViewModel.NavToGroupRequested += NavToGroup;
            _menuViewModel.NavToReportRequested += NavToReport;

            _studentViewModel.AddRequested += NavToAddStudent;
            _studentViewModel.EditRequested += NavToEditStudent;
            _addEditStudentViewModel.Done += NavToStudent;
            _addEditStudentViewModel.EditMode = false;

            _settingsViewModel.NavToHomeRequested += NavToHome;
            _settingsViewModel.NavToConfigRequested += NavToConfig;
            _settingsViewModel.NavToUserRequested += NavToUser;
        }

        private void OnLogout()
        {
            _loginViewModel.Username = "";
            CurrentViewModel = _loginViewModel;
            Menu = null;
            AdminMode = false;
            serviceHost?.Close();
        }

        #region Methods
        private void NavToUser()
        {
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _userManagementViewModel;
        }

        private void NavToConfig()
        {
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _configManagementViewModel;
        }

        private void NavToHome()
        {
            _studentViewModel.LoadStudents();

            Menu = _menuViewModel;
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _studentViewModel;
        }

        private void NavToReport()
        {
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _reportViewModel;
        }

        private void NavToGroup()
        {
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _groupViewModel;
        }

        private void NavToSection()
        {
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _sectionViewModel;
        }

        private async void NavToStudent()
        {
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _studentViewModel;

            try
            {
                _studentViewModel.LoadStudents();
            }
            catch(Exception error)
            {
                var result =  await DialogHelper.ShowDialog(DialogType.Error, "Something went wrong. Please try again.");
                Logger.Error(error);
            }
        }

        private void NavToSettings()
        {
            Menu = _settingsViewModel;
            ClockViewModel = _clockViewModel;
            CurrentViewModel = _userManagementViewModel;
        }

        private async void NavToAttendance()
        {
            ClockViewModel = null;
            CurrentViewModel = _attendanceViewModel;

            try
            {
                _attendanceViewModel.SwitchOn();
            }
            catch (Exception error)
            {
                var result =  await DialogHelper.ShowDialog(DialogType.Error, "Something went wrong. Please try restart the application.");
                Logger.Error(error);
            }
        }

        private async void NavToAddStudent(Data.Student selectedStudent)
        {
            CurrentViewModel = _addEditStudentViewModel;

            try
            {
                _attendanceViewModel.SwitchOff();
                _addEditStudentViewModel.EditMode = false;
                _addEditStudentViewModel.SetStudent(selectedStudent);
                _addEditStudentViewModel.Initialize();
            }
            catch (Exception error)
            {
                var result =  await DialogHelper.ShowDialog(DialogType.Error, "Something went wrong. Please try again.");
                Logger.Error(error);
            }
        }

        private void NavToEditStudent(Data.Student selectedStudent)
        {            
            CurrentViewModel = _addEditStudentViewModel;

            try
            {
                _attendanceViewModel.SwitchOff();
                _addEditStudentViewModel.EditMode = true;
                _addEditStudentViewModel.SetStudent(selectedStudent);
                _addEditStudentViewModel.Initialize();
            }
            catch (Exception error)
            {
                var result =  DialogHelper.ShowDialog(DialogType.Error, "Something went wrong. Please try again.");
                Logger.Error(error);
            }
        }

        private async void NavToFreeLogin()
        {
            ClockViewModel = null;
            Menu = null;
            CurrentViewModel = _attendanceViewModel;

            try
            {
                _attendanceViewModel.SwitchOn();
            }
            catch (Exception error)
            {
                var result =  await DialogHelper.ShowDialog(DialogType.Error, "Something went wrong. Please try restart the application.");
                Logger.Error(error);
            }
        }

        private void NavToMenu(Data.User user)
        {
            if (user.Type.ToLower().Equals("admin"))
            {
                Menu = _menuViewModel;
                AdminMode = true;
                NavToStudent();
            }
            else if (user.Type.ToLower().Equals("user"))
            {
                AdminMode = false;
                NavToFreeLogin();
            }
        }

        private void LoadConfiguration()
        {
            try
            {
                string json = File.ReadAllText(ConfigurationManager.AppSettings["configPath"]);
                ConnectionHelper.Config = JsonConvert.DeserializeObject<Config>(json);

                TestConnection();

                if (string.IsNullOrEmpty(ConnectionHelper.Config.AppConfiguration.Settings.SmsService.Url))
                    throw new ArgumentNullException();
            }
            catch (FileNotFoundException error)
            {
                CreateConfiguration();
                StartupConfigManagementWindow = new StartupConfigManagementWindow();
                StartupConfigManagementWindow.ShowDialog();
                Logger.Error(error);
                LoadConfiguration();
            }
            catch (Exception error)
            {
                StartupConfigManagementWindow = new StartupConfigManagementWindow();
                StartupConfigManagementWindow.ShowDialog();
                Logger.Error(error);
                LoadConfiguration();
            }
        }

        private async void CreateConfiguration()
        {
            try
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
            catch(Exception error)
            {
                var result =  await DialogHelper.ShowDialog(DialogType.Error, "Something went wrong setting up configuration files. Please check the logs.");
                Logger.Error(error);
            }
        }

        private void TestConnection()
        {
            using (AmsModel dbContext = ConnectionHelper.CreateConnection())
            {
                new LevelsRepository().GetLevels().ToList();
            }
        }

        private void StartSMSService()
        {
            string ipAddress = GetIPAddress();
            Uri baseAddress = new Uri(String.Format("http://{0}:54000/SMSService/", ipAddress));
            serviceHost = new WebServiceHost(typeof(RESTService), baseAddress);

            try
            {
                serviceHost.AddServiceEndpoint(typeof(IRESTService), new WebHttpBinding(), "");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true
                };
                serviceHost.Description.Behaviors.Add(smb);
                serviceHost.Description.Namespace = "http://SJBCS.SMS";
                serviceHost.Open();
                Logger.Info("SMS Service started");
            }
            catch (CommunicationException ce)
            {
                Logger.Error("Error encountered when starting SMS Service", ce);
                serviceHost.Abort();
            }
            catch (Exception e)
            {
                Logger.Error("Error encountered when starting SMS Service", e);
                serviceHost.Abort();
            }
        }

        private string GetIPAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 80);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }
        #endregion
    }
}
