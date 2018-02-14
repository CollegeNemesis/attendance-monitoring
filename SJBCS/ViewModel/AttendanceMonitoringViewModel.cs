using DPFP;
using DPFP.Verification;
using MaterialDesignThemes.Wpf;
using SJBCS.Model;
using SJBCS.View;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static DPFP.Verification.Verification;

namespace SJBCS.ViewModel
{
    class AttendanceMonitoringViewModel : FingerScanner, INotifyPropertyChanged
    {
        #region Model Property
        private string _digitalClock;
        private string _digitalCalendar;

        private String _remarks;
        private String _statusColor;
        private Student _student;
        private Level _level;
        private Section _section;
        private Attendance _attendance;
        private Biometric _biometric;
        private RelBiometric _relBiometric;

        private StudentWrapper _studentWrapper;
        private LevelWrapper _levelWrapper;
        private SectionWrapper _sectionWrapper;
        private AttendanceWrapper _attendanceWrapper;
        private BiometricWrapper _biometricWrapper;
        private RelBiometricWrapper _relBioWrapper;

        private Template Template;
        private Verification Verificator;
        private ObservableCollection<Object> _fptList;

        private ObservableCollection<AttendanceLog> _attendanceLogList;
        #endregion

        #region MessageDialog
        private bool _isMessageDialogOpen;
        private bool _isInitial;
        private object _messageContent;
        public ICommand OpenMessageDialogCommand { get; }
        public ICommand AcceptMessageDialogCommand { get; }
        public bool IsMessageDialogOpen
        {
            get { return _isMessageDialogOpen; }
            set
            {
                if (_isMessageDialogOpen == value) return;
                _isMessageDialogOpen = value;
                OnPropertyChanged();
            }
        }
        public object MessageContent
        {
            get { return _messageContent; }
            set
            {
                if (_messageContent == value) return;
                _messageContent = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
        }
        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new MessageDialogBoxView());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void OpenMessageDialog(Object obj)
        {
            _messageContent = new MessageDialogBoxView();
            _isMessageDialogOpen = true;
            OnPropertyChanged();
        }
        private async void OpenDialog(String messageType, String message)
        {
            var view = new MessageDialogBoxView
            {
                DataContext = new MessageDialogBoxViewModel(messageType, message)
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check if dialog has been acknowledged
            if ((Boolean)result)
            {
                _isMessageDialogOpen = false;
            }
        }
        //How to call MessageDialogBox
        //_isMessageDialogOpen = true;
        //Application.Current.Dispatcher.Invoke((Action)delegate {
        //    OpenDialog("Error", "You're not allowed to logout 1 hour after logging in.");
        //});
        #endregion

        #region View Property
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<AttendanceLog> AttendanceLogList
        {
            get
            {
                return _attendanceLogList;
            }
            set
            {
                _attendanceLogList = value;
            }
        }
        public String Remarks
        {
            get
            {
                return _remarks;
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
        public String ActionTaken
        {
            get
            {
                return _actionTaken;
            }
            set
            {
                _actionTaken = value;
                RaisePropertyChanged("ActionTaken");
            }
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
        public string FirstName
        {
            get
            {
                return _student.FirstName;
            }
            set
            {
                ;
            }
        }
        public string LastName
        {
            get
            {
                return _student.LastName;
            }
            set
            {
                ;
            }
        }
        public string SectionName
        {
            get
            {
                return _section.SectionName;
            }
            set
            {
                ;
            }
        }
        public string GradeLevel
        {
            get
            {
                if (_level.GradeLevel != null)
                {
                    return _level.GradeLevel.Trim();
                }
                return null;
            }
            set
            {
                ;
            }
        }
        public String ImageData
        {
            get
            {
                return _student.ImageData;
            }
            set
            {
                ;
            }
        }
        #endregion

        public AttendanceMonitoringViewModel()
        {
            //Start of Digital Clock
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            //Initiate Finger Scanning
            _attendanceLogList = new AsyncObservableCollection<AttendanceLog>();
            _biometric = new Biometric();
            _relBiometric = new RelBiometric();
            _attendance = new Attendance();

            _biometricWrapper = new BiometricWrapper();
            _relBioWrapper = new RelBiometricWrapper();
            _studentWrapper = new StudentWrapper();
            _levelWrapper = new LevelWrapper();
            _sectionWrapper = new SectionWrapper();
            _attendanceWrapper = new AttendanceWrapper();

            _isMessageDialogOpen = false;
            _isInitial = true;

            DefaultSettings();

            _fptList = _biometricWrapper.RetrieveAll(); //Load all FingerPrintTemplate (fpt);

            Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
            Start();
        }

        public override void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            if (_isMessageDialogOpen == false)
            {
                _isMessageDialogOpen = true;
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OpenDialog("Error", "Finger scanner is disconnected. Make sure it is connected.");
                });
            }
        }
        protected override void Process(Sample Sample)
        {
            //Save current finger print sample for processing and stop capturing incoming fingerprint samples
            base.Process(Sample);
            Stop();

            // Process the sample and create a feature set for the enrollment purpose.
            FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (_isMessageDialogOpen == false)
            {
                if (features != null)
                {
                    MemoryStream fingerprintData = null;
                    Result result = null;
                    // Loop on the FPT List from DB to Compare the feature set with the DB templates
                    int counter = _fptList.Count();
                    foreach (var temp in _fptList)
                    {
                        counter--;
                        Biometric biometric = (Biometric)temp;
                        fingerprintData = new MemoryStream(biometric.FingerPrintTemplate);
                        Template = new Template(fingerprintData);
                        result = new Result();
                        Verificator.Verify(features, Template, ref result);
                        if (result.Verified)
                        {
                            Verificator = new DPFP.Verification.Verification();
                            //Check if finger is already link to a student.
                            _relBiometric = (RelBiometric)_relBioWrapper.RetrieveViaKey(biometric).FirstOrDefault();

                            if (_relBiometric != null)
                            {   //Play a sound
                                _student = (Student)_studentWrapper.RetrieveViaKey(_relBiometric).FirstOrDefault();
                                _level = (Level)_levelWrapper.RetrieveViaKey(_student).FirstOrDefault();
                                _section = (Section)_sectionWrapper.RetrieveViaKey(_student).FirstOrDefault();

                                //Check if student has already logged in for the day.
                                _attendance = (Attendance)_attendanceWrapper.RetrieveViaKey(_student).FirstOrDefault();
                                if (_attendance != null)
                                {
                                    //Check if student already logged out for the day
                                    if (_attendance.TimeOut == null)
                                    {
                                        //Check if Timeout is greater than 1 hour after login
                                        DateTime login = _attendance.TimeIn;
                                        DateTime logout = DateTime.Now;
                                        TimeSpan span = logout.Subtract(login);
                                        Console.WriteLine("Time Difference (seconds): " + span.Seconds);
                                        Console.WriteLine("Time Difference (minutes): " + span.Minutes);
                                        Console.WriteLine("Time Difference (hours): " + span.Hours);
                                        Console.WriteLine("Time Difference (days): " + span.Days);
                                        if (span.Seconds > 30)
                                        {
                                            //Update student logout
                                            _attendance.TimeOut = logout;
                                            _attendanceWrapper.Update(_attendance);
                                            //Add logout to attendance log
                                            //_attendanceLogList.Add(new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged out.", "/SJBCS;component/Resources/Images/Logout-icon.png", "red", Convert.ToDateTime(_attendance.TimeOut)));
                                            _attendanceLogList.Insert(0, new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged out.", "Logout", "red", Convert.ToDateTime(_attendance.TimeOut)));
                                            _statusColor = "Green";
                                            _remarks = "Have a good day!";
                                            RaisePropertyChanged(null);

                                        }
                                        else
                                        {
                                            //Prompt user that it is not allowed to logout 1 hour after login.
                                            _statusColor = "Red";
                                            _remarks = "You're not allowed to logout 1 hour after logging in.";
                                            RaisePropertyChanged("null");
                                        }
                                    }
                                    else
                                    {
                                        _statusColor = "Red";
                                        _remarks = "You've already logged out. You're done for the day.";
                                        RaisePropertyChanged(null);
                                    }

                                }
                                else
                                {
                                    //Create new attendance record for the student.
                                    _attendance = new Attendance(); _attendance.AttendanceID = Guid.NewGuid();
                                    _attendance.StudentID = _student.StudentID;
                                    _attendance.TimeIn = DateTime.Now;
                                    _attendanceWrapper.Add(_attendance);
                                    //_attendanceLogList.Add(new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged in.", "/SJBCS;component/Resources/Images/Login-icon.png", "green", _attendance.TimeIn));
                                    _attendanceLogList.Insert(0, new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged in.", "Login", "Green", Convert.ToDateTime(_attendance.TimeIn)));
                                    _statusColor = "Green";
                                    _remarks = "Have a good day!";
                                    RaisePropertyChanged(null);
                                }
                                break;
                            }
                            else
                            {
                            }
                        }

                        else
                        {
                            Verificator = new DPFP.Verification.Verification();

                            //Play a beep sound.
                            //errorSound.Play();
                            if (counter == 0)
                            {

                                _student = new Student();
                                _level = new Level();
                                _section = new Section();
                                _student.ImageData = "/SJBCS;component/Resources/Images/Error-icon.png";
                                _student.FirstName = "Unauthorized";
                                _student.LastName = "User";
                                _statusColor = "Red";
                                _remarks = "Verification failed. Please try again.";
                                RaisePropertyChanged(null);
                            }
                        }


                    }

                    Start();
                    RaisePropertyChanged(null);
                }
            }
        }
        private void DefaultSettings()
        {
            _remarks = "";
            _statusColor = "Green";
            _student = new Student();
            _level = new Level();
            _section = new Section();
            _student.ImageData = "/SJBCS;component/Resources/Images/default-user-image.png";
            RaisePropertyChanged(null);

        }
        private async void DelayRestoreDefault()
        {
            await Task.Delay(5000);
            DefaultSettings();
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
