using System;
using System.Collections.Generic;
using Kinect.Interfaces;
using Microsoft.Kinect;

namespace Kinect.Core
{
    public sealed class KinectManager : IKinectStatus, IGestureDetector, IDisposable
    {
        private readonly KinectSensor _kinectSensor;
        private readonly BodyFrameReader _bodyFrameReader;

        public string Status { get; private set; }
        public int TrackedBodies { get; private set; }
        public event EventHandler<bool> AvailabilityChanged;
        public event EventHandler<string> ErrorOccured;
        public event EventHandler<int> TrackedBodyCountChanged;
        private readonly List<IGesture> _gestures = new List<IGesture>();

        public KinectManager()
        {
            _kinectSensor = KinectSensor.GetDefault();
            if (_kinectSensor == null)
            {
                Status = "Failed to create Kinect instance";
                return;
            }
            OnAvailableChanged(_kinectSensor.IsAvailable);

            _kinectSensor.IsAvailableChanged += _kinectSensor_IsAvailableChanged;
            _kinectSensor.Open();
            _bodyFrameReader = _kinectSensor.BodyFrameSource.OpenReader();
            _bodyFrameReader.FrameArrived += FrameArrived;
        }

        private void _kinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            OnAvailableChanged(e.IsAvailable);
        }

        private void FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            try
            {
                using (var frame = e.FrameReference.AcquireFrame())
                {
                    AnalyzeFrame(frame);    
                }
            }
            catch (Exception ex)
            {
                OnErrorOccured(ex.Message);
            }
        }

        private void AnalyzeFrame(BodyFrame frame)
        {
            if (frame == null) return;
            if (frame.BodyFrameSource == null) return;
            UpdateTrackedBodyCount(frame.BodyFrameSource.BodyCount);
        }

        private void UpdateTrackedBodyCount(int count)
        {
            if (TrackedBodies == count) return;
            TrackedBodies = count;
            OnTrackedBodyCountChanged(count);
        }

        private void OnAvailableChanged(bool isAvailable)
        {
            Status = _kinectSensor.IsAvailable ? "Kinect is available" : "Kinect is not available";
            OnEvent(AvailabilityChanged, isAvailable);
        }

        private void OnErrorOccured(string errorMessage)
        {
            OnEvent(ErrorOccured, errorMessage);
        }

        private void OnTrackedBodyCountChanged(int newTrackedBodyCount)
        {
            OnEvent(TrackedBodyCountChanged, newTrackedBodyCount);
        }

        private void OnEvent<T>(EventHandler<T> handler, T args)
        {
            var invoker = handler;
            if (invoker != null) invoker(this, args);
        }

        public void Dispose()
        {
            if (_bodyFrameReader != null)
            {
                _gestures.ForEach(gesture => _bodyFrameReader.FrameArrived -= gesture.FrameArrived);
                _bodyFrameReader.FrameArrived -= FrameArrived;
                _bodyFrameReader.Dispose();
            }

            _gestures.Clear();

            if (_kinectSensor != null)
            {
                if (_kinectSensor.IsOpen) _kinectSensor.Close();
            }
        }

        public void RegisterGesture(IGesture gesture)
        {
            if (gesture == null) throw new ArgumentNullException("gesture");
            if (_gestures.Contains(gesture)) return;
            _gestures.Add(gesture);
            _bodyFrameReader.FrameArrived += gesture.FrameArrived;
        }

        public void UnRegisterGesture(IGesture gesture)
        {
            if(gesture == null) throw new ArgumentNullException("gesture");
            _bodyFrameReader.FrameArrived -= gesture.FrameArrived;
            _gestures.Remove(gesture);
        }
    }
}
