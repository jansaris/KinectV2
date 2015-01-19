using System.Linq;
using Kinect.Gestures;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    internal class HandTracker : GestureBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HandTracker));

        public HandTracker()
        {
            Logger.Info("Handtracker initialized");
        }

        public float KinectX { get; private set; }
        public float KinectY { get; private set; }
        public float ScreenX { get; private set; }
        public float ScreenY { get; private set; }
        private const float MaxValue = 736;

        protected override void AnalyzeNewBodyData()
        {
            var body = Bodies.FirstOrDefault(b => b.IsTracked);
            if (body == null) return;
            var hand = body.Joints[JointType.HandRight].Position;
            SetValues(hand);
            InvokeDetected(body.TrackingId);
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
