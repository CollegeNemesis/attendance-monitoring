﻿using System;
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

        public FingerScanner()
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();				// Create a capture operation.

                if (null != Capturer)
                    Capturer.EventHandler = this;					// Subscribe for capturing events.
                else
                    Console.WriteLine("Can't initiate capture operation! on initial");
            }
            catch
            {
                Console.WriteLine("Can't initiate capture operation!", "Error");
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
                    Console.WriteLine("Using the fingerprint reader, scan your fingerprint.");
                }
                catch
                {
                    Console.WriteLine("Can't initiate capture! on Start()");
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
                    Console.WriteLine("Capture stopped!");
                }
                catch
                {
                    Console.WriteLine("Can't terminate capture!");
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
                Console.WriteLine("The quality of the fingerprint sample is good.");
            }
            else
            {
                Console.WriteLine("The quality of the fingerprint sample is poor.");
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            Console.WriteLine("The fingerprint sample was captured.");
            Console.WriteLine("Scan the same fingerprint again.");
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            Console.WriteLine("The finger was removed from the fingerprint reader.");
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            Console.WriteLine("The fingerprint reader was touched.");
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            Console.WriteLine("The fingerprint reader was connected.");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            Console.WriteLine("The fingerprint reader was disconnected.");
        }
    }
}
