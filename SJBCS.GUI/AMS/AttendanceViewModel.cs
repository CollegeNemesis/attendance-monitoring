using AMS.Utilities;
using DPFP;
using DPFP.Verification;

namespace SJBCS.GUI.AMS
{
    public class AttendanceViewModel : FingerScanner
    {
        private DPFP.Template Template;
        private DPFP.Verification.Verification Verificator;

        public void Verify(DPFP.Template template)
        {
            Template = template;
        }
        public AttendanceViewModel()
        {
            Verificator = new DPFP.Verification.Verification();		// Create a fingerprint template verificator
            Start();
        }

        protected override void Process(DPFP.Sample Sample)
        {
            base.Process(Sample);
            
            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (features != null)
            {
                // Compare the feature set with our template
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                Verificator.Verify(features, Template, ref result);

                if (result.Verified)
                {
                    
                }
                else
                {

                }
            }
        }
    }
}
