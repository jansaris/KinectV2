using Kinect.Core;
using Kinect.Gestures;
using Kinect.Interfaces;
using Ninject;

namespace Kinect.Keyboard
{
    internal static class BootStrapper
    {
        public static readonly IKernel Kernel = new StandardKernel();
        public static void BootUp()
        {
            Kernel.Bind<IKinectStatus>().To(typeof(KinectManager)).InSingletonScope();
            Kernel.Bind<IGestureDetector>().To(typeof(KinectManager)).InSingletonScope();
        }

        public static void ShutDown()
        {
            Kernel.Dispose();
        }
    }
}