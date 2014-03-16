namespace Kinect.Interfaces
{
    public interface IGestureDetector
    {
        void RegisterGesture(IGesture gesture);
        void UnRegisterGesture(IGesture gesture);
    }
}