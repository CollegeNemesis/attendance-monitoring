using AMS.Utilities;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;

namespace SJBCS.GUI.Settings
{
    public class SmsManagementViewModel : ValidatableBindableBase
    {
        private EditableSmsConfig _editableSmsConfig;

        public EditableSmsConfig EditableSmsConfig
        {
            get { return _editableSmsConfig; }
            set { SetProperty(ref _editableSmsConfig, value); }
        }

        public RelayCommand UpdateUrlCommand { get; private set; }

        public SmsManagementViewModel()
        {
            Config config = ConnectionHelper.Config;
            UpdateUrlCommand = new RelayCommand(OnUpdateUrl, CanUpdate);
            if (EditableSmsConfig != null) EditableSmsConfig.ErrorsChanged -= RaiseCanExecuteChanged;

            EditableSmsConfig = new EditableSmsConfig();

            EditableSmsConfig.ErrorsChanged += RaiseCanExecuteChanged;
            EditableSmsConfig.Url = config.AppConfiguration.Settings.SmsService.Url;
        }

        private bool CanUpdate()
        {
            return !EditableSmsConfig.HasErrors;
        }

        private async void OnUpdateUrl()
        {
            try
            {
                Config config = ConnectionHelper.Config;
                config.AppConfiguration.Settings.SmsService.Url = EditableSmsConfig.Url;
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(ConfigurationManager.AppSettings["configPath"], json);

                var view = new DialogBoxView
                {
                    DataContext = new DialogBoxViewModel(MessageType.Informational, "SMS URL has been updated.")
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
            UpdateUrlCommand.RaiseCanExecuteChanged();
        }
    }
}
