using System;
using Kinect.Core;
using Kinect.Gestures;
using log4net;
using log4net.Appender;

namespace Kinect.DisplayLog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainWindow));
        private KinectManager _kinectManager;
        private GestureFactory _gestureFactory;
        public MainWindow()
        {
            InitializeComponent();
            InitializeLog4Net();
            Logger.Info("Applicatie is gestart");
            InitializeKinect();
            
        }

        void TrackedBodyCountChanged(object sender, int e)
        {
            Logger.WarnFormat("TrackedBodyCount changed to: {0}", e);
        }

        void GrabAndThrowRight(object sender, ulong e)
        {
            Logger.Warn("GrabAndThrow detected (right)");
        }

        void GrabAndThrowLeft(object sender, ulong e)
        {
            Logger.Warn("GrabAndThrow detected (left)");
        }

        private void InitializeLog4Net()
        {
            var textBoxAppender = new TextBoxAppender {Threshold = log4net.Core.Level.Info, AppenderTextBox = txtLog};
            var consoleAppender = new ConsoleAppender { Layout = new log4net.Layout.SimpleLayout() };
            log4net.Config.BasicConfigurator.Configure(new IAppender[] { textBoxAppender, consoleAppender });
        }

        private void InitializeKinect()
        {
            try
            {
                Logger.InfoFormat("Initialize kinect");
                _kinectManager = new KinectManager();
                _gestureFactory = new GestureFactory(_kinectManager);
                //_gestureFactory.GrabAndThrowLeftHanded += GrabAndThrowLeft;
                //_gestureFactory.GrabAndThrowRightHanded += GrabAndThrowRight;
                _gestureFactory.RightHandClosed += RightHandClosed;
                _gestureFactory.LeftHandClosed += LeftHandClosed;
                _gestureFactory.RightHandOpened += RightHandOpened;
                _gestureFactory.LeftHandOpened += LeftHandOpened;
                _kinectManager.TrackedBodyCountChanged += TrackedBodyCountChanged;
                Logger.InfoFormat("Kinect initialized with status: {0}", _kinectManager.Status);
            }
            catch (Exception eX)
            {
                Logger.ErrorFormat("Failed to start kinect: {0}", eX.Message);
            }
        }

        void LeftHandOpened(object sender, ulong e)
        {
            Logger.Warn("Hand opened detected (left)");
        }

        void RightHandOpened(object sender, ulong e)
        {
            Logger.Warn("Hand opened detected (right)");
        }

        private void LeftHandClosed(object sender, ulong e)
        {
            Logger.Warn("Hand closed detected (left)");
        }

        private void RightHandClosed(object sender, ulong e)
        {
            Logger.Warn("Hand closed detected (right)");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_kinectManager != null)
            {
                _kinectManager.Dispose();
            }
            Environment.Exit(0);
        }
    }
}
