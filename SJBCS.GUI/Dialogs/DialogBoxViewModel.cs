﻿using SJBCS.GUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    public enum DialogType
    {
        Success,
        Error,
        Informational,
        Warning,
        Validation
    };
    public class DialogBoxViewModel : BindableBase
    {
        private DialogType _messageType;

        public DialogType MessageType
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

        public DialogBoxViewModel(DialogType messageType, string message)
        {
            Message = message;
            MessageType = messageType;
        }

    }
}
