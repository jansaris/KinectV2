using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Gestures
{
    public class HandOpened : GestureBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HandClosed));

        private enum State { Unknown, HandOpen }
        private readonly Dictionary<ulong, Tuple<State, int>> _currentState = new Dictionary<ulong, Tuple<State, int>>();
        private readonly JointType _handToWatch;

        public HandOpened(HandToWatch handToWatch)
        {
            _handToWatch = handToWatch == HandToWatch.HandLeft ? JointType.HandLeft : JointType.HandRight;
        }

        protected override void AnalyzeNewBodyData()
        {
            foreach (var body in Bodies.Where(b => b.IsTracked))
            {
                var state = GetCurrentState(body.TrackingId);
                switch (state.Item1)
                {
                    case State.HandOpen:
                        if (IsHandNotOpen(body)) SetCurrentState(body.TrackingId, State.Unknown);
                        break;
                    default:
                        if (IsHandOpen(body)) IncreaseTicker(body.TrackingId);
                        else if(state.Item2 != 0) SetCurrentState(body.TrackingId, State.Unknown);

                        if (state.Item2 > 2)
                        {
                            SetCurrentState(body.TrackingId, State.HandOpen);
                            InvokeDetected(body.TrackingId);
                        }
                        break;

                }
            }
        }

        private bool IsHandOpen(Body body)
        {
            var handState = _handToWatch == JointType.HandLeft ? body.HandLeftState : body.HandRightState;
            var handConfidence = _handToWatch == JointType.HandLeft ? body.HandLeftConfidence : body.HandRightConfidence;

            Logger.DebugFormat("IsHandOpen: Hand {0}, Confidence {1}", handState, handConfidence);
            return handState == HandState.Open && handConfidence == TrackingConfidence.High;
        }

        private bool IsHandNotOpen(Body body)
        {
            var handState = _handToWatch == JointType.HandLeft ? body.HandLeftState : body.HandRightState;
            var handConfidence = _handToWatch == JointType.HandLeft ? body.HandLeftConfidence : body.HandRightConfidence;

            Logger.DebugFormat("IsHandNotOpen: Hand {0}, Confidence {1}", handState, handConfidence);
            return handState != HandState.Open;
        }

        private void SetCurrentState(ulong trackingId, State newState)
        {
            Logger.DebugFormat("Switching state from '{0}' to '{1}' for user '{2}'", _currentState[trackingId], newState, trackingId);
            _currentState[trackingId] = new Tuple<State, int>(newState, 0);
        }

        private Tuple<State, int> GetCurrentState(ulong body)
        {
            if (!_currentState.ContainsKey(body)) _currentState.Add(body, new Tuple<State, int>(State.Unknown, 0));
            return _currentState[body];
        }

        private void IncreaseTicker(ulong body)
        {
            var tuple = _currentState[body];
            _currentState[body] = new Tuple<State, int>(tuple.Item1, tuple.Item2 + 1);
        }
    }
}