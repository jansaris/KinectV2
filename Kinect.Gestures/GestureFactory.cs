using System;
using Kinect.Interfaces;

namespace Kinect.Gestures
{
    public class GestureFactory : IGestures, IDisposable
    {
        private readonly IGestureDetector _detector;
        private GrabAndThrow _leftHand;
        private GrabAndThrow _rightHand;

        public event EventHandler<ulong> GrabAndThrowRightHanded
        {
            add{ RegisterRightHandGrabAndThrow(value); }
            remove {UnRegisterRightHandGrabAndThrow(value);}
        }
        public event EventHandler<ulong> GrabAndThrowLeftHanded
        {
            add{ RegisterLeftHandGrabAndThrow(value); }
            remove {UnRegisterLeftHandGrabAndThrow(value);}
        }

        public GestureFactory(IGestureDetector detector)
        {
            _detector = detector;
        }

        private void RegisterRightHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_rightHand == null) _rightHand = InitializeGrabAndThrow(GrabAndThrow.HandToWatch.HandRight);
            _rightHand.Detected += handler;
        }

        private void UnRegisterRightHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_rightHand == null) return;
            _rightHand.Detected -= handler;
        }

        private void RegisterLeftHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_leftHand == null) _leftHand = InitializeGrabAndThrow(GrabAndThrow.HandToWatch.HandLeft);
            _leftHand.Detected += handler;
        }

        private void UnRegisterLeftHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_leftHand == null) return;
            _leftHand.Detected -= handler;
        }

        private GrabAndThrow InitializeGrabAndThrow(GrabAndThrow.HandToWatch hand)
        {
            var gesture = new GrabAndThrow(hand);
            _detector.RegisterGesture(gesture);
            return gesture;
        }

        public void Dispose()
        {
            if(_leftHand != null) _detector.UnRegisterGesture(_leftHand);
            if (_rightHand != null) _detector.UnRegisterGesture(_rightHand);
        }
    }
}