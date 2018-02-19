using System;

namespace SJBCS.Ams
{
    public class AttendanceLog
    {
        private String _imageData;
        private String _firstName;
        private String _lastName;
        private String _actionTaken;
        private String _actionTakenIcon;
        private String _statusColor;
        private DateTime _timestamp;

        public String ImageData
        {
            get
            {
                return _imageData;
            }
            set
            {
                ;
            }
        }
        public String FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                ;
            }
        }
        public String LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                ;
            }
        }
        public String ActionTaken
        {
            get
            {
                return _actionTaken;
            }
            set
            {
                ;
            }
        }
        public String ActionTakenIcon
        {
            get
            {
                return _actionTakenIcon;
            }
            set
            {
                ;
            }
        }
        public String StatusColor
        {
            get
            {
                return _statusColor;
            }
            set
            {
                ;
            }
        }
        public String Timestamp
        {
            get
            {
                return _timestamp.ToShortTimeString();
            }
            set
            {
                ;
            }
        }


        public AttendanceLog(String imageData,String firstName,String lastName, String actionTaken, String actionTakenIcon,string statusColor, DateTime timestamp)
        {
            _imageData = imageData;
            _firstName = firstName;
            _lastName = lastName;
            _actionTaken = actionTaken;
            _actionTakenIcon = actionTakenIcon;
            _statusColor = statusColor;
            _timestamp = timestamp;

        }

    }
}
