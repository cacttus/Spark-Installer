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
    public partial class CompletionControl : BaseView
    {
        public CompletionControl(MainForm objForm)
        {
            InitializeComponent();
            Init(objForm);
            _lblLogDir.Text = "Logfile: " + Proteus.Globals.Logger.LogFilePath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void CompletionControl_Load(object sender, EventArgs e)
        {
            string strInstallOrUninstall = "";
            if (SparkGlobals.ProgramMode == SparkProgramMode.Uninstall)
                strInstallOrUninstall = "Uninstall";
            else if (SparkGlobals.ProgramMode == SparkProgramMode.Install)
                strInstallOrUninstall = "Install";
            else
                Globals.Throw("Invalid enumeration for completion window.");

            if (_objForm.GetInstaller().InstallState == Installer.InstallState.Canceled)
            {
                _lstInstallErrorBox.Show();
                _lblLogDir.Show();
                _lblInstallStatus.Text = strInstallOrUninstall + " failed.\r\n\r\nPlease see the error log below.\r\n\r\nPress exit to close this window.";

                //There were errrors
                foreach (string str in _objForm.GetInstaller().InstallErrors)
                {
                    _lstInstallErrorBox.Items.Add(str);
                }
            }
            else
            {
                _lblInstallStatus.Text = strInstallOrUninstall + " successful.\r\n\r\nPress exit to close this window.";
            }

        }
    }
}
