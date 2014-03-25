using System;
using System.Diagnostics;
using System.Linq;
using Kinect.Gestures;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    internal class HandTracker : GestureBase
    {
        public CameraSpacePoint? HandLeft { get; private set; }
        public CameraSpacePoint? HandRight { get; private set; }

        public HandTracker()
        {
            HandLeft = null;
            HandRight = null;
        }

        protected override void AnalyzeNewBodyData()
        {
            try
            {
                var body = Bodies.FirstOrDefault(b => b.IsTracked);
                if (body == null) return;
                HandLeft = body.Joints[JointType.HandLeft].Position;
                HandRight = body.Joints[JointType.HandRight].Position;
                InvokeDetected(body.TrackingId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to read body data", ex.Message);
            }
        }
    }
}
