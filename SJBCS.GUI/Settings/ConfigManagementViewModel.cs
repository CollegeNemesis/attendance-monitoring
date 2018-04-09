using Newtonsoft.Json;
using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Utilities;
using SJBCS.Services.Repository;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SJBCS.GUI.Settings
{
    public class ConfigManagementViewModel : BindableBase
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        

        private bool closeTrigger;
        public bool CloseTrigger
        {
            get { return this.closeTrigger; }
            set
            {
                SetProperty(ref closeTrigger, value);
            }
        }

        private Config Config;

        private EditableDbConfig _editableDbConfig;
        public EditableDbConfig EditableDbConfig
        {
            get { return _editableDbConfig; }
            set => SetProperty(ref _editableDbConfig, value);
        }

        private EditableSmsConfig _editableSmsConfig;
        public EditableSmsConfig EditableSmsConfig
        {
            get { return _editableSmsConfig; }
            set => SetProperty(ref _editableSmsConfig, value);
        }

        public RelayCommand UpdateDbConfigCommand { get; private set; }
        public RelayCommand TestDbCommand { get; private set; }

        public ConfigManagementViewModel()
        {
            UpdateDbConfigCommand = new RelayCommand(OnUpdateDbConfig, CanUpdate);
            TestDbCommand = new RelayCommand(OnTestDb, CanTest);

            if (EditableDbConfig != null)
            {
                EditableDbConfig.ErrorsChanged -= RaiseCanExecuteChanged;
                EditableSmsConfig.ErrorsChanged -= RaiseCanExecuteChanged;
            }

            EditableDbConfig = new EditableDbConfig();
            EditableSmsConfig = new EditableSmsConfig();

            EditableDbConfig.ErrorsChanged += RaiseCanExecuteChanged;
            EditableSmsConfig.ErrorsChanged += RaiseCanExecuteChanged;

            EditableDbConfig.Hostname = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Hostname;
            EditableDbConfig.InitialCatalog = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.InitialCatalog;
            EditableDbConfig.Username = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Username;
            EditableDbConfig.Password = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Password;
            EditableSmsConfig.Url = ConnectionHelper.Config.AppConfiguration.Settings.SmsService.Url;
        }

        private async void OnTestDb()
        {
            Config = ConnectionHelper.Config.Copy();
            SetConfiguration();

            try
            {
                LoadingWindowHelper.Open();
                await System.Threading.Tasks.Task.Run(() => TestConnection());
                LoadingWindowHelper.Close();
                var result = await DialogHelper.ShowDialog(DialogType.Success, "Connection established.");
            }
            catch (Exception error)
            {
                LoadingWindowHelper.Close();
                ConnectionHelper.Config = Config;
                var result = await DialogHelper.ShowDialog(DialogType.Error, "Connection cannot be established.");
                Logger.Error(error);
            }
        }

        private bool CanTest()
        {
            return !EditableDbConfig.HasErrors;
        }

        private async void OnUpdateDbConfig()
        {
            Config = ConnectionHelper.Config.Copy();
            SetConfiguration();

            try
            {
                LoadingWindowHelper.Open();
                await System.Threading.Tasks.Task.Run(() => TestConnection());
                LoadingWindowHelper.Close();
                string json = JsonConvert.SerializeObject(ConnectionHelper.Config);
                File.WriteAllText(ConfigurationManager.AppSettings["configPath"], json);
                var result = await DialogHelper.ShowDialog(DialogType.Success, "Successfully saved settings.");
                //CloseTrigger = true;
            }
            catch (Exception error)
            {
                LoadingWindowHelper.Close();
                ConnectionHelper.Config = Config;
                var result = await DialogHelper.ShowDialog(DialogType.Error, "Connection cannot be established.");
                Logger.Error(error);
            }
        }

        private bool CanUpdate()
        {
            return !EditableDbConfig.HasErrors && !EditableSmsConfig.HasErrors;
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
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Hostname = EditableDbConfig.Hostname;
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.InitialCatalog = EditableDbConfig.InitialCatalog;
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Username = EditableDbConfig.Username;
            ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Password = EditableDbConfig.Password;
            ConnectionHelper.Config.AppConfiguration.Settings.SmsService.Url = EditableSmsConfig.Url;
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            UpdateDbConfigCommand.RaiseCanExecuteChanged();
            TestDbCommand.RaiseCanExecuteChanged();
        }
    }
}
