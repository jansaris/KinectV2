using System.Linq;
using Kinect.Gestures;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    public class ClapGesture : GestureBase
    {
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
            return false;
        }
    }
}