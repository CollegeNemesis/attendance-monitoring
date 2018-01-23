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

    class EnrollStudentBiometricsViewModel : FingerScanner, INotifyPropertyChanged
    {
        delegate void Function();
        
        private int _indicator;
        private DPFP.Template Template;
        private DPFP.Processing.Enrollment Enroller;
        private Biometric _bio;
        private static AMSEntities DBContext = new AMSEntities();
        private BiometricWrapper _biometricWrapper;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public EnrollStudentBiometricsViewModel()
        {

            
            _indicator = 0;
            _bio = new Biometric();
            _biometricWrapper = new BiometricWrapper();
            Start();
            Enroller = new DPFP.Processing.Enrollment();            // Create an enrollment.

        }

        public int Indicator
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
                            RaisePropertyChanged(null);
                            Stop();
                            break;

                        case DPFP.Processing.Enrollment.Status.Failed:  // report failure and restart capturing
                            Enroller.Clear();
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
                Console.WriteLine("The fingerprint template is ready for fingerprint verification.");
                MemoryStream fingerprintData = new MemoryStream();
                Template.Serialize(fingerprintData);
                fingerprintData.Position = 0;
                BinaryReader br = new BinaryReader(fingerprintData);
                Byte[] bytes = br.ReadBytes((Int32)fingerprintData.Length);
                _bio.FingerID = Guid.NewGuid();
                _bio.FingerPrintTemplate = bytes;
                _biometricWrapper.Add(DBContext,_bio);
            }
            else
                Console.WriteLine("The fingerprint template is not valid. Repeat fingerprint enrollment.");
            RaisePropertyChanged(null);
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
