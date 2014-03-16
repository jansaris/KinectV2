using System;
using System.Globalization;
using Kinect.Interfaces;
using Microsoft.Office.Tools.Ribbon;
using Ninject;

namespace Kinect.Powerpoint
{
    public partial class Ribbon
    {
        private IKinectStatus _kinectStatus;
        private IGestures _gestures;
        private int _count;
        private string _lastEvent = string.Empty;

        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
            _kinectStatus = BootStrapper.Kernel.Get<IKinectStatus>();
            UpdateUsers(null, _kinectStatus.TrackedBodies);

            _kinectStatus.AvailabilityChanged += UpdateAvailability;
            _kinectStatus.TrackedBodyCountChanged += UpdateUsers;
            _kinectStatus.ErrorOccured += _kinectStatus_ErrorOccured;

            _gestures = BootStrapper.Kernel.Get<IGestures>();
            _gestures.GrabAndThrowLeftHanded += GrabAndThrowLeftHanded;
            _gestures.GrabAndThrowRightHanded += GrabAndThrowRightHanded;
        }

        private void GrabAndThrowRightHanded(object sender, EventArgs e)
        {
            UpdateLastEvent("Grab and throw (right hand)");
        }

        void GrabAndThrowLeftHanded(object sender, EventArgs e)
        {
            UpdateLastEvent("Grab and throw (left hand)");
        }

        void _kinectStatus_ErrorOccured(object sender, string error)
        {
            lblError.Label = error;
        }

        private void UpdateLastEvent(string eventName)
        {
            if (_lastEvent == eventName) _count++;
            else
            {
                _lastEvent = eventName;
                _count = 1;
            }
            lblLastEvent.Label = string.Format("{0} {1}", _count, _lastEvent);
        }

        private void UpdateAvailability(object sender, bool status)
        {
            if(!status) lblError.Label = "Kinect disconnected";
        }

        private void UpdateUsers(object sender, int users)
        {
            lblBodies.Label = string.Format("{0} users", users == 0 ? "No" : users.ToString(CultureInfo.InvariantCulture));
        }
    }
}
