using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kinect.Keyboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            BootStrapper.BootUp();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            BootStrapper.ShutDown();
            base.OnExit(e);
        }
    }
}
