using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spark.Installer;
using Proteus;

namespace Spark
{
    public partial class ProgressControl : BaseView
    {
        private Object _objUpdateLockObject = new Object();
        System.Windows.Threading.DispatcherTimer myDispatcherTimer;

        public ProgressControl(MainForm objForm)
        {
            InitializeComponent();
            Init(objForm);
        }
        private void ProgressControl_Load(object sender, EventArgs e)
        {
            if(SparkGlobals.ProgramMode == SparkProgramMode.Install)
                _objForm.GetInstaller().BeginInstallation();
            else if(SparkGlobals.ProgramMode == SparkProgramMode.Uninstall)
                _objForm.GetInstaller().BeginUninstall();
            else
                Globals.Throw("Invalid enumeration for progress window.");

            myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 Milliseconds 
            myDispatcherTimer.Tick += new EventHandler(UpdateTick);
            myDispatcherTimer.Start();
        }
        private void UpdateTick(Object o, EventArgs e)
        {
            if (!System.Threading.Monitor.TryEnter(_objUpdateLockObject))
                return;

            UpdateInstallProgress();
            HandleInstallState();

            System.Threading.Monitor.Exit(_objUpdateLockObject);
        }
        private void UpdateInstallProgress()
        {
            try
            {
                double progress = _objForm.GetInstaller().InstallProgress;
                _pgbInstallProgress.Value = (int)(progress * 100.0);
            }
            catch (Exception ex)
            {
                _pgbInstallProgress.Value = 100;
            }
        }
        private void HandleInstallState()
        {
            Installer.InstallState state = _objForm.GetInstaller().InstallState;
            switch (state)
            {
                case Installer.InstallState.Successful:
                    myDispatcherTimer.Stop();
                    _objForm.SwapView(InstallerViewType.Complete);
                    break;
                case Installer.InstallState.Canceled:
                    myDispatcherTimer.Stop();
                    _objForm.SwapView(InstallerViewType.Complete);
                    break;
                case Installer.InstallState.Installing:
                    _lblInstallDetails.Text = "Installing.." + _objForm.GetInstaller().CurrentFile;
                    break;
                case Installer.InstallState.Uninstalling:
                    _lblInstallDetails.Text = "Uninstalling.." + _objForm.GetInstaller().CurrentFile;
                    break;
                case Installer.InstallState.Rollingback:
                    _lblInstallDetails.Text = "Rolling Back.." + _objForm.GetInstaller().CurrentFile;
                    break;
            }
        }
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            _objForm.GetInstaller().CancelInstallation();
        }
        public override void UpdateInstallerProgress(double progress)
        {
            _pgbInstallProgress.Value = (int)progress;
        }

    }
}
