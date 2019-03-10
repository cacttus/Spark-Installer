namespace Spark
{
    partial class OptionsControl
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
            this._grpOptions = new System.Windows.Forms.GroupBox();
            this._chkStartMenuIcon = new System.Windows.Forms.CheckBox();
            this._chkDesktopIcon = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this._btnInstall = new System.Windows.Forms.Button();
            this._lblOptions = new System.Windows.Forms.LinkLabel();
            this._btnSelectPath = new System.Windows.Forms.Button();
            this._txtInstallDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnExit
            // 
            this._btnExit.Location = new System.Drawing.Point(350, 303);
            this._btnExit.Name = "_btnExit";
            this._btnExit.Size = new System.Drawing.Size(81, 28);
            this._btnExit.TabIndex = 21;
            this._btnExit.Text = "Exit";
            this._btnExit.UseVisualStyleBackColor = true;
            this._btnExit.Click += new System.EventHandler(this._btnExit_Click);
            // 
            // _grpOptions
            // 
            this._grpOptions.Controls.Add(this._chkStartMenuIcon);
            this._grpOptions.Controls.Add(this._chkDesktopIcon);
            this._grpOptions.Location = new System.Drawing.Point(39, 164);
            this._grpOptions.Name = "_grpOptions";
            this._grpOptions.Size = new System.Drawing.Size(346, 98);
            this._grpOptions.TabIndex = 20;
            this._grpOptions.TabStop = false;
            // 
            // _chkStartMenuIcon
            // 
            this._chkStartMenuIcon.AutoSize = true;
            this._chkStartMenuIcon.Checked = true;
            this._chkStartMenuIcon.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkStartMenuIcon.Location = new System.Drawing.Point(70, 43);
            this._chkStartMenuIcon.Name = "_chkStartMenuIcon";
            this._chkStartMenuIcon.Size = new System.Drawing.Size(176, 17);
            this._chkStartMenuIcon.TabIndex = 3;
            this._chkStartMenuIcon.Text = "Create a folder in the start menu";
            this._chkStartMenuIcon.UseVisualStyleBackColor = true;
            // 
            // _chkDesktopIcon
            // 
            this._chkDesktopIcon.AutoSize = true;
            this._chkDesktopIcon.Checked = true;
            this._chkDesktopIcon.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkDesktopIcon.Location = new System.Drawing.Point(70, 20);
            this._chkDesktopIcon.Name = "_chkDesktopIcon";
            this._chkDesktopIcon.Size = new System.Drawing.Size(130, 17);
            this._chkDesktopIcon.TabIndex = 4;
            this._chkDesktopIcon.Text = "Create a desktop icon";
            this._chkDesktopIcon.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Install Location:";
            // 
            // _btnInstall
            // 
            this._btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnInstall.Location = new System.Drawing.Point(121, 278);
            this._btnInstall.Name = "_btnInstall";
            this._btnInstall.Size = new System.Drawing.Size(171, 53);
            this._btnInstall.TabIndex = 16;
            this._btnInstall.Text = "Install";
            this._btnInstall.UseVisualStyleBackColor = true;
            this._btnInstall.Click += new System.EventHandler(this._btnInstall_Click);
            // 
            // _lblOptions
            // 
            this._lblOptions.AutoSize = true;
            this._lblOptions.Location = new System.Drawing.Point(36, 143);
            this._lblOptions.Name = "_lblOptions";
            this._lblOptions.Size = new System.Drawing.Size(52, 13);
            this._lblOptions.TabIndex = 18;
            this._lblOptions.TabStop = true;
            this._lblOptions.Text = "Options...";
            this._lblOptions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lblOptions_LinkClicked);
            // 
            // _btnSelectPath
            // 
            this._btnSelectPath.Location = new System.Drawing.Point(385, 119);
            this._btnSelectPath.Name = "_btnSelectPath";
            this._btnSelectPath.Size = new System.Drawing.Size(28, 21);
            this._btnSelectPath.TabIndex = 15;
            this._btnSelectPath.Text = "...";
            this._btnSelectPath.UseVisualStyleBackColor = true;
            this._btnSelectPath.Click += new System.EventHandler(this._btnSelectPath_Click);
            // 
            // _txtInstallDir
            // 
            this._txtInstallDir.Location = new System.Drawing.Point(39, 120);
            this._txtInstallDir.Name = "_txtInstallDir";
            this._txtInstallDir.Size = new System.Drawing.Size(346, 20);
            this._txtInstallDir.TabIndex = 14;
            this._txtInstallDir.Text = "C:\\Program Files\\Rifle";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(110, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 65);
            this.label1.TabIndex = 13;
            this.label1.Text = "Welcome to the Fulmination Spark© installer.\r\n\r\nClick Install to begin the instal" +
    "lation process.\r\n\r\nAdditionally choose your install options below.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::Spark.Properties.Resources.spark_logo1;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 62);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // OptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._btnExit);
            this.Controls.Add(this._grpOptions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._btnInstall);
            this.Controls.Add(this._lblOptions);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._btnSelectPath);
            this.Controls.Add(this._txtInstallDir);
            this.Controls.Add(this.label1);
            this.Name = "OptionsControl";
            this.Size = new System.Drawing.Size(434, 334);
            this.Load += new System.EventHandler(this.OptionsControl_Load);
            this._grpOptions.ResumeLayout(false);
            this._grpOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnExit;
        private System.Windows.Forms.GroupBox _grpOptions;
        private System.Windows.Forms.CheckBox _chkStartMenuIcon;
        private System.Windows.Forms.CheckBox _chkDesktopIcon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel _lblOptions;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button _btnInstall;
        private System.Windows.Forms.Button _btnSelectPath;
        private System.Windows.Forms.TextBox _txtInstallDir;
        private System.Windows.Forms.Label label1;
    }
}
