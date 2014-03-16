using Kinect.Core;
using Kinect.Gestures;
using Kinect.Interfaces;
using Ninject;

namespace Kinect.Powerpoint
{
    internal static class BootStrapper
    {
        public static readonly IKernel Kernel = new StandardKernel();
        private static SlideShowManager _manager;
        public static void BootUp()
        {
            Kernel.Bind<IKinectStatus>().To(typeof (KinectManager)).InSingletonScope();
            Kernel.Bind<IGestureDetector>().To(typeof(KinectManager)).InSingletonScope();
            Kernel.Bind<IGestures>().To(typeof (GestureFactory)).InSingletonScope();
            _manager = new SlideShowManager();
        }

        public static void ShutDown()
        {
            _manager.Dispose();
            Kernel.Dispose();
        }
    }
}
