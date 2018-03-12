using AMS.Utilities;
using SJBCS.GUI.Dialogs;
using DPFP;
using DPFP.Verification;
using MaterialDesignThemes.Wpf;
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
using SJBCS.Data;
using SJBCS.Services.Repository;

namespace SJBCS.GUI.AMS
{
    public class AttendanceViewModel : FingerScanner
    {

        #region Model Property
        private IStudentsRepository _studentsRepository;
        private IBiometricsRepository _biometricsRepository;
        private IRelBiometricsRepository _relBiometricsRepository;
        private IAttendancesRepository _attendancesRepository;

        private bool IsFingerEnrolled;

        private Attendance _attendance;
        private Biometric _biometric;
        private RelBiometric _relBiometric;

        private Template Template;
        private Verification Verificator;
        private List<Biometric> Biometrics;

        #endregion

        #region View Property

        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }


        private Data.Student _student;

        public Data.Student Student
        {
            get { return _student; }
            set { SetProperty(ref _student, value); }
        }

        private ObservableCollection<AttendanceLog> _attendanceLogs;

        public ObservableCollection<AttendanceLog> AttendanceLogs
        {
            get { return _attendanceLogs; }
            set { SetProperty(ref _attendanceLogs, value); }
        }

        private string _scannerStatus;

        public string ScannerStatus
        {
            get { return _scannerStatus; }
            set { SetProperty(ref _scannerStatus, value); }
        }

        private string _remarks;

        public string Remarks
        {
            get { return _remarks; }
            set { SetProperty(ref _remarks, value); }
        }


        #endregion

        public AttendanceViewModel()
        {

            Initialize();

            Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
            Start();
        }

        public override void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            ScannerStatus = "Scanner connected.";
            Icon = "CheckCircle";
            Status = "Green";
        }

        public override void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            ScannerStatus = "Scanner disconnected.";
            Icon = "CloseCircle";
            Status = "Red";
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
            if (features != null)
            {
                MemoryStream fingerprintData = null;
                Result result = null;

                // Loop on the FPT List from DB to Compare the feature set with the DB templates

                foreach (Biometric biometric in Biometrics)
                {
                    fingerprintData = new MemoryStream(biometric.FingerPrintTemplate);
                    Template = new Template(fingerprintData);
                    result = new Result();
                    Verificator.Verify(features, Template, ref result);

                    if (result.Verified)
                    {
                        Verificator = new DPFP.Verification.Verification();
                        _relBiometric = _relBiometricsRepository.GetRelBiometric(biometric.FingerID);

                        //Check if finger is already link to a student.
                        if (_relBiometric != null)
                        {
                            _student = _studentsRepository.GetStudent(_relBiometric.StudentID);
                            Student = _student;
                            _attendance = _attendancesRepository.GetAttendance(_student.StudentGuid);

                            //Check if student has already logged in for the day.
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

                                        //Check if student is overstay or undertime;
                                        TimeSpan timeOut = _attendance.TimeIn.TimeOfDay;
                                        TimeSpan endTime = _student.Section.EndTime;
                                        TimeSpan outSpan = endTime.Subtract(timeOut);

                                        if (outSpan.Minutes > 0)
                                        {
                                            _attendance.IsOverstay = true;
                                            _attendance.IsEarlyOut = false;
                                        }
                                        else if (outSpan.Minutes < 0)
                                        {
                                            _attendance.IsOverstay = false;
                                            _attendance.IsEarlyOut = true;
                                        }
                                        else
                                        {
                                            _attendance.IsOverstay = false;
                                            _attendance.IsEarlyOut = false;
                                        }

                                        _attendancesRepository.UpdateAttendance(_attendance);
                                        //Add action to attendance log
                                        Application.Current.Dispatcher.Invoke(delegate
                                        {
                                            _attendanceLogs.Add(new AttendanceLog("Logout", "Red", _student.ImageData, _student.FirstName, _student.LastName, "logged out.", _attendance.TimeOut));
                                        });
                                        Remarks = "User logged out.";
                                    }
                                    else
                                    {
                                        //Prompt user that it is not allowed to logout 1 hour after login.
                                        Remarks = "You're not allowed to logout 1 hour after logging in.";
                                    }
                                }
                                else
                                {
                                    //Prompt user that it is not allowed to logout twice in 1 day.
                                    Remarks = "You're not allowed to logout twice a day";
                                }

                            }
                            else
                            {
                                //Create new attendance record for the student.
                                _attendance = new Attendance();
                                _attendance.AttendanceID = Guid.NewGuid();
                                _attendance.StudentID = _student.StudentGuid;
                                _attendance.TimeIn = DateTime.Now;
                                //Check if student is late
                                TimeSpan timeIn = _attendance.TimeIn.TimeOfDay;
                                TimeSpan startTime = _student.Section.StartTime;
                                TimeSpan inSpan = startTime.Subtract(timeIn);

                                if (inSpan.Minutes > 1)
                                {
                                    _attendance.IsLate = true;
                                }
                                else
                                {
                                    _attendance.IsLate = false;
                                }
                                _attendancesRepository.AddAttendance(_attendance);
                                Application.Current.Dispatcher.Invoke(delegate
                                {
                                    _attendanceLogs.Add(new AttendanceLog("Login", "Green", _student.ImageData, _student.FirstName, _student.LastName, "logged in.", _attendance.TimeIn));
                                });
                                Remarks = "User logged in.";
                            }
                            IsFingerEnrolled = true;
                            break;
                        }
                        else
                        {
                            //Fingerprint not link to in Biometrics table but not linked to any students!
                        }
                    }
                }

                if (!IsFingerEnrolled)
                {
                    Verificator = new DPFP.Verification.Verification();

                    //Play an error beep sound.

                    //Prompt for fingerprint not recognized.

                    Student = new Data.Student();
                    Student.ImageData = "/SJBCS.GUI;component/Image/default-user-image.png";
                    Remarks = "Fingerprint not recognized.";
                }

                IsFingerEnrolled = false;
                Start();

            }
        }

        private void Initialize()
        {

            #region Models
            _student = new Data.Student();
            _biometric = new Biometric();
            _relBiometric = new RelBiometric();
            _attendance = new Attendance();
            #endregion

            IsFingerEnrolled = false;
            #region Repositories
            _studentsRepository = new StudentsRepository();
            _biometricsRepository = new BiometricsRepository();
            _relBiometricsRepository = new RelBiometricsRepository();
            _attendancesRepository = new AttendancesRepository();
            #endregion

            Status = "Green";
            _attendanceLogs = new ObservableCollection<AttendanceLog>();
            Biometrics = _biometricsRepository.GetBiometrics(); //Load all FingerPrintTemplate (fpt);
            if (Biometrics.Count == 0)
            {
                Remarks = "No fingerprint template available in our records.";
            }
        }

        public void SwitchOff()
        {
            Stop();
        }

        public void SwitchOn()
        {
            Start();
        }
    }
}
