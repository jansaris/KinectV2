using System;

namespace Kinect.Interfaces
{
    public interface IGestures
    {
        event EventHandler<ulong> GrabAndThrowRightHanded;
        event EventHandler<ulong> GrabAndThrowLeftHanded;
    }
}
