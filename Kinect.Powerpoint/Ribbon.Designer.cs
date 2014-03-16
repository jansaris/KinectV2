namespace Kinect.Powerpoint
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab = this.Factory.CreateRibbonTab();
            this.grpKinect = this.Factory.CreateRibbonGroup();
            this.lblBodies = this.Factory.CreateRibbonLabel();
            this.lblError = this.Factory.CreateRibbonLabel();
            this.lblLastEvent = this.Factory.CreateRibbonLabel();
            this.tab.SuspendLayout();
            this.grpKinect.SuspendLayout();
            // 
            // tab
            // 
            this.tab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab.Groups.Add(this.grpKinect);
            this.tab.Label = "TabAddIns";
            this.tab.Name = "tab";
            // 
            // grpKinect
            // 
            this.grpKinect.Items.Add(this.lblBodies);
            this.grpKinect.Items.Add(this.lblError);
            this.grpKinect.Items.Add(this.lblLastEvent);
            this.grpKinect.Label = "Kinect";
            this.grpKinect.Name = "grpKinect";
            // 
            // lblBodies
            // 
            this.lblBodies.Label = "0 Users";
            this.lblBodies.Name = "lblBodies";
            // 
            // lblError
            // 
            this.lblError.Label = "No Errors";
            this.lblError.Name = "lblError";
            // 
            // lblLastEvent
            // 
            this.lblLastEvent.Label = "Last event";
            this.lblLastEvent.Name = "lblLastEvent";
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.PowerPoint.Presentation";
            this.Tabs.Add(this.tab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tab.ResumeLayout(false);
            this.tab.PerformLayout();
            this.grpKinect.ResumeLayout(false);
            this.grpKinect.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpKinect;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblBodies;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblError;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblLastEvent;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
