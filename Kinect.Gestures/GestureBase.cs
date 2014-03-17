using System;
using Kinect.Interfaces;
using log4net;
using Microsoft.Kinect;

namespace Kinect.Gestures
{
    public abstract class GestureBase : IGesture
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GestureBase));
        protected readonly Body[] Bodies = new Body[6];

        public event EventHandler<ulong> Detected;
        public void FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            try
            {
                using (var frame = e.FrameReference.AcquireFrame())
                {
                    if (frame == null) return;
                    frame.GetAndRefreshBodyData(Bodies);
                    AnalyzeNewBodyData();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("An error occured during processing of gesture: {0}", ex.Message);
            }
        }

        protected abstract void AnalyzeNewBodyData();

        protected void InvokeDetected(ulong userId)
        {
            var handler = Detected;
            if (handler != null)
            {
                handler.Invoke(this, userId);
            }
        }
    }
}
