using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SJBCS.GUI.Dialogs
{
    public class WaitingScreenViewModel
    {
        private ResourceDictionary DialogDictionary = new ResourceDictionary() { Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml") };

        public async void ProgressDialog()
        {
            var metroDialogSettings = new MetroDialogSettings
            {
                CustomResourceDictionary = DialogDictionary,
                //NegativeButtonText = "CANCEL",
                SuppressDefaultResources = true
            };

            var controller = await DialogCoordinator.Instance.ShowProgressAsync(this, "", "(WORK IN PROGRESS)", true, metroDialogSettings);
            controller.SetIndeterminate();
            await Task.Delay(6000);
            await controller.CloseAsync();
        }
    }
}
