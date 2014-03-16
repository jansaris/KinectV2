using System;
using Microsoft.Kinect;

namespace Kinect.Interfaces
{
    public interface IGesture
    {
        event EventHandler Detected;
        void FrameArrived(object sender, BodyFrameArrivedEventArgs e);
    }
}