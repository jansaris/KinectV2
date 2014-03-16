using System;
using Microsoft.Kinect;

namespace Kinect.Gestures
{
    public static class Extensions
    {
        public static string ToReadableString(this CameraSpacePoint point)
        {
            return String.Format("X: {0}; Y: {1}; Z: {2}", point.X, point.Y, point.Z);
        }
    }
}
