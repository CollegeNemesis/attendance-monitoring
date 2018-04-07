using DPFP;
using DPFP.Verification;
using SJBCS.Data;
using SJBCS.GUI.Utilities;
using SJBCS.Services.Repository;
using System;
using System.Collections.ObjectModel;
using System.IO;
using static DPFP.Verification.Verification;

namespace SJBCS.GUI.Student
{
    public class EnrollBiometricsViewModel : FingerScanner
    {
        delegate void Function();

        private bool IsInitial;

        private IBiometricsRepository _biometricsRepository;
        private IRelBiometricsRepository _relBiometricsRepository;

        private DPFP.Template Template;
        private DPFP.Processing.Enrollment Enroller;
        private Verification Verificator;

        private bool IsFingerEnrolled;

        private bool isDone;

        public bool IsDone
        {
            get { return isDone; }
            set { SetProperty(ref isDone, value); }
        }


        private Biometric _biometric;

        public Biometric Biometric
        {
            get { return _biometric; }
            set { SetProperty(ref _biometric, value); }
        }

        private ObservableCollection<Biometric> _biometrics;

        public ObservableCollection<Biometric> Biometrics
        {
            get { return _biometrics; }
            set { SetProperty(ref _biometrics, value); }
        }

        private int _completion;

        public int Completion
        {
            get { return _completion; }
            set { SetProperty(ref _completion, value); }
        }

        private string _notification;

        public string Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, value); }
        }

        private string _instruction;

        public string Instruction
        {
            get { return _instruction; }
            set { SetProperty(ref _instruction, value); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public EnrollBiometricsViewModel(IRelBiometricsRepository relBiometricsRepository, IBiometricsRepository biometricsRepository)
        {
            _biometricsRepository = biometricsRepository;
            _relBiometricsRepository = relBiometricsRepository;
        }

        private void Initialize()
        {
            IsFingerEnrolled = false;
            IsInitial = true;
            IsDone = false;

            Biometrics = new ObservableCollection<Biometric>(_biometricsRepository.GetBiometrics());
            Biometric = new Biometric();

            Status = "Let\'s start";
            Notification = "Put your finger on the sensor and lift after you see next instruction.";
            Instruction = "";

            Completion = 0;

            Enroller = new DPFP.Processing.Enrollment();    // Create an enrollment.

        }

        protected override void Process(DPFP.Sample Sample)
        {
            base.Process(Sample);


            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

            // Check quality of the sample and add to enroller if it's good
            if (features != null)
            {
                MemoryStream fingerprintData = null;
                Result result = null;

                //Check if finger is already enrolled
                foreach (Biometric biometric in Biometrics)
                {
                    features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

                    fingerprintData = new MemoryStream(biometric.FingerPrintTemplate);
                    Template = new Template(fingerprintData);
                    result = new Result();

                    Verificator = new DPFP.Verification.Verification();
                    Verificator.Verify(features, Template, ref result);

                    if (result.Verified)
                    {
                        Verificator = new DPFP.Verification.Verification();
                        IsFingerEnrolled = true;
                        break;
                    }
                }

                //Check user try to enroll duplicate finger at one transaction.

                if (!IsFingerEnrolled)
                {
                    Verificator = new DPFP.Verification.Verification();

                    features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

                    try
                    {
                        Enroller.AddFeatures(features);     // Add feature set to template.
                        Completion += 25;

                        Status = "Great. Now repeat.";
                        Instruction = "Move your finger slightly to add all different parts of your fingerprint.";
                        Notification = "Lift finger, then touch the sensor again. Completion - " + Completion + "%.";
                    }
                    finally
                    {
                        //UpdateStatus();

                        // Check if template has been created.
                        switch (Enroller.TemplateStatus)
                        {
                            case DPFP.Processing.Enrollment.Status.Ready:   // report success and stop capturing
                                OnTemplate(Enroller.Template);
                                Stop();
                                Status = "Fingerprint enrolled.";
                                Instruction = "Fingerprint enrollment completed succesfully. You may close this window.";
                                IsDone = true;
                                break;

                            case DPFP.Processing.Enrollment.Status.Failed:  // report failure and restart capturing
                                Enroller.Clear();
                                Stop();
                                OnTemplate(null);

                                Status = "Fingerprint not enrolled.";
                                Instruction = "Fingerprint enrollment not completed succesfully. Please try again.";

                                Notification = "Put your finger on the sensor again and lift after you see next instruction.";
                                Instruction = "";
                                Completion = 0;

                                Start();
                                break;
                        }
                    }
                }
                else
                {

                    SwitchOff();
                    Status = "Fingerprint has been enrolled, try again.";
                    Start();
                }
            }

        }

        private void OnTemplate(DPFP.Template template)
        {
            Template = template;
            if (Template != null)
            {
                MemoryStream fingerprintData = new MemoryStream();
                Template.Serialize(fingerprintData);
                fingerprintData.Position = 0;
                BinaryReader br = new BinaryReader(fingerprintData);
                Byte[] bytes = br.ReadBytes((Int32)fingerprintData.Length);

                Biometric.FingerID = Guid.NewGuid();
                Biometric.FingerPrintTemplate = bytes;

                Stop();

            }
            else
            {
                //Faile case, repeat.
            }
        }

        public override void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            if (!IsInitial)
            {
                Status = "Scanner reconnected.";
                Notification = "Put your finger on the sensor and lift after you see next instruction.";
                Instruction = "";
            }
        }

        public override void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            Status = "Scanner disconnected. Unable to capture finger.";
            Notification = "Reconnect the scanner to resume finger enrollment.";
            Instruction = "";
            IsInitial = false;

        }

        public void SwitchOff()
        {
            Stop();
            Initialize();
        }

        public void SwitchOn()
        {
            Initialize();
            Start();
        }
    }
}

