using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kinect.Interfaces;
using Kinect.Keyboard.Annotations;
using Ninject;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Kinect.Keyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private float _handLeft;
        private float _handTop;
        private float _kinectX;
        private float _kinectY;
        private float MaxValue = 736;
        private IKernel Kernel { get { return BootStrapper.Kernel; } }

        public float KinectX
        {
            get { return _kinectX; }
            set
            {
                _kinectX = value;
                OnPropertyChanged();
                OnPropertyChanged("ScreenMessage");
            }
        }

        public float KinectY
        {
            get { return _kinectY; }
            set
            {
                _kinectY = value;
                OnPropertyChanged();
                OnPropertyChanged("ScreenMessage");
            }
        }

        public string ScreenMessage
        {
            get
            {
                return string.Format("X: {0:N2} - Y: {1:N2}", KinectX, KinectY);
            }
        }

        public float HandTop
        {
            get { return _handTop; }
            set
            {
                _handTop = value;
                OnPropertyChanged();
            }
        }

        public float HandLeft
        {
            get { return _handLeft; }
            set
            {
                _handLeft = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            HandTop = 363;
            HandLeft = 339;

            DataContext = this;
            InitializeComponent();
            HandImage.Source = CreateBitmapSourceFromGdiBitmap(Properties.Resources.hand);
            InitializeKinect();
        }

        private void InitializeKinect()
        {
            var tracker = new HandTracker();
            tracker.Detected += (s, e) =>
            {
                if (!tracker.HandRight.HasValue) return;
                KinectY = tracker.HandRight.Value.Y;
                KinectX = tracker.HandRight.Value.X;
                HandTop = (MaxValue / 2) - (KinectY * MaxValue);
                HandLeft = (MaxValue / 2) + (KinectX * MaxValue);
            };

            Kernel.Get<IGestureDetector>().RegisterGesture(tracker);
        }

        private void Window_Closed(object sender, EventArgs eventArgs)
        {
            BootStrapper.ShutDown();
            Environment.Exit(0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
