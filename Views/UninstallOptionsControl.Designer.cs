namespace Spark
{
    partial class UninstallOptionsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this._btnExit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._optUninstall = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._optRepair = new System.Windows.Forms.RadioButton();
            this._btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnExit
            // 
            this._btnExit.Location = new System.Drawing.Point(345, 298);
            this._btnExit.Name = "_btnExit";
            this._btnExit.Size = new System.Drawing.Size(86, 31);
            this._btnExit.TabIndex = 23;
            this._btnExit.Text = "Exit";
            this._btnExit.UseVisualStyleBackColor = true;
            this._btnExit.Click += new System.EventHandler(this._btnExit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::Spark.Properties.Resources.spark_logo1;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 62);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // _optUninstall
            // 
            this._optUninstall.AutoSize = true;
            this._optUninstall.Checked = true;
            this._optUninstall.Location = new System.Drawing.Point(31, 54);
            this._optUninstall.Name = "_optUninstall";
            this._optUninstall.Size = new System.Drawing.Size(65, 17);
            this._optUninstall.TabIndex = 25;
            this._optUninstall.TabStop = true;
            this._optUninstall.Text = "Uninstall";
            this._optUninstall.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._optRepair);
            this.groupBox1.Controls.Add(this._optUninstall);
            this.groupBox1.Location = new System.Drawing.Point(91, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 148);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Please select an option below and press Ok";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // _optRepair
            // 
            this._optRepair.AutoSize = true;
            this._optRepair.Enabled = false;
            this._optRepair.Location = new System.Drawing.Point(31, 31);
            this._optRepair.Name = "_optRepair";
            this._optRepair.Size = new System.Drawing.Size(56, 17);
            this._optRepair.TabIndex = 25;
            this._optRepair.Text = "Repair";
            this._optRepair.UseVisualStyleBackColor = true;
            // 
            // _btnOk
            // 
            this._btnOk.Location = new System.Drawing.Point(164, 244);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(85, 31);
            this._btnOk.TabIndex = 27;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // UninstallOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._btnExit);
            this.Controls.Add(this.pictureBox1);
            this.Name = "UninstallOptionsControl";
            this.Size = new System.Drawing.Size(434, 332);
            this.Load += new System.EventHandler(this.UninstallOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnExit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton _optUninstall;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.RadioButton _optRepair;
    }
}
