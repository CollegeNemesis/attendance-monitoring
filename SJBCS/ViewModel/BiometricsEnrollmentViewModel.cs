using DPFP;
using DPFP.Capture;
using DPFP.Processing;
using MaterialDesignThemes.Wpf;
using SJBCS.Model;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.ViewModel
{
    class BiometricsEnrollmentViewModel : FingerScanner, INotifyPropertyChanged
    {
        private string _studentID;
        delegate void Function();

        private int _indicator;
        private String _instruction;
        private String _status;
        private Boolean _flag;
        private String _visibility;
        private DPFP.Template Template;
        private DPFP.Processing.Enrollment Enroller;
        private Biometric _bio;
        private RelBiometric _relBio;
        private static AMSEntities DBContext = new AMSEntities();
        private BiometricWrapper _biometricWrapper;
        private RelBiometricWrapper _relBiometricWrapper;

        public event PropertyChangedEventHandler PropertyChanged;
        public int Completion
        {
            get
            {
                return _indicator;
            }
            set
            {
                _indicator = value;
            }
        }
        public String Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public String Instruction
        {
            get
            {
                return _instruction;
            }
            set
            {
                _instruction = value;
            }
        }
        public String Visibility
        {
            get
            {
                return _visibility;
            }
        }
        public Boolean Flag
        {
            get
            {
                return _flag;
            }
        }

        public BiometricsEnrollmentViewModel(string studentID)
        {
            _studentID = studentID;
            _indicator = 0;
            _instruction = "Let's start.";
            _status = "Put your finger on the sensor and lift after notification appear.";
            _bio = new Biometric();
            _relBio = new RelBiometric();
            _biometricWrapper = new BiometricWrapper();
            _relBiometricWrapper = new RelBiometricWrapper();
            _visibility = "Hidden";
            Start();
            Enroller = new DPFP.Processing.Enrollment();            // Create an enrollment.

        }


        protected override void Process(DPFP.Sample Sample)
        {
            base.Process(Sample);

            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

            // Check quality of the sample and add to enroller if it's good
            if (features != null) try
                {
                    Enroller.AddFeatures(features);     // Add feature set to template.
                    _indicator += 25;
                    if (_indicator > 25)
                    {

                        _instruction = "Great. Now repeat.";
                        _status = "Move your finger slightly to add all different parts of your fingerprint.";
                        _flag = true;
                    }
                    else
                    {
                        _instruction = "Great. Now repeat.";
                        _status = "Put your finger on the sensor and lift after notification appear.";
                        _flag = true;
                    }
                    RaisePropertyChanged(null);
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
                            Default();
                            Stop();
                            RaisePropertyChanged(null);
                            OnTemplate(null);
                            Start();
                            break;
                    }
                }
        }

        private void OnTemplate(DPFP.Template template)
        {
            Template = template;
            if (Template != null)
            {
                _instruction = "Fingerprint added.";
                _status = "You can now close this window.";
                _flag = true;
                MemoryStream fingerprintData = new MemoryStream();
                Template.Serialize(fingerprintData);
                fingerprintData.Position = 0;
                BinaryReader br = new BinaryReader(fingerprintData);
                Byte[] bytes = br.ReadBytes((Int32)fingerprintData.Length);
                _bio.FingerID = Guid.NewGuid();
                _bio.FingerPrintTemplate = bytes;
                _biometricWrapper.Add(DBContext, _bio);
                _relBio.FingerID = _bio.FingerID;
                _relBio.RelBiometricID = Guid.NewGuid();
                _relBio.StudentID = _studentID;
                _relBiometricWrapper.Add(DBContext, _relBio);
                _visibility = "Visible";
                RaisePropertyChanged(null);
            }
            else
            {
                _instruction = "Fingerprint enrollment failed.";
                _status = "The fingerprint template is not valid. Repeat fingerprint enrollment.";
                _flag = true;
                RaisePropertyChanged(null);
            }
        }

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
        private void Default()
        {
            _indicator = 0;
            _instruction = "Let's start.";
            _status = "Put your finger on the sensor and lift after notification appear.";
            _bio = new Biometric();
            _relBio = new RelBiometric();
            _biometricWrapper = new BiometricWrapper();
            _relBiometricWrapper = new RelBiometricWrapper();
            _visibility = "Hidden";
        }
    }
}
