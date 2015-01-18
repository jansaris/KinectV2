using System;
using System.Diagnostics;
using System.Linq;
using Kinect.Gestures;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    internal class HandTracker : GestureBase
    {
        public float KinectX { get; private set; }
        public float KinectY { get; private set; }
        public float ScreenX { get; private set; }
        public float ScreenY { get; private set; }
        private const float MaxValue = 736;

        protected override void AnalyzeNewBodyData()
        {
            try
            {
                var body = Bodies.FirstOrDefault(b => b.IsTracked);
                if (body == null) return;
                var hand = body.Joints[JointType.HandRight].Position;
                SetValues(hand);
                InvokeDetected(body.TrackingId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to read body data", ex.Message);
            }
        }

        private void SetValues(CameraSpacePoint hand)
        {
            KinectX = hand.X;
            KinectY = hand.Y;
            ScreenX = (MaxValue / 2) + (KinectX * MaxValue);
            ScreenY = (MaxValue / 2) - (KinectY * MaxValue);
        }
    }
}
