using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spark
{
    public class BaseView : UserControl
    {
        protected MainForm _objForm;

        public BaseView()
        {
            InitializeComponent();
        }
        protected void Init(MainForm objForm)
        {
            //need to do this because of ** designer.
            _objForm = objForm;
        }

        public virtual void UpdateInstallerProgress(double progress)
        {
            ; // override to update with install progress.
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseView
            // 
            this.Name = "BaseView";
            this.Load += new System.EventHandler(this.BaseView_Load);
            this.ResumeLayout(false);

        }

        private void BaseView_Load(object sender, EventArgs e)
        {

        }

    }
}
