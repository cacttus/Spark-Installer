using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spark
{
    public partial class UninstallOptionsControl : BaseView
    {
        string _strProgramName = "";
        public UninstallOptionsControl(MainForm objForm)
        {
            InitializeComponent();
            Init(objForm);
        }

        private void _btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        
        private void UninstallOptions_Load(object sender, EventArgs e)
        {
            Installer.InstallOption opt = _objForm.GetInstaller().GetOption(Installer.InstallOption.DisplayName);
            if (opt != null)
                _strProgramName = opt.Value;
            _optUninstall.Text = "Uninstall " + _strProgramName;
            _optRepair.Text    = "Repair " + _strProgramName;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (_optUninstall.Checked == true)
            {
                Installer.InstallOption opt = _objForm.GetInstaller().GetOption(Installer.InstallOption.DisplayName);

                DialogResult res = System.Windows.Forms.MessageBox.Show(
                    "Are you sure you want to uninstall " 
                    + _strProgramName 
                    + "?",
                    "Confirm Uninstall",
                    MessageBoxButtons.YesNo);

                if(res == System.Windows.Forms.DialogResult.Yes)
                    _objForm.SwapView(InstallerViewType.Progress);
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
