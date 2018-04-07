using SJBCS.GUI.Dialogs;
using System;
using System.Windows;

namespace SJBCS.GUI.Utilities
{
    public class FingerScanner : BindableBase, DPFP.Capture.EventHandler
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected DPFP.Capture.Capture Capturer;

        public FingerScanner()
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();              // Create a capture operation.

                if (null != Capturer)
                    Capturer.EventHandler = this;                   // Subscribe for capturing events.
            }
            catch(Exception error)
            {
                LogError("Fatal error with the SDK. Please restart the application.");
                Logger.Error(error);
                System.Windows.Application.Current.Shutdown();
            }
        }

        protected virtual void Process(DPFP.Sample Sample)
        {

        }

        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                }
                catch (Exception error)
                {
                    LogError("Fatal error with the SDK. Please restart the application.");
                    Logger.Error(error);
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch (Exception error)
                {
                    LogError("Fatal error with the SDK. Please restart the application.");
                    Logger.Error(error);
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }

        private async void LogError(string error)
        {
            await DialogHelper.ShowDialog(DialogType.Error, error); 
        }

        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
            {
            }
            else
            {
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
        }

        public virtual void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
        }

        public virtual void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
        }
    }
}
