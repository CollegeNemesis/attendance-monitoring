using AMS.Utilities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    
    public static class MessageDialog
    {
        public static bool isDialogOpen;
        public static object _messageContent;
        public static RelayCommand OpenDialogCommand { get; private set; }

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
                isDialogOpen = false;
            }
        }
    }
    public class MessageDialogViewModel : BindableBase
    {
        private string _statusColor;

        public string StatusColor
        {
            get { return _statusColor; }
            set { SetProperty(ref _statusColor, value); }
        }
        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _dialogType;

        public string DialogType
        {
            get { return _dialogType; }
            set { SetProperty(ref _dialogType, value); }
        }


        public MessageDialogViewModel(MessageType messageType, String message)
        {
            _message = message;
            if (messageType.Equals(MessageType.Error))
            {
                _dialogType = "Alert";
                _statusColor = "Red";
            }
            else
            {
                _dialogType = "Information";
                _statusColor = "Yellow";
            }
        }
    }
}
