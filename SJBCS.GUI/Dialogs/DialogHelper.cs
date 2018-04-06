using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    public class DialogHelper
    {
        public static async Task<bool> ShowDialog(DialogType dialogType, string message)
        {
            //Create the dialog
            var view = new DialogBoxView
            {
                DataContext = new DialogBoxViewModel(dialogType, message)
            };

            //Show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            return (bool)result;
        }

        private static void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) { }
    }
}
