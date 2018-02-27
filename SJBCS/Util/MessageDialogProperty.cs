using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SJBCS.Util
{
    public enum MessageType
    {
        Error,
        Informational
    };
    public static class MessageDialogProperty
    {
        
        public static bool _isMessageDialogOpen;
        public static object _messageContent;
        public static ICommand OpenMessageDialogCommand { get; }
        public static ICommand AcceptMessageDialogCommand { get; }

        public static void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) {; }
        public static void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs) {; }
        public static void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new MessageDialogView());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
        public static async void OpenDialog(MessageType messageType, String message)
        {
            var view = new MessageDialogView
            {
                DataContext = new MessageDialogViewModel(messageType, message)
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check if dialog has been acknowledged
            if ((Boolean)result)
            {
                MessageDialogProperty._isMessageDialogOpen = false;
            }
        }
    }
}
