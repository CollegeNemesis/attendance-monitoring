using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPFP;
using DPFP.Capture;
using DPFP.Processing;

namespace SJBCS.Model
{
    class FingerScanner : DPFP.Capture.EventHandler
    {

        private DPFP.Capture.Capture Capturer;
        protected String _actionTaken;

        public FingerScanner()
        {
            _actionTaken = "";
            try
            {
                Capturer = new DPFP.Capture.Capture();				// Create a capture operation.

                if (null != Capturer)
                    Capturer.EventHandler = this;					// Subscribe for capturing events.
                else
                    _actionTaken = "Can't initiate capture operation! on initial" + "\n" + _actionTaken;
            }
            catch
            {
                _actionTaken = "Can't initiate capture operation!" + "\n" + _actionTaken;
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
                    _actionTaken = "Using the fingerprint reader, scan your fingerprint." + "\n" + _actionTaken;
                }
                catch
                {
                    _actionTaken = "Can't initiate capture! on Start()" + "\n" + _actionTaken;
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
                    _actionTaken = "Capture stopped!" + "\n" + _actionTaken;
                }
                catch
                {
                    _actionTaken = "Can't terminate capture!" + "\n" + _actionTaken;
                }
            }
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
            if (CaptureFeedback == CaptureFeedback.Good)
            {
                _actionTaken = "The quality of the fingerprint sample is good." + "\n" + _actionTaken;
            }
            else
            {
                _actionTaken = "The quality of the fingerprint sample is poor." + "\n" + _actionTaken;
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            _actionTaken = "The fingerprint sample was captured." + "\n" + _actionTaken;
            _actionTaken = "Scan the same fingerprint again." + "\n" + _actionTaken;
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            _actionTaken = "The finger was removed from the fingerprint reader." + "\n" + _actionTaken;
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            _actionTaken = "The fingerprint reader was touched." + "\n" + _actionTaken;
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            _actionTaken = "The fingerprint reader was connected." + "\n" + _actionTaken;
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            _actionTaken = "The fingerprint reader was disconnected." + "\n" + _actionTaken;
        }
    }
}
