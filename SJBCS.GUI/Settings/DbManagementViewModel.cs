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
    public class DbManagementViewModel : BindableBase
    {
        private EditableDbConfig _editableDbConfig;

        public EditableDbConfig EditableDbConfig
        {
            get { return _editableDbConfig; }
            set { SetProperty(ref _editableDbConfig, value); }
        }

        public RelayCommand UpdateDbConfigCommand { get; private set; }
        public RelayCommand TestDbCommand { get; private set; }

        public DbManagementViewModel()
        {
            Config config = ConnectionHelper.Config;
            UpdateDbConfigCommand = new RelayCommand(OnUpdateDbConfig, CanUpdate);
            TestDbCommand = new RelayCommand(OnTestDb, CanUpdate);
            if (EditableDbConfig != null) EditableDbConfig.ErrorsChanged -= RaiseCanExecuteChanged;

            EditableDbConfig = new EditableDbConfig();

            EditableDbConfig.ErrorsChanged += RaiseCanExecuteChanged;

            EditableDbConfig.Hostname = config.AppConfiguration.Settings.DataSource.Hostname;
            EditableDbConfig.InitialCatalog = config.AppConfiguration.Settings.DataSource.InitialCatalog;
            EditableDbConfig.Username = config.AppConfiguration.Settings.DataSource.Username;
            EditableDbConfig.Password = config.AppConfiguration.Settings.DataSource.Password;
        }

        private async void OnTestDb()
        {
            Config tempConfig = new Config()
            {
                AppConfiguration = new AppConfiguration()
                {
                    Settings = new Data.Settings()
                    {
                        DataSource = new DataSource()
                        {
                            Metadata = "res://*/AmsModel.csdl|res://*/AmsModel.ssdl|res://*/AmsModel.msl",
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

                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Informational, "Connection to database established!")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            }
            catch(Exception error)
            {
                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Error, error.Message)
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
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

        private async void OnUpdateDbConfig()
        {
            try
            {

                Config config = ConnectionHelper.Config;
                config.AppConfiguration.Settings.DataSource.Hostname = EditableDbConfig.Hostname;
                config.AppConfiguration.Settings.DataSource.InitialCatalog = EditableDbConfig.InitialCatalog;
                config.AppConfiguration.Settings.DataSource.Username = EditableDbConfig.Username;
                config.AppConfiguration.Settings.DataSource.Password = EditableDbConfig.Password;
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(ConfigurationManager.AppSettings["configPath"], json);

                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Informational, "Data source information updated.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            }
            catch (Exception error)
            {
                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Error, error.Message)
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
