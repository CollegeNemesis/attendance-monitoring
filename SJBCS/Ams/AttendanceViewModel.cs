using SJBCS.Util;

namespace SJBCS.Ams
{
    public class AttendanceViewModel : BindableBase
    {
        private string _digitalClock;
        private string _digitalCalendar;

        public string DigitalClock
        {
            get { return _digitalClock; }
            set { SetProperty(ref _digitalClock, value); }
        }
        public string DigitalCalendar
        {
            get { return _digitalCalendar; }
            set { SetProperty(ref _digitalCalendar, value); }
        }
    }
}
