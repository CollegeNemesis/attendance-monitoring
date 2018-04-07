using SJBCS.GUI.Utilities;
using System;
using System.Globalization;
using System.Windows.Threading;

namespace SJBCS.GUI.Home
{
    public class MainClockViewModel : BindableBase
    {
        private string _digitalClock;
        public string DigitalClock
        {
            get { return _digitalClock; }
            set { SetProperty(ref _digitalClock, value); }
        }

        private string _digitalCalendar;
        public string DigitalCalendar
        {
            get { return _digitalCalendar; }
            set { SetProperty(ref _digitalCalendar, value); }
        }

        public MainClockViewModel()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DigitalClock = DateTime.Now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            DigitalCalendar = DateTime.Now.ToString("dddd MMMM dd, yyyy", CultureInfo.InvariantCulture);
        }

    }
}
