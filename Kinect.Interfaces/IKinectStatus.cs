using System;

namespace Kinect.Interfaces
{
    public interface IKinectStatus 
    {
        string Status { get; }
        int TrackedBodies { get; }
        event EventHandler<bool> AvailabilityChanged;
        event EventHandler<string> ErrorOccured;
        event EventHandler<int> TrackedBodyCountChanged;
    }
}