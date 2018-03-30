using AMS.Utilities;
using System;

namespace SJBCS.GUI.AMS
{
    public class AttendanceLog : BindableBase
    {
        #region Properties
        private string _imageData;
        public string ImageData
        {
            get { return _imageData; }
            set { SetProperty(ref _imageData, value); }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _action;
        public string Action
        {
            get { return _action; }
            set { SetProperty(ref _action, value); }
        }

        private DateTime? _timestamp;
        public DateTime? Timestamp
        {
            get { return _timestamp; }
            set { SetProperty(ref _timestamp, value); }
        }
        #endregion

        public AttendanceLog(string imageData, string firstName, string lastName, string action, DateTime? timestamp)
        {
            _imageData = imageData;
            _firstName = firstName;
            _lastName = lastName;
            _action = action;
            _timestamp = timestamp;
        }
    }
}
