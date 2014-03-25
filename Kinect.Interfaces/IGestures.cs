using System;

namespace Kinect.Interfaces
{
    public interface IGestures
    {
        event EventHandler<ulong> GrabAndThrowRightHanded;
        event EventHandler<ulong> GrabAndThrowLeftHanded;
        event EventHandler<ulong> LeftHandOpened;
        event EventHandler<ulong> LeftHandClosed;
        event EventHandler<ulong> RightHandOpened;
        event EventHandler<ulong> RightHandClosed;
    }
}
