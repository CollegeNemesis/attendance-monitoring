using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    public class DialogHelper
    {
        public static async Task<bool> ShowDialog(DialogType dialogType, string message)
        {
            try
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
            catch (InvalidOperationException)
            {
                DialogHost.CloseDialogCommand.Execute(new object(), null);
                return false;
            }
        }

        private static void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) { }
    }
}
