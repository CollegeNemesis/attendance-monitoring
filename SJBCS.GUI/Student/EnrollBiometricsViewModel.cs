using DPFP;
using System;
using System.ComponentModel;
using System.IO;
using SJBCS.Data;
using AMS.Utilities;
using SJBCS.Services.Repository;
using System.Collections.ObjectModel;
using static DPFP.Verification.Verification;
using DPFP.Verification;

namespace SJBCS.GUI.Student
{
    public class EnrollBiometricsViewModel : FingerScanner
    {
        delegate void Function();

        private int counter;
        private IBiometricsRepository _biometricsRepository;
        private IRelBiometricsRepository _relBiometricsRepository;


        private DPFP.Template Template;
        private DPFP.Processing.Enrollment Enroller;
        private Verification Verificator;

        private bool IsFingerEnrolled;

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

        private string _scanner;

        public string Scanner
        {
            get { return _scanner; }
            set { SetProperty(ref _scanner, value); }
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

            Biometrics = new ObservableCollection<Biometric>(_biometricsRepository.GetBiometrics());
            Biometric = new Biometric();
            IsFingerEnrolled = false;

            counter = 0;

            Start();
            Enroller = new DPFP.Processing.Enrollment();    // Create an enrollment.
        }


        protected override void Process(DPFP.Sample Sample)
        {
            base.Process(Sample);


            Status = "Scanning " + ++counter;
            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

            // Check quality of the sample and add to enroller if it's good

            if (features != null)
            {
                MemoryStream fingerprintData = null;
                Result result = null;

                //Check first if finger is already enrolled
                foreach (Biometric biometric in Biometrics)
                {
                    fingerprintData = new MemoryStream(biometric.FingerPrintTemplate);
                    Template = new Template(fingerprintData);
                    result = new Result();
                    Verificator.Verify(features, Template, ref result);
                    if (result.Verified)
                    {
                        Verificator = new DPFP.Verification.Verification();
                        IsFingerEnrolled = true;
                        break;
                    }
                }
                if (!IsFingerEnrolled)
                {
                    Verificator = new DPFP.Verification.Verification();

                    try
                    {
                        Enroller.AddFeatures(features);     // Add feature set to template.
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
                                break;

                            case DPFP.Processing.Enrollment.Status.Failed:  // report failure and restart capturing
                                Enroller.Clear();
                                Stop();
                                OnTemplate(null);
                                Start();
                                break;
                        }
                    }
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

                Status = "Done";

            }
            else
            {
                //Faile case, repeat.
            }
        }

        public override void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            Scanner = "Scanner connected.";
            
        }

        public override void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            Scanner = "Scanner disconnected.";
        }
    }
}

