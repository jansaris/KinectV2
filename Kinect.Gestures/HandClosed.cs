using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Gestures
{
    public class HandClosed : GestureBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HandClosed));
        public enum HandToWatch { HandLeft, HandRight };

        private enum State { Unknown, HandOpen, HandClosed }
        private readonly Dictionary<ulong, Tuple<State, int>> _currentState = new Dictionary<ulong, Tuple<State, int>>();
        private readonly JointType _handToWatch;

        public HandClosed(HandToWatch handToWatch)
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
                    case State.HandOpen:
                        if (IsHandClosed(body))
                        {
                            InvokeDetected(body.TrackingId);
                            SetCurrentState(body.TrackingId, State.HandClosed, 15);
                        }
                        break;
                    case State.HandClosed:
                        if (IsHandOpen(body)) SetCurrentState(body.TrackingId, State.HandOpen);
                        break;
                }
                DecreaseFrameTicker(body.TrackingId);
            }
        }

        private bool IsHandClosed(Body body)
        {
            var handState = _handToWatch == JointType.HandLeft ? body.HandLeftState : body.HandRightState;
            var handConfidence = _handToWatch == JointType.HandLeft ? body.HandLeftConfidence : body.HandRightConfidence;

            Logger.DebugFormat("IsHandClosed: Hand {0}, Confidence {1}", handState, handConfidence);
            return handState == HandState.Closed && handConfidence == TrackingConfidence.High;
        }

        private bool IsHandOpen(Body body)
        {
            var handState = _handToWatch == JointType.HandLeft ? body.HandLeftState : body.HandRightState;
            var handConfidence = _handToWatch == JointType.HandLeft ? body.HandLeftConfidence : body.HandRightConfidence;

            Logger.DebugFormat("IsHandClosed: Hand {0}, Confidence {1}", handState, handConfidence);
            return handState == HandState.Open && handConfidence == TrackingConfidence.High;
        }

        private void SetCurrentState(ulong trackingId, State newState, int frameTickerCount = 10)
        {
            Logger.InfoFormat("Switching state from '{0}' to '{1}' for user '{2}'", _currentState[trackingId], newState, trackingId);
            _currentState[trackingId] = new Tuple<State, int>(newState, frameTickerCount);
        }

        private State GetCurrentState(ulong body)
        {
            if (!_currentState.ContainsKey(body)) _currentState.Add(body, new Tuple<State, int>(State.Unknown, 0));
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