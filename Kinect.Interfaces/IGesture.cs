using System;
using Microsoft.Kinect;

namespace Kinect.Interfaces
{
    public interface IGesture
    {
        event EventHandler<ulong> Detected;
        void FrameArrived(object sender, BodyFrameArrivedEventArgs e);
    }
}