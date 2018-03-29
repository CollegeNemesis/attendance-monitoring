using AMS.Utilities;
using DPFP;
using DPFP.Verification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using static DPFP.Verification.Verification;
using SJBCS.Data;
using SJBCS.Services.Repository;
using SJBCS.GUI.SMS;

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
            _attendanceLogs = new ObservableCollection<AttendanceLog>();
            Start();    //Begin capture
        }

        public override void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            ScannerStatus = "Connected";
        }

        public override void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            ScannerStatus = "Disconnected";
        }

        protected override void Process(Sample Sample)
        {
            //Save current finger print sample for processing and stop capturing incoming fingerprint samples
            base.Process(Sample);
            Stop();

            // Process the sample and create a feature set for the enrollment purpose.
            FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            if (features != null)
            {
                MemoryStream fingerprintData = null;
                Result result = null;

                foreach (Biometric biometric in Biometrics) // Loop on the FPT List from DB to Compare the feature set with the DB templates
                {
                    fingerprintData = new MemoryStream(biometric.FingerPrintTemplate);
                    Template = new Template(fingerprintData);
                    result = new Result();
                    Verificator.Verify(features, Template, ref result);

                    if (result.Verified)
                    {
                        Verificator = new DPFP.Verification.Verification();
                        _relBiometric = _relBiometricsRepository.GetRelBiometric(biometric.FingerID);

                        if (_relBiometric != null)  //Check if finger is enrolled.
                        {
                            _student = _studentsRepository.GetStudent(_relBiometric.StudentID);
                            Student = _student;
                            _attendance = _attendancesRepository.GetAttendanceByStudentID(_student.StudentGuid);

                            if (_attendance != null)    //Check if student has already logged in for the day.
                            {
                                if (_attendance.TimeOut == null)    //Check if student already logged out for the day
                                {
                                    DateTime login = _attendance.TimeIn;
                                    DateTime logout = DateTime.Now;
                                    TimeSpan span = logout.Subtract(login);

                                    Console.WriteLine("Time Difference (seconds): " + span.Seconds);
                                    Console.WriteLine("Time Difference (minutes): " + span.Minutes);
                                    Console.WriteLine("Time Difference (hours): " + span.Hours);
                                    Console.WriteLine("Time Difference (days): " + span.Days);

                                    if (span.Seconds > 30) //Check if span between login and logout is greater than allowed threshold
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

                                        _attendancesRepository.UpdateAttendance(_attendance); //Updated attendance record

                                        Application.Current.Dispatcher.Invoke(delegate
                                        {
                                            _attendanceLogs.Add(new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged out.", _attendance.TimeOut)); //Add action to attendance log

                                        });

                                        ProcessSMSIntegration(_attendance.AttendanceID.ToString(), false, (DateTime) _attendance.TimeOut);
                                        Remarks = "Student logged out.";
                                    }
                                    else
                                    {
                                        Remarks = "Student is not allowed to logout 1 hour after logging in.";
                                    }
                                }
                                else
                                {
                                    Remarks = "Student is not allowed to logout twice a day.";
                                }

                            }
                            else
                            {
                                //Create new attendance record for the student.
                                _attendance = new Attendance();
                                _attendance.AttendanceID = Guid.NewGuid();
                                _attendance.StudentID = _student.StudentGuid;
                                _attendance.TimeIn = DateTime.Now;

                                TimeSpan timeIn = _attendance.TimeIn.TimeOfDay;
                                TimeSpan startTime = _student.Section.StartTime;
                                TimeSpan inSpan = startTime.Subtract(timeIn);

                                if (inSpan.Minutes > 1) //Check if student is late
                                {
                                    _attendance.IsLate = true;
                                }
                                else
                                {
                                    _attendance.IsLate = false;
                                }

                                _attendancesRepository.AddAttendance(_attendance); //Add Record

                                Application.Current.Dispatcher.Invoke(delegate
                                {
                                    _attendanceLogs.Add(new AttendanceLog(_student.ImageData, _student.FirstName, _student.LastName, "logged in.", _attendance.TimeIn));
                                });

                                ProcessSMSIntegration(_attendance.AttendanceID.ToString(), true, _attendance.TimeIn);
                                Remarks = "Student logged in.";
                            }

                            IsFingerEnrolled = true;
                            break;
                        }
                        else
                        {
                            //Fingerprint enrolled but not linked to any students!
                        }
                    }
                }

                if (!IsFingerEnrolled)
                {
                    Remarks = "Fingerprint not recognized.";
                    Initialize();
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
            Verificator = new DPFP.Verification.Verification(); // Create a fingerprint template verificator

            #region Repositories
            _studentsRepository = new StudentsRepository();
            _biometricsRepository = new BiometricsRepository();
            _relBiometricsRepository = new RelBiometricsRepository();
            _attendancesRepository = new AttendancesRepository();
            #endregion

            Biometrics = _biometricsRepository.GetBiometrics(); //Load all FingerPrintTemplate (fpt);

            if (Biometrics.Count == 0)
            {
                Remarks = "No fingerprint template available in our records.";
            }

            Student.ImageData = "/SJBCS.GUI;component/Image/default-user-image.png";
        }

        public void SwitchOff()
        {
            Stop();
        }

        public void SwitchOn()
        {
            Initialize();
            Start();
        }

        private void ProcessSMSIntegration(string attendanceID, bool isTimeIn, DateTime time)
        {
            foreach (Contact contact in _student.Contacts)
            {
                string text = String.Format("STJOHNBCS Messaging:\nPlease be informed that {0} {1} St. John the Baptist Catholic School at {2:h:mm tt}.",
                    _student.FirstName, isTimeIn ? "ENTERED" : "EXITED", time);

                SMSUtility.SendSMS(text, contact.ContactNumber, attendanceID, response =>
                {
                    Attendance attendanceObj = _attendancesRepository.GetAttendanceByID(Guid.Parse(response[1]));

                    if ((isTimeIn && !String.IsNullOrEmpty(attendanceObj.TimeInSMSID)) ||
                        (!isTimeIn && !String.IsNullOrEmpty(attendanceObj.TimeOutSMSID)))
                    {
                        return;
                    }

                    if (isTimeIn)
                    {
                        attendanceObj.TimeInSMSID = response[0];
                    }
                    else
                    {
                        attendanceObj.TimeOutSMSID = response[0];
                    }
                    _attendancesRepository.UpdateAttendance(attendanceObj);
                });
            }
        }
    }
}
