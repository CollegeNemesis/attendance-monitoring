using DPFP;
using DPFP.Verification;
using SJBCS.Model;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using static DPFP.Verification.Verification;

namespace SJBCS.ViewModel
{
    class AttendanceViewModel : FingerScanner, INotifyPropertyChanged
    {
        private string _digitalClock;
        private string _digitalCalendar;
        private string _status;
        private Template Template;
        private Verification Verificator;
        private Biometric _biometric;
        private BiometricWrapper _bioWrapper;
        private ObservableCollection<Object> _fptList;
        private static AMSEntities DBContext = new AMSEntities();

        public event PropertyChangedEventHandler PropertyChanged;

        public AttendanceViewModel()
        {
            //Start of Digital Clock
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            //End of Digital Clock

            //Initiate Finger Scanning
            _biometric = new Biometric();
            _bioWrapper = new BiometricWrapper();
            _fptList = _bioWrapper.RetrieveAll(DBContext, _biometric); //Load all FingerPrintTemplate (fpt);

            Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
            Start();
        }
        
        public String Status => _status;

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

        protected override void Process(Sample Sample)
        {
            base.Process(Sample);

            // Process the sample and create a feature set for the enrollment purpose.
            FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (features != null)
            {
                MemoryStream fingerprintData = null;
                Result result = null;
                // Loop on the FPT List from DB to Compare the feature set with the DB templates
                foreach (var temp in _fptList)
                {
                    Biometric biometric = (Biometric)temp;
                    fingerprintData = new MemoryStream(biometric.FingerPrintTemplate);
                    Template = new Template(fingerprintData);
                    result = new Result();
                    Verificator.Verify(features, Template, ref result);

                    if (result.Verified)
                        _status = "VERIFIED.";

                    else
                        _status = "NOT VERIFIED.";
                    RaisePropertyChanged(null);
                }
                
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DigitalClock = DateTime.Now.ToLongTimeString();
            DigitalCalendar = DateTime.Now.ToLongDateString();
        }

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}
