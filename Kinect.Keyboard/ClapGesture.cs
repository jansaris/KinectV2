using System;
using System.Linq;
using Kinect.Gestures;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    public class ClapGesture : GestureBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ClapGesture));
        private enum State { Open, Closed }

        private const int MaxFrames = 6;
        private const float MaxDistance = 0.05f;

        private int _counter = MaxFrames;
        private State _previousStateFrame = State.Open;

        public ClapGesture()
        {
            Logger.Info("ClapGesture initialized");
        }

        protected override void AnalyzeNewBodyData()
        {
            var body = Bodies.FirstOrDefault(b => b.IsTracked);
            if (body == null) return;
            var clapFinished = IsClapFinished(body);
            if (clapFinished)
            {
                InvokeDetected(body.TrackingId);
            }
        }

        private bool IsClapFinished(Body body)
        {
            var closed = IsClosed(body);
            if (closed)
            {
                _counter--;
            }
            ResetCounter();
            
            var clapFinished = !closed && _counter < MaxFrames && _counter >= 0;
            var newState = closed ? State.Closed : State.Open;
            Logger.DebugFormat("Prev: {0}; New: {1}; Count: {2}, Finish: {3}", _previousStateFrame, newState, _counter, clapFinished);

            _previousStateFrame = newState;
            ResetCounter();
            
            return clapFinished;
        }

        private void ResetCounter()
        {
            if (_previousStateFrame == State.Open)
            {
                _counter = MaxFrames;
            }
        }

        private static bool IsClosed(Body body)
        {
            var left = body.Joints[JointType.HandLeft].Position;
            var right = body.Joints[JointType.HandRight].Position;
            return Math.Abs(left.X - right.X) < MaxDistance && Math.Abs(left.Y - right.Y) < MaxDistance;
        }
    }
}