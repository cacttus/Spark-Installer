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
    public partial class ErrorControl : BaseView
    {
        public ErrorControl(MainForm objForm)
        {
            InitializeComponent();
            Init(objForm);
        }

        private void _btnOk_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ErrorControl_Load(object sender, EventArgs e)
        {

        }
    }
}
