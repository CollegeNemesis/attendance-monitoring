using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.ViewModel
{
    public class MessageDialogBoxViewModel
    {
        private String _messageType;
        private String _message;
        private String _statusColor;

        public String Message
        {
            get
            {
                return _message;
            }
            set {; }
        }
        public String MessageType
        {
            get
            {
                if (_messageType.ToLower().Equals("error"))
                {
                    _statusColor = "Red";
                    return "Alert";
                }
                else if(_messageType.ToLower().Equals("information"))
                {
                    _statusColor = "Blue";
                    return "Information";
                }
                else
                {
                    _statusColor = "Red";
                    return "Alert";
                }
            }
            set {; }
        }
        public String StatusColor
        {
            get
            {
                return _statusColor;
            }
            set {; }
        }

        public MessageDialogBoxViewModel(String messageType, String message)
        {
            _messageType = messageType;
            _message = message;
            _statusColor = "Red";
        }
    }
}
