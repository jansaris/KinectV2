using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Kinect.Interfaces;
using Kinect.Keyboard.Arrows;
using Microsoft.Kinect;
using Ninject;

namespace Kinect.Keyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IKernel Kernel { get { return BootStrapper.Kernel; } }
        private bool _trackRightHand;
        private CameraSpacePoint? _startRightHand = null;
        private HandTracker _tracker;

        public MainWindow()
        {
            InitializeComponent();
            DrawCanvas(true);
            InitializeKinect();
        }

        private void InitializeKinect()
        {
            var factory = Kernel.Get<IGestures>();
            factory.RightHandClosed += (s, e) =>
            {
                _trackRightHand = true;
            };
                
            factory.RightHandOpened += (s, e) => TurnRightHandOff();
            _tracker = new HandTracker();
            _tracker.Detected += (s, e) => DrawCanvas();
            Kernel.Get<IGestureDetector>().RegisterGesture(_tracker);
        }

        private void TurnRightHandOff()
        {
            _trackRightHand = false;
            _startRightHand = null;
            DrawCanvas(true);
        }

        private void DrawCanvas(bool force = false)
        {
            if (!(_trackRightHand || force)) return;
            Debug.WriteLine("Drawing canvas with tracking: {0}", _trackRightHand);

            Canvas.Children.Clear();
            AddArrows();
            AddRightHandPointer();
        }

        private void AddArrows()
        {
            var thickness = 18;
            var color = Brushes.Red;

            var down = new ArrowLine { Stroke = color, StrokeThickness = thickness, X1 = 200, Y1 = 250, X2 = 200, Y2 = 300 };
            var up = new ArrowLine { Stroke = color, StrokeThickness = thickness, X1 = 200, Y1 = 100, X2 = 200, Y2 = 50 };
            var left = new ArrowLine { Stroke = color, StrokeThickness = thickness, X1 = 100, Y1 = 181, X2 = 50, Y2 = 181 };
            var right = new ArrowLine { Stroke = color, StrokeThickness = thickness, X1 = 300, Y1 = 181, X2 = 350, Y2 = 181 };

            Canvas.Children.Add(up);
            Canvas.Children.Add(left);
            Canvas.Children.Add(right);
            Canvas.Children.Add(down);
        }

        private void AddRightHandPointer()
        {
            if (!_trackRightHand) return;
            if (!_startRightHand.HasValue) _startRightHand = _tracker.HandRight;
            if (_tracker.HandRight.HasValue && _startRightHand.HasValue) AddPointer(_tracker.HandRight.Value, _startRightHand.Value);
        }

        private void AddPointer(CameraSpacePoint handRight, CameraSpacePoint startRightHand)
        {
            var xDiff = startRightHand.X - handRight.X;
            var yDiff = startRightHand.Z - handRight.Z;
            var circle = new Ellipse
            {
                Width = 30,
                Height = 30,
                StrokeThickness = 15,
                Stroke = Brushes.Green
            };
            var top = 175 - (2000*yDiff);
            var left = 200 - (2000 * xDiff);

            System.Windows.Controls.Canvas.SetTop(circle, top);
            System.Windows.Controls.Canvas.SetLeft(circle, left);
            Canvas.Children.Add(circle);
        }

        private void Window_Closed(object sender, EventArgs eventArgs)
        {
            Kernel.Get<IGestureDetector>().UnRegisterGesture(_tracker);
            BootStrapper.ShutDown();
            Environment.Exit(0);
        }
    }
}
