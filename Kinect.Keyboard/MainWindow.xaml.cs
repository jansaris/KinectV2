using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Kinect.Interfaces;
using Kinect.Keyboard.Annotations;
using WpfAnimatedGif;

namespace Kinect.Keyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly IGestureDetector _gestureDetector;
        private float _handLeft;
        private float _handTop;
        private float _kinectX;
        private float _kinectY;

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
            get { return string.Format("X: {0:N2} - Y: {1:N2}", KinectX, KinectY); }
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

        public Visibility HandUpVisibility { get; private set; }
        public Visibility ClapVisibility { get; private set; }

        public MainWindow(IGestureDetector gestureDetector)
        {
            _gestureDetector = gestureDetector;
            HandTop = 363;
            HandLeft = 339;

            DataContext = this;
            InitializeComponent();
            PrepareImages();
            InitializeKinect();
        }

        private void InitializeKinect()
        {
            InitializeHandTracker();
        }

        private void InitializeHandTracker()
        {
            var tracker = new HandTracker();
            tracker.Detected += (s, e) =>
            {
                KinectX = tracker.KinectX;
                KinectY = tracker.KinectY;
                HandLeft = tracker.ScreenX;
                HandTop = tracker.ScreenY;
            };

            _gestureDetector.RegisterGesture(tracker);
        }

        private void PrepareImages()
        {
            HandImage.Source = CreateBitmapImage("Hand.png");
            HandUpImage.Source = CreateBitmapImage("HandUp.jpg");
            ImageBehavior.SetAnimatedSource(ClapImage, CreateBitmapImage("Clap.gif"));
        }

        private BitmapImage CreateBitmapImage(string image)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(Environment.CurrentDirectory + "\\Resources\\" + image);
            img.EndInit();
            return img;
        }

        private void Window_Closed(object sender, EventArgs eventArgs)
        {
            Environment.Exit(0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
