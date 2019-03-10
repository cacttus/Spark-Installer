using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proteus;
namespace Spark
{
    public partial class BuildForm : Form
    {
        public BuildForm()
        {
            InitializeComponent();
        }
        private void BuildForm_Load(object sender, EventArgs e)
        {
        }
        public void DetailMessage(string msg)
        {
            _lblBuildDetails.Text = msg;
            Globals.Logger.LogInfo(msg);
            System.Windows.Forms.Application.DoEvents();
        }
        public double CurrentProgress()
        {
            return (((double)_pgbBuildProgress.Value) / 100.0);
        }
        public void Progress(double progress)
        {
            _pgbBuildProgress.Value = (int)(progress*100);
            System.Windows.Forms.Application.DoEvents();
        }
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
