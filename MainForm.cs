using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spark.Installer;

namespace Spark
{
    public enum InstallerViewType {
        Options,
        Progress,
        Complete,
        UninstallOptions,
        ShowError
    }
    public partial class MainForm : Form
    {
        private InstallManager _objInstallerManager;
        private List<InstallerViewType> _objViewStack = new List<InstallerViewType>();
        private BaseView _objCurrentView = null;
        
        public InstallManager GetInstaller() { return _objInstallerManager; }

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            _objInstallerManager = new InstallManager();
            _objInstallerManager.LoadOptions();

            // ** Check if the uninstaller was run manually (without the /u switch) in which case we will switch to uinstall mode.
            ProgramInfo pi = new ProgramInfo(_objInstallerManager.Options);
            if (InstallManager.ApplicationIsInstalled(pi))
            {
                SparkGlobals.ProgramMode = SparkProgramMode.Uninstall;
                Proteus.Globals.Logger.LogWarn("Uninstall run without " + SparkFlags.Uninstall + " switch - detected installed app so switching to uninstall mode.");
            }
            else 
            {
                if (_objInstallerManager.GetOption(InstallOption.UninstallFolderRoot_Uninstaller_Only) != null)
                {
                    // ** if the appliation is not installed, but we are running the uninstaller, then tell the user and quit.
                    DisplayView(InstallerViewType.ShowError);
                    return;
                }
            }

            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 Milliseconds 
            myDispatcherTimer.Tick += new EventHandler(UpdateTick);
            myDispatcherTimer.Start();

            if (SparkGlobals.ProgramMode == SparkProgramMode.Install)
                DisplayView(InstallerViewType.Options);
            else if (SparkGlobals.ProgramMode == SparkProgramMode.Uninstall)
                DisplayView(InstallerViewType.UninstallOptions);
        }
        private void UpdateTick(Object o, EventArgs e)
        {
            //if (_objInstallerManager.InstallState==InstallState.Installing && _objCurrentView!=null)
            //    _objCurrentView.UpdateInstallerProgress(_objInstallerManager.GetInstallProgress());

            //Swap views
            foreach (InstallerViewType view in _objViewStack)
            {
                HideCurrentView();
                DisplayView(view);
            }
            _objViewStack.Clear();
        }
        private void DisplayView(InstallerViewType view)
        {
            BaseView ctl = null;
            if (view == InstallerViewType.Options)
            {
                ctl = new OptionsControl(this);
            }
            if (view == InstallerViewType.Progress)
            {
                ctl = new ProgressControl(this);
            }
            if (view == InstallerViewType.Complete)
            {
                ctl = new CompletionControl(this);
            }
            if (view == InstallerViewType.UninstallOptions)
            {
                ctl = new UninstallOptionsControl(this);
            }
            if (view == InstallerViewType.ShowError)
            {
                ctl = new ErrorControl(this);
            }
            ctl.Dock = DockStyle.Fill;
            this.Controls.Add(ctl);
            _objCurrentView = ctl;
        }
        private void HideCurrentView()
        {
            if (_objCurrentView!=null)
            {
                _objCurrentView.Hide();
                this.Controls.Remove(_objCurrentView);
            }
            _objCurrentView = null;
        }
        public void SwapView(InstallerViewType t)
        {
            _objViewStack.Add(t);
        }



    }
}
