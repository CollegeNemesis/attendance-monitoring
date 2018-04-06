using SJBCS.GUI.Utilities;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using SJBCS.Services.Repository;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;


namespace SJBCS.GUI.Settings
{
    public class StartupConfigManagementWindowViewModel : BindableBase
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LoadingWindowHandler LoadingScreen;

        private bool closeTrigger;
        public bool CloseTrigger
        {
            get { return this.closeTrigger; }
            set
            {
                SetProperty(ref closeTrigger, value);
            }
        }

        private EditableConfig _editableConfig;
        public EditableConfig EditableConfig
        {
            get { return _editableConfig; }
            set => SetProperty(ref _editableConfig, value);
        }

        private Config _config;
        public Config Config
        {
            get { return _config; }
            set { SetProperty(ref _config, value); }
        }

        public RelayCommand UpdateDbConfigCommand { get; private set; }
        public RelayCommand TestDbCommand { get; private set; }

        public StartupConfigManagementWindowViewModel()
        {
            Config = ConnectionHelper.Config;
            UpdateDbConfigCommand = new RelayCommand(OnUpdateDbConfig, CanUpdate);
            TestDbCommand = new RelayCommand(OnTestDb, CanTest);

            if (EditableConfig != null) EditableConfig.ErrorsChanged -= RaiseCanExecuteChanged;
            EditableConfig = new EditableConfig();
            EditableConfig.ErrorsChanged += RaiseCanExecuteChanged;

            EditableConfig.Hostname = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Hostname;
            EditableConfig.InitialCatalog = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.InitialCatalog;
            EditableConfig.Username = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Username;
            EditableConfig.Password = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Password;
            EditableConfig.Url = ConnectionHelper.Config.AppConfiguration.Settings.SmsService.Url;

            LoadingScreen = new LoadingWindowHandler();
        }

        private async void OnTestDb()
        {
            SetConfiguration();

            try
            {
                LoadingScreen.Start();
                TestConnection();
                LoadingScreen.Stop();
                await DialogHelper.ShowDialog(DialogType.Success, "Connection established.");
            }
            catch (Exception error)
            {
                LoadingScreen.Stop();
                ConnectionHelper.Config = Config;
                await DialogHelper.ShowDialog(DialogType.Error, "Connection cannot be established.");
                Logger.Error(error);
            }
        }
        private bool CanTest()
        {
            return !EditableConfig.HasErrors;
        }

        private async void OnUpdateDbConfig()
        {
            SetConfiguration();

            try
            {
                TestConnection();
                string json = JsonConvert.SerializeObject(ConnectionHelper.Config);
                File.WriteAllText(ConfigurationManager.AppSettings["configPath"], json);
                await DialogHelper.ShowDialog(DialogType.Success, "Connection established.");
                CloseTrigger = true;
            }
            catch (Exception error)
            {
                ConnectionHelper.Config = Config;
                await DialogHelper.ShowDialog(DialogType.Error, "Connection cannot be established.");
            }
        }
        private bool CanUpdate()
        {
            return !EditableConfig.HasErrors;
        }

        private void TestConnection()
        {
            using (AmsModel dbContext = ConnectionHelper.CreateConnection())
            {
                new LevelsRepository().GetLevels().ToList();
            }
        }

        private void SetConfiguration()
        {
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Hostname = EditableConfig.Hostname;
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.InitialCatalog = EditableConfig.InitialCatalog;
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Username = EditableConfig.Username;
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Password = EditableConfig.Password;
            ConnectionHelper.Config.AppConfiguration.Settings.SmsService.Url = EditableConfig.Url;
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            UpdateDbConfigCommand.RaiseCanExecuteChanged();
            TestDbCommand.RaiseCanExecuteChanged();
        }
    }
}
