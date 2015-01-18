using System;
using System.Linq;
using Kinect.Gestures;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    public class HandUpGesture : GestureBase
    {
        public event EventHandler<ulong> HandUpChanged;
        public bool HandUp { get; private set; }

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
            var x = Math.Abs(hand.X - elbow.X);
            var y = hand.Y - elbow.Y;
            return x < 0.1 && y > 0; 
        }

        protected virtual void OnHandUpChanged(ulong e)
        {
            var handler = HandUpChanged;
            if (handler != null) handler(this, e);
        }
    }
}