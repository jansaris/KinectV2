﻿using Office = Microsoft.Office.Core;

namespace Kinect.Powerpoint
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            BootStrapper.BootUp();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            BootStrapper.ShutDown();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }
        
        #endregion
    }
}
