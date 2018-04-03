using AMS.Utilities;
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
    public class StartupConfigWindowViewModel : BindableBase
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EditableConfig _editableDbConfig;
        public EditableConfig EditableDbConfig
        {
            get { return _editableDbConfig; }
            set
            {
                SetProperty(ref _editableDbConfig, value);
                TestDbCommand.RaiseCanExecuteChanged();
            }
        }

        private bool closeTrigger;
        public bool CloseTrigger
        {
            get { return this.closeTrigger; }
            set
            {
                SetProperty(ref closeTrigger, value);
            }
        }

        public RelayCommand UpdateDbConfigCommand { get; private set; }
        public RelayCommand TestDbCommand { get; private set; }

        public StartupConfigWindowViewModel()
        {
            Config config = ConnectionHelper.Config;
            UpdateDbConfigCommand = new RelayCommand(OnUpdateDbConfig, CanUpdate);
            TestDbCommand = new RelayCommand(OnTestDb, CanTest);
            if (EditableDbConfig != null) EditableDbConfig.ErrorsChanged -= RaiseCanExecuteChanged;

            EditableDbConfig = new EditableConfig();

            EditableDbConfig.ErrorsChanged += RaiseCanExecuteChanged;

            EditableDbConfig.Hostname = config.AppConfiguration.Settings.DataSource.Hostname;
            EditableDbConfig.InitialCatalog = config.AppConfiguration.Settings.DataSource.InitialCatalog;
            EditableDbConfig.Username = config.AppConfiguration.Settings.DataSource.Username;
            EditableDbConfig.Password = config.AppConfiguration.Settings.DataSource.Password;
            EditableDbConfig.Url = config.AppConfiguration.Settings.SmsService.Url;
        }

        private async void OnTestDb()
        {
            WaitingScreenView waitingScreen = new WaitingScreenView();
            if (TestConnection())
            {
                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Informational, "Connection has been established.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            }
            else
            {
                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Informational, "Connection cannot be established.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            }
        }

        private bool TestConnection()
        {
            Config tempConfig = new Config()
            {
                AppConfiguration = new AppConfiguration()
                {
                    Settings = new Data.Settings()
                    {
                        DataSource = new DataSource()
                        {
                            Metadata = "res://*/DataModel.csdl|res://*/DataModel.ssdl|res://*/DataModel.msl",
                            Hostname = "",
                            InitialCatalog = "",
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

            Config origConfig = ConnectionHelper.Config;

            tempConfig.AppConfiguration.Settings.DataSource.Hostname = EditableDbConfig.Hostname;
            tempConfig.AppConfiguration.Settings.DataSource.InitialCatalog = EditableDbConfig.InitialCatalog;
            tempConfig.AppConfiguration.Settings.DataSource.Username = EditableDbConfig.Username;
            tempConfig.AppConfiguration.Settings.DataSource.Password = EditableDbConfig.Password;

            ConnectionHelper.Config = tempConfig;

            try
            {
                using (AmsModel dbContext = ConnectionHelper.CreateConnection())
                {
                    new LevelsRepository().GetLevels().ToList();
                }
                return true;
            }
            catch (Exception error)
            {
                Logger.Error(error);
                return false;
            }
            finally
            {
                ConnectionHelper.Config = origConfig;
            }
        }

        private bool CanUpdate()
        {
            return !EditableDbConfig.HasErrors;
        }

        private bool CanTest()
        {
            if (string.IsNullOrEmpty(EditableDbConfig.Hostname) || string.IsNullOrEmpty(EditableDbConfig.InitialCatalog) || string.IsNullOrEmpty(EditableDbConfig.Username) || string.IsNullOrEmpty(EditableDbConfig.Password))
                return false;
            return true;
        }

        private async void OnUpdateDbConfig()
        {
            if (TestConnection())
            {
                Config config = ConnectionHelper.Config;
                config.AppConfiguration.Settings.DataSource.Hostname = EditableDbConfig.Hostname;
                config.AppConfiguration.Settings.DataSource.InitialCatalog = EditableDbConfig.InitialCatalog;
                config.AppConfiguration.Settings.DataSource.Username = EditableDbConfig.Username;
                config.AppConfiguration.Settings.DataSource.Password = EditableDbConfig.Password;
                config.AppConfiguration.Settings.SmsService.Url = EditableDbConfig.Url;
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(ConfigurationManager.AppSettings["configPath"], json);

                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Informational, "Connection has been established. Settings has been saved.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                CloseTrigger = true;
            }
            else
            {
                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Error, "Settings cannot be save. Connection cannot be established.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            }
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            UpdateDbConfigCommand.RaiseCanExecuteChanged();
            TestDbCommand.RaiseCanExecuteChanged();
        }
    }
}
