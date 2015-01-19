using System;
using System.Linq;
using Kinect.Gestures;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    public class HandUpGesture : GestureBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HandUpGesture));
       
        public event EventHandler<ulong> HandUpChanged;
        public bool HandUp { get; private set; }

        public HandUpGesture()
        {
            Logger.Info("HandUpGesture initialized");
        }

        protected override void AnalyzeNewBodyData()
        {
            var body = Bodies.FirstOrDefault(b => b.IsTracked);
            if (body == null) return;
            var handIsUpInFrame = DetermineHandUp(body);
            if (HandUp == handIsUpInFrame) return;
            HandUp = handIsUpInFrame;
            OnHandUpChanged(body.TrackingId);
        }

        private bool DetermineHandUp(Body body)
        {
            var hand = body.Joints[JointType.HandRight].Position;
            var elbow = body.Joints[JointType.ElbowRight].Position;
            var shoulder = body.Joints[JointType.ShoulderRight].Position;
            var xDiffs = new[]
            {
                Math.Abs(hand.X - elbow.X),
                Math.Abs(hand.X - shoulder.X),
                Math.Abs(elbow.X - shoulder.X)
            };
            var yDiffs = new[]
            {
                hand.Y - elbow.Y,
                elbow.Y - shoulder.Y
            };
            var xOk = xDiffs.All(xDiff => xDiff < 0.1);
            var yOk = yDiffs.All(yDiff => yDiff > 0);
            Logger.InfoFormat("X: {0} - Y: {1}", xOk, yOk);
            return xOk && yOk;
        }

        protected virtual void OnHandUpChanged(ulong e)
        {
            var handler = HandUpChanged;
            if (handler != null) handler(this, e);
        }
    }
}