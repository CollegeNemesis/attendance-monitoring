using AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    public enum MessageType
    {
        Error,
        Informational,
        Warning
    };
    public class DialogBoxViewModel : BindableBase
    {
        private MessageType _messageType;

        public MessageType MessageType
        {
            get { return _messageType; }
            set { SetProperty(ref _messageType, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public DialogBoxViewModel(MessageType messageType, string message)
        {
            Message = message;
            MessageType = messageType;
        }

    }
}
