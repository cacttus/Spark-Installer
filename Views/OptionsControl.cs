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

namespace Spark
{
    public partial class OptionsControl : BaseView
    {
        public OptionsControl(MainForm objForm)
        {
            InitializeComponent();
            Init(objForm);
            _grpOptions.Visible = false;
            ResizeItems();

            Installer.InstallOption opt = objForm.GetInstaller().GetOption(Installer.InstallOption.DefaultDirectory);
            if (opt != null)
            {
                opt.Value = objForm.GetInstaller().ReplaceValueVariables(opt.Value);
                _txtInstallDir.Text = opt.Value;
            }

        }

        private void _btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void _btnInstall_Click(object sender, EventArgs e)
        {
            // Add the options
            if (_chkDesktopIcon.Visible)
            {
                if (_chkDesktopIcon.Checked == true)
                    _objForm.GetInstaller().AddOrReplaceOption(InstallOption.CreateDesktopShortcut, InstallOptionValue.True);
                else
                    _objForm.GetInstaller().AddOrReplaceOption(InstallOption.CreateDesktopShortcut, InstallOptionValue.False);
            }
            if (_chkStartMenuIcon.Visible)
            {
                if (_chkStartMenuIcon.Checked == true)
                    _objForm.GetInstaller().AddOrReplaceOption(InstallOption.CreateStartMenuFolder, InstallOptionValue.True);
                else
                    _objForm.GetInstaller().AddOrReplaceOption(InstallOption.CreateStartMenuFolder, InstallOptionValue.False);
            }

            _objForm.GetInstaller().AddOrReplaceOption(InstallOption.InstallLocation, _txtInstallDir.Text);

            // Swap the view
            _objForm.SwapView(InstallerViewType.Progress);
        }
        private void OptionsControl_Load(object sender, EventArgs e)
        {
        }
        private void _lblOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _grpOptions.Visible = !_grpOptions.Visible;
            ResizeItems();
        }

        private void ResizeItems()
        {
            Control ctl = null;
            int yoff = 0;
            if (_grpOptions.Visible)
            {
                ctl = _grpOptions;
                yoff = 18;
            }
            else
            {
                ctl = _txtInstallDir;
                yoff = 30;
            }

            _btnInstall.Location = new System.Drawing.Point(
            (int)(
            ctl.Location.X + ctl.ClientSize.Width / 2 - _btnInstall.ClientSize.Width / 2)
            ,
            (int)(
            ctl.Location.Y + ctl.Height + yoff)
            );
        }

        private void _btnSelectPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog d = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult res = d.ShowDialog();
            if (res == DialogResult.OK)
            {
                _txtInstallDir.Text = d.SelectedPath;
            }

        }




    }
}
