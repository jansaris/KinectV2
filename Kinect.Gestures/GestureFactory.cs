using System;
using Kinect.Interfaces;

namespace Kinect.Gestures
{
    public enum HandToWatch { HandRight, HandLeft }

    public class GestureFactory : IGestures, IDisposable
    {
        private readonly IGestureDetector _detector;
        private GrabAndThrow _leftHand;
        private GrabAndThrow _rightHand;
        private HandClosed _leftHandClosed;
        private HandClosed _rightHandClosed;
        private HandOpened _leftHandOpened;
        private HandOpened _rightHandOpened;

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

        public event EventHandler<ulong> LeftHandClosed
        {
            add { RegisterLeftHandClosed(value); }
            remove { UnRegisterLeftHandClosed(value); }
        }

        public event EventHandler<ulong> RightHandClosed
        {
            add { RegisterRightHandClosed(value); }
            remove { UnRegisterRightHandClosed(value); }
        }

        public event EventHandler<ulong> LeftHandOpened
        {
            add { RegisterLeftHandOpened(value); }
            remove { UnRegisterLeftHandOpened(value); }
        }

        public event EventHandler<ulong> RightHandOpened
        {
            add { RegisterRightHandOpened(value); }
            remove { UnRegisterRightHandOpened(value); }
        }

        public GestureFactory(IGestureDetector detector)
        {
            _detector = detector;
        }

        private void RegisterRightHandOpened(EventHandler<ulong> handler)
        {
            if (_rightHandOpened == null) _rightHandOpened = InitializeHandOpened(HandToWatch.HandRight);
            _rightHandOpened.Detected += handler;
        }

        private void UnRegisterRightHandOpened(EventHandler<ulong> handler)
        {
            if (_rightHandOpened == null) return;
            _rightHandOpened.Detected -= handler;
        }

        private void RegisterLeftHandOpened(EventHandler<ulong> handler)
        {
            if (_leftHandOpened == null) _leftHandOpened = InitializeHandOpened(HandToWatch.HandLeft);
            _leftHandOpened.Detected += handler;
        }

        private void UnRegisterLeftHandOpened(EventHandler<ulong> handler)
        {
            if (_leftHandOpened == null) return;
            _leftHandOpened.Detected -= handler;
        }

        private void RegisterLeftHandClosed(EventHandler<ulong> handler)
        {
            if (_leftHandClosed == null) _leftHandClosed = InitializeHandClosed(HandToWatch.HandLeft);
            _leftHandClosed.Detected += handler;
        }

        private void UnRegisterLeftHandClosed(EventHandler<ulong> handler)
        {
            if (_leftHandClosed == null) return;
            _leftHandClosed.Detected -= handler;
        }

        private void RegisterRightHandClosed(EventHandler<ulong> handler)
        {
            if (_rightHandClosed == null) _rightHandClosed = InitializeHandClosed(HandToWatch.HandRight);
            _rightHandClosed.Detected += handler;
        }

        private void UnRegisterRightHandClosed(EventHandler<ulong> handler)
        {
            if (_rightHandClosed == null) return;
            _rightHandClosed.Detected -= handler;
        }

        private void RegisterRightHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_rightHand == null) _rightHand = InitializeGrabAndThrow(HandToWatch.HandRight);
            _rightHand.Detected += handler;
        }

        private void UnRegisterRightHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_rightHand == null) return;
            _rightHand.Detected -= handler;
        }

        private void RegisterLeftHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_leftHand == null) _leftHand = InitializeGrabAndThrow(HandToWatch.HandLeft);
            _leftHand.Detected += handler;
        }

        private void UnRegisterLeftHandGrabAndThrow(EventHandler<ulong> handler)
        {
            if (_leftHand == null) return;
            _leftHand.Detected -= handler;
        }

        private GrabAndThrow InitializeGrabAndThrow(HandToWatch hand)
        {
            var gesture = new GrabAndThrow(hand);
            _detector.RegisterGesture(gesture);
            return gesture;
        }

        private HandClosed InitializeHandClosed(HandToWatch hand)
        {
            var gesture = new HandClosed(hand);
            _detector.RegisterGesture(gesture);
            return gesture;
        }

        private HandOpened InitializeHandOpened(HandToWatch hand)
        {
            var gesture = new HandOpened(hand);
            _detector.RegisterGesture(gesture);
            return gesture;
        }

        public void Dispose()
        {
            if(_leftHand != null) _detector.UnRegisterGesture(_leftHand);
            if (_rightHand != null) _detector.UnRegisterGesture(_rightHand);
            if(_leftHandClosed != null) _detector.UnRegisterGesture(_leftHandClosed);
            if (_rightHandClosed != null) _detector.UnRegisterGesture(_rightHandClosed);
        }
    }
}