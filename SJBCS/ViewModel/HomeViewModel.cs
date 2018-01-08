using SJBCS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SJBCS.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private string _digitalClock;
        private string _digitalCalendar;


        public event PropertyChangedEventHandler PropertyChanged;

        public HomeViewModel()
        {
            //Start of Digital Clock
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            //End of Digital Clock
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DigitalClock = DateTime.Now.ToLongTimeString();
            DigitalCalendar = DateTime.Now.ToLongDateString();
        }
        public string DigitalClock
        {
            get { return _digitalClock; }
            set { _digitalClock = value; RaisePropertyChanged("DigitalClock"); }
        }
        public string DigitalCalendar
        {
            get { return _digitalCalendar; }
            set { _digitalCalendar = value; RaisePropertyChanged("DigitalCalendar"); }
        }

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
