using AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Dialogs
{
    public class DialogBoxViewModel : BindableBase
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public DialogBoxViewModel(string message)
        {
            Message = message;
        }

    }
}
