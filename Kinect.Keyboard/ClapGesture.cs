using System.Linq;
using Kinect.Gestures;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Keyboard
{
    public class ClapGesture : GestureBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ClapGesture));


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
            return false;
        }
    }
}