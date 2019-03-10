namespace Spark
{
    partial class CompletionControl
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
            this.button1 = new System.Windows.Forms.Button();
            this._lblInstallStatus = new System.Windows.Forms.Label();
            this._lstInstallErrorBox = new System.Windows.Forms.ListBox();
            this._lblLogDir = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(320, 289);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _lblInstallStatus
            // 
            this._lblInstallStatus.AutoSize = true;
            this._lblInstallStatus.Location = new System.Drawing.Point(145, 30);
            this._lblInstallStatus.Name = "_lblInstallStatus";
            this._lblInstallStatus.Size = new System.Drawing.Size(112, 13);
            this._lblInstallStatus.TabIndex = 1;
            this._lblInstallStatus.Text = "Installation Successful";
            this._lblInstallStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _lstInstallErrorBox
            // 
            this._lstInstallErrorBox.FormattingEnabled = true;
            this._lstInstallErrorBox.Location = new System.Drawing.Point(24, 101);
            this._lstInstallErrorBox.Name = "_lstInstallErrorBox";
            this._lstInstallErrorBox.Size = new System.Drawing.Size(355, 121);
            this._lstInstallErrorBox.TabIndex = 2;
            this._lstInstallErrorBox.Visible = false;
            // 
            // _lblLogDir
            // 
            this._lblLogDir.AutoSize = true;
            this._lblLogDir.Location = new System.Drawing.Point(24, 229);
            this._lblLogDir.Name = "_lblLogDir";
            this._lblLogDir.Size = new System.Drawing.Size(56, 13);
            this._lblLogDir.TabIndex = 3;
            this._lblLogDir.Text = "Log Direct";
            this._lblLogDir.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::Spark.Properties.Resources.spark_logo1;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 62);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // CompletionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._lblLogDir);
            this.Controls.Add(this._lstInstallErrorBox);
            this.Controls.Add(this._lblInstallStatus);
            this.Controls.Add(this.button1);
            this.Name = "CompletionControl";
            this.Size = new System.Drawing.Size(409, 328);
            this.Load += new System.EventHandler(this.CompletionControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label _lblInstallStatus;
        private System.Windows.Forms.ListBox _lstInstallErrorBox;
        private System.Windows.Forms.Label _lblLogDir;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
