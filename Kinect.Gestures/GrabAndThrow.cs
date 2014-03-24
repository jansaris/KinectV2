using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Gestures
{
    public class GrabAndThrow : GestureBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GrabAndThrow));

        private enum State { Unknown, ArmInFront, HandClosed, HandRight };

        private readonly JointType _handToWatch;
        private readonly Dictionary<ulong, Tuple<State,int>> _currentState = new Dictionary<ulong, Tuple<State,int>>(); 

        public GrabAndThrow(HandToWatch handToWatch)
        {
            _handToWatch = handToWatch == HandToWatch.HandLeft ? JointType.HandLeft : JointType.HandRight;
        }

        protected override void AnalyzeNewBodyData()
        {
            foreach (var body in Bodies.Where(b => b.IsTracked))
            {
                var state = GetCurrentState(body.TrackingId);
                switch (state)
                {
                    case State.Unknown:
                        if(IsArmInFront(body))
                            SetCurrentState(body.TrackingId, State.ArmInFront);
                        break;
                    case State.ArmInFront:
                        if (IsHandClosed(body)) 
                            SetCurrentState(body.TrackingId, State.HandClosed, 15);
                        break;
                    case State.HandClosed:
                        if (IsHandAwayFromBody(body))
                            SetCurrentState(body.TrackingId, State.HandRight);
                        break;
                    case State.HandRight:
                        InvokeDetected(body.TrackingId);
                        SetCurrentState(body.TrackingId, State.Unknown, 0);
                        break;
                }
                DecreaseFrameTicker(body.TrackingId);
            }
        }

        private bool IsArmInFront(Body body)
        {
            var hand = body.Joints[_handToWatch].Position;
            var spine = body.Joints[JointType.SpineMid].Position;
            var diff = spine.Z - hand.Z;
            Logger.DebugFormat("IsArmInFront: Diff {0}, Hand {1}, Spine {2}", diff, hand.ToReadableString(), spine.ToReadableString());
            return diff > 0.4;
        }

        private bool IsHandClosed(Body body)
        {
            var handState = _handToWatch == JointType.HandLeft ? body.HandLeftState : body.HandRightState;
            var handConfidence = _handToWatch == JointType.HandLeft ? body.HandLeftConfidence : body.HandRightConfidence;

            Logger.DebugFormat("IsHandClosed: Hand {0}, Confidence {1}", handState, handConfidence);
            return handState == HandState.Closed && handConfidence == TrackingConfidence.High;    
        }

        private bool IsHandAwayFromBody(Body body)
        {
            var hand = body.Joints[_handToWatch].Position;
            var spine = body.Joints[JointType.SpineMid].Position;
            var diff = Math.Abs(hand.X - spine.X);
            Logger.DebugFormat("IsHandAwayFromBody: Diff {0}, Hand {1}, Spine {2}",diff, hand.ToReadableString(), spine.ToReadableString());
            return diff > 0.4;
        }

        private void SetCurrentState(ulong trackingId, State newState, int frameTickerCount = 10)
        {
            Logger.InfoFormat("Switching state from '{0}' to '{1}' for user '{2}'",_currentState[trackingId], newState, trackingId);
            _currentState[trackingId] = new Tuple<State, int>(newState, frameTickerCount);
        }

        private State GetCurrentState(ulong body)
        {
            if(!_currentState.ContainsKey(body)) _currentState.Add(body,new Tuple<State, int>(State.Unknown,0));
            return _currentState[body].Item1;
        }

        private void DecreaseFrameTicker(ulong body)
        {
            var tuple = _currentState[body];
            if (tuple.Item1 == State.Unknown) return;
            var newTuple = new Tuple<State, int>(tuple.Item1, tuple.Item2 - 1);
            if (newTuple.Item2 < 0) SetCurrentState(body, State.Unknown, 0);
            else _currentState[body] = newTuple;
        }
    }
}
