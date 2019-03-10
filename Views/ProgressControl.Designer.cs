namespace Spark
{
    partial class ProgressControl
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
            this._pgbInstallProgress = new System.Windows.Forms.ProgressBar();
            this._lblInstallDetails = new System.Windows.Forms.Label();
            this._lblInstallInfo = new System.Windows.Forms.Label();
            this._btnCancel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // _pgbInstallProgress
            // 
            this._pgbInstallProgress.Location = new System.Drawing.Point(25, 152);
            this._pgbInstallProgress.Name = "_pgbInstallProgress";
            this._pgbInstallProgress.Size = new System.Drawing.Size(387, 27);
            this._pgbInstallProgress.TabIndex = 0;
            // 
            // _lblInstallDetails
            // 
            this._lblInstallDetails.AutoSize = true;
            this._lblInstallDetails.Location = new System.Drawing.Point(22, 182);
            this._lblInstallDetails.Name = "_lblInstallDetails";
            this._lblInstallDetails.Size = new System.Drawing.Size(54, 13);
            this._lblInstallDetails.TabIndex = 1;
            this._lblInstallDetails.Text = "Installing..";
            // 
            // _lblInstallInfo
            // 
            this._lblInstallInfo.AutoSize = true;
            this._lblInstallInfo.Location = new System.Drawing.Point(92, 112);
            this._lblInstallInfo.Name = "_lblInstallInfo";
            this._lblInstallInfo.Size = new System.Drawing.Size(234, 13);
            this._lblInstallInfo.TabIndex = 2;
            this._lblInstallInfo.Text = "Please wait while the installer finishes setting up.\r\n";
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(337, 306);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
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
            // ProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._lblInstallInfo);
            this.Controls.Add(this._lblInstallDetails);
            this.Controls.Add(this._pgbInstallProgress);
            this.Name = "ProgressControl";
            this.Size = new System.Drawing.Size(429, 343);
            this.Load += new System.EventHandler(this.ProgressControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar _pgbInstallProgress;
        private System.Windows.Forms.Label _lblInstallDetails;
        private System.Windows.Forms.Label _lblInstallInfo;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
