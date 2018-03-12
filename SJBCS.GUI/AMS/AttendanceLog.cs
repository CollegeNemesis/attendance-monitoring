using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.AMS
{
    public class AttendanceLog
    {
        private string _imageData;

        public string ImageData
        {
            get { return _imageData; }
            set { _imageData = value; }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }


        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private string _action;

        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        private DateTime? _timestamp;

        public DateTime? Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        public AttendanceLog(string icon, string status, string imageData, string firstName, string lastName, string action, DateTime? timestamp)
        {
            _imageData = imageData;
            _firstName = firstName;
            _lastName = lastName;
            _action = action;
            _timestamp = timestamp;
            _icon = icon;
            _status = status;
        }
    }
}
