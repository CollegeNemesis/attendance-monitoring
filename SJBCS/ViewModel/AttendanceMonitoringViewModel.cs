using DPFP;
using DPFP.Verification;
using SJBCS.Model;
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
using System.Windows.Threading;
using static DPFP.Verification.Verification;

namespace SJBCS.ViewModel
{
    class AttendanceMonitoringViewModel : FingerScanner, INotifyPropertyChanged
    {
        private string _digitalClock;
        private string _digitalCalendar;
        private Student _student;
        private Level _level;
        private Section _section;
        private Attendance _attendance;
        private StudentWrapper _studentWrapper;
        private LevelWrapper _levelWrapper;
        private SectionWrapper _sectionWrapper;
        private AttendanceWrapper _attendanceWrapper;
        private Template Template;
        private Verification Verificator;
        private Biometric _biometric;
        private BiometricWrapper _bioWrapper;
        private RelBiometric _relBiometric;
        private RelBiometricWrapper _relBioWrapper;
        private ObservableCollection<Object> _fptList;
        private ObservableCollection<AttendanceLog> _attendanceLogList;
        private static AMSEntities DBContext = new AMSEntities();

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
        }
        public string LastName
        {
            get
            {
                return _student.LastName;
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

        public AttendanceMonitoringViewModel()
        {
            //Start of Digital Clock
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            //End of Digital Clock

            //Initiate Finger Scanning
            _attendanceLogList = new AsyncObservableCollection<AttendanceLog>();
            _biometric = new Biometric();
            _bioWrapper = new BiometricWrapper();
            _relBiometric = new RelBiometric();
            _relBioWrapper = new RelBiometricWrapper();

            _student = new Student();
            _level = new Level();
            _section = new Section();
            _attendance = new Attendance();

            _studentWrapper = new StudentWrapper();
            _levelWrapper = new LevelWrapper();
            _sectionWrapper = new SectionWrapper();
            _attendanceWrapper = new AttendanceWrapper();

            _fptList = _bioWrapper.RetrieveAll(DBContext, _biometric); //Load all FingerPrintTemplate (fpt);

            Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
            Start();
        }

        protected override void Process(Sample Sample)
        {
            base.Process(Sample);
            Stop();

            // Process the sample and create a feature set for the enrollment purpose.
            FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (features != null)
            {
                MemoryStream fingerprintData = null;
                Result result = null;
                // Loop on the FPT List from DB to Compare the feature set with the DB templates
                int counter = 0;
                foreach (var temp in _fptList)
                {
                    counter++;
                    Biometric biometric = (Biometric)temp;
                    Console.WriteLine(biometric.FingerPrintTemplate);
                    fingerprintData = new MemoryStream(biometric.FingerPrintTemplate);
                    Template = new Template(fingerprintData);
                    result = new Result();
                    Verificator.Verify(features, Template, ref result);

                    if (result.Verified)
                    {
                        Verificator = new DPFP.Verification.Verification();
                        _relBiometric = (RelBiometric)_relBioWrapper.RetrieveAll(DBContext, biometric).FirstOrDefault();
                        if (_relBiometric != null)
                        {
                            _actionTaken = "VERIFIED - " + _relBiometric.StudentID + " - " + DateTime.Now.ToString("h:mm:ss tt") + "\n" + _actionTaken;

                            //Play a beep sound.

                            _student = (Student)_studentWrapper.RetrieveViaKeyword(DBContext, _student, _relBiometric.StudentID).FirstOrDefault();
                            _level = (Level)_levelWrapper.RetrieveViaKeyword(DBContext, _level, _student.LevelID.ToString()).FirstOrDefault();
                            _section = (Section)_sectionWrapper.RetrieveViaStudent(DBContext, _student, _student.SectionID.ToString()).FirstOrDefault();

                            //Check if Student has already logged in for the day
                            _attendance = (Attendance)_attendanceWrapper.RetrieveViaStudent(DBContext, _student).FirstOrDefault();
                            if (_attendance != null)
                            {
                                //Check if Timeout is greater than 1 hour after login
                                DateTime login = _attendance.TimeIn;
                                DateTime logout = DateTime.Now;
                                TimeSpan span = logout.Subtract(login);
                                Console.WriteLine("Time Difference (seconds): " + span.Seconds);
                                Console.WriteLine("Time Difference (minutes): " + span.Minutes);
                                Console.WriteLine("Time Difference (hours): " + span.Hours);
                                Console.WriteLine("Time Difference (days): " + span.Days);
                                if (span.Minutes > 2)
                                {
                                    _attendance.TimeOut = logout;
                                    DBContext.SaveChanges();
                                    //_attendanceLogList.Add(new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged out.", "/SJBCS;component/Resources/Images/Logout-icon.png", "red", Convert.ToDateTime(_attendance.TimeOut)));
                                    _attendanceLogList.Insert(0, new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged out.", "/SJBCS;component/Resources/Images/Logout-icon.png", "red", Convert.ToDateTime(_attendance.TimeOut)));
                                    RaisePropertyChanged("AttendanceLogList");

                                }
                            }
                            else
                            {
                                _attendance = new Attendance();
                                _student = (Student)_studentWrapper.RetrieveViaKeyword(DBContext, _student, _relBiometric.StudentID).FirstOrDefault();
                                _attendance.AttendanceID = Guid.NewGuid();
                                _attendance.StudentID = _student.StudentID;
                                _attendance.TimeIn = DateTime.Now;
                                _attendanceWrapper.Add(DBContext, _attendance);
                                //_attendanceLogList.Add(new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged in.", "/SJBCS;component/Resources/Images/Login-icon.png", "green", _attendance.TimeIn));
                                _attendanceLogList.Insert(0, new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged in.", "/SJBCS;component/Resources/Images/Login-icon.png", "Red", Convert.ToDateTime(_attendance.TimeOut)));
                                RaisePropertyChanged("AttendanceLogList");
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

                        _student = new Student();
                        _level = new Level();
                        _section = new Section();

                        _actionTaken = "NOT VERIFIED." + DateTime.Now.ToString("h:mm:ss tt") + "\n" + _actionTaken;
                        //Play a beep sound.
                        //errorSound.Play();

                        _student.ImageData = "/SJBCS;component/Resources/Images/Stop-icon.png";
                        _student.FirstName = "Unauthorized";
                        _student.LastName = "User";
                        _level.GradeLevel = "-";
                        _section.SectionName = "-";
                    }
                }

                Start();
                RaisePropertyChanged(null);
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
