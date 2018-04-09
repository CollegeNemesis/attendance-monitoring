using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    public class LoadingWindowHelper
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async void Open()
        {
            try
            {
                //Create the dialog
                var view = new LoadingWindow
                {
                    DataContext = new LoadingWindowViewModel()
                };

                //Show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            }
            catch (InvalidOperationException error)
            {
                Logger.Error(error);
            }

            catch (Exception error)
            {
                Logger.Error(error);
            }
        }

        public static void Close()
        {
            try
            {
                DialogHost.CloseDialogCommand.Execute(new object(), null);
            }
            catch (InvalidOperationException error)
            {
                Logger.Error(error);
            }

            catch (Exception error)
            {
                Logger.Error(error);
            }
        }

        private static void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) { }
    }
}
