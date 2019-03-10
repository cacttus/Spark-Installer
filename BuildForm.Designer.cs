namespace Spark
{
    partial class BuildForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._btnCancel = new System.Windows.Forms.Button();
            this._pgbBuildProgress = new System.Windows.Forms.ProgressBar();
            this._lblBuildDetails = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Building installer...";
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(220, 80);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 1;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _pgbBuildProgress
            // 
            this._pgbBuildProgress.Location = new System.Drawing.Point(15, 35);
            this._pgbBuildProgress.Name = "_pgbBuildProgress";
            this._pgbBuildProgress.Size = new System.Drawing.Size(280, 11);
            this._pgbBuildProgress.TabIndex = 2;
            // 
            // _lblBuildDetails
            // 
            this._lblBuildDetails.AutoSize = true;
            this._lblBuildDetails.Location = new System.Drawing.Point(15, 53);
            this._lblBuildDetails.Name = "_lblBuildDetails";
            this._lblBuildDetails.Size = new System.Drawing.Size(53, 13);
            this._lblBuildDetails.TabIndex = 3;
            this._lblBuildDetails.Text = "Building...";
            // 
            // BuildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 115);
            this.Controls.Add(this._lblBuildDetails);
            this.Controls.Add(this._pgbBuildProgress);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "BuildForm";
            this.Text = "BuildForm";
            this.Load += new System.EventHandler(this.BuildForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.ProgressBar _pgbBuildProgress;
        private System.Windows.Forms.Label _lblBuildDetails;
    }
}