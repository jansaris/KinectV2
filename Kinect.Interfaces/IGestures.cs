using System;

namespace Kinect.Interfaces
{
    public interface IGestures
    {
        event EventHandler GrabAndThrowRightHanded;
        event EventHandler GrabAndThrowLeftHanded;
    }
}
