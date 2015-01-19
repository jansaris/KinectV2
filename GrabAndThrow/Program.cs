using System;
using Kinect.Core;
using Kinect.Gestures;
using log4net;

namespace GrabAndThrow
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private Program()
        {
            Logger.Info("Start Application");
            var manager = new KinectManager();
            var left = new Kinect.Gestures.GrabAndThrow(HandToWatch.HandLeft);
            left.Detected += LeftOnDetected;
            manager.RegisterGesture(left);
            var right = new Kinect.Gestures.GrabAndThrow(HandToWatch.HandRight);
            right.Detected += RightOnDetected;
            manager.RegisterGesture(right);
        }

        private void RightOnDetected(object sender, ulong @ulong)
        {
            Console.WriteLine("Right");
            TryToSendKey("{RIGHT}");
        }
        private void LeftOnDetected(object sender, ulong @ulong)
        {
            Console.WriteLine("Left");
            TryToSendKey("{LEFT}");
        }

        private void TryToSendKey(string key)
        {
            try
            {
                System.Windows.Forms.SendKeys.SendWait(key);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Failed to send key: {0}", key), ex);
            }
        }


        private void Run()
        {
            Console.WriteLine("Running, hit Enter to exit");
            Console.ReadLine();
            Logger.Info("Exit application");
        }
    }
}
