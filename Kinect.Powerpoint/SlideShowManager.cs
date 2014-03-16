using System;
using Kinect.Interfaces;
using log4net;
using Microsoft.Office.Interop.PowerPoint;
using Ninject;

namespace Kinect.Powerpoint
{
    class SlideShowManager : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SlideShowManager));

        private readonly IGestures _detector;

        public SlideShowManager()
        {
            _detector = BootStrapper.Kernel.Get<IGestures>();
            _detector.GrabAndThrowRightHanded += GrabAndThrowRightHanded;
            _detector.GrabAndThrowLeftHanded += GrabAndThrowLeftHanded;
        }

        private void GrabAndThrowLeftHanded(object sender, ulong e)
        {
            Logger.DebugFormat("Try to execute 'previous' on presentation");
            ExecuteOnSlideShow(view => view.Previous());
        }

        private void GrabAndThrowRightHanded(object sender, ulong e)
        {
            Logger.DebugFormat("Try to execute 'next' on presentation");
            ExecuteOnSlideShow(view => view.Next());
        }

        private void ExecuteOnSlideShow(Action<SlideShowView> toExecute)
        {
            try
            {
                var view = GetSlideShowView();
                if (view == null) return;
                if (view.State == PpSlideShowState.ppSlideShowRunning)
                {
                    Logger.DebugFormat("Execute action on presentation");
                    toExecute(view);
                }
                else
                {
                    Logger.InfoFormat("Can't execute action because presentation state is: {0}", view.State);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error when executing aciton on active presentation", ex);
            }
        }

        private static SlideShowView GetSlideShowView()
        {
            try
            {
                var presentation = Globals.ThisAddIn.Application.ActivePresentation;
                if (presentation == null)
                {
                    Logger.Info("No active presentation");
                    return null;
                }
                if (presentation.SlideShowWindow == null)
                {
                    Logger.Info("No active slide show window");
                    return null;
                }
                if (presentation.SlideShowWindow.View == null)
                {
                    Logger.Info("No view on active slide show window");
                    return null;
                }
                return presentation.SlideShowWindow.View;
            }
            catch (Exception ex)
            {
                Logger.WarnFormat("Failed to retrieve active presentation: {0}", ex.Message);
            }
            return null;
        }

        public void Dispose()
        {
            BootStrapper.Kernel.Release(_detector);
        }
    }
}
