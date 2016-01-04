namespace AutoUpdater
{
    partial class UpdateProgress
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
            this.progressBarUpdate = new System.Windows.Forms.ProgressBar();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.downloadLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPerc = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.labelDownloaded = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarUpdate
            // 
            this.progressBarUpdate.Location = new System.Drawing.Point(11, 158);
            this.progressBarUpdate.Name = "progressBarUpdate";
            this.progressBarUpdate.Size = new System.Drawing.Size(354, 13);
            this.progressBarUpdate.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(259, 25);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(223, 126);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // downloadLabel
            // 
            this.downloadLabel.AutoSize = true;
            this.downloadLabel.Location = new System.Drawing.Point(9, 121);
            this.downloadLabel.Name = "downloadLabel";
            this.downloadLabel.Size = new System.Drawing.Size(71, 13);
            this.downloadLabel.TabIndex = 2;
            this.downloadLabel.Text = "Descargando";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(197, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cambios";
            // 
            // labelPerc
            // 
            this.labelPerc.AutoSize = true;
            this.labelPerc.Location = new System.Drawing.Point(372, 158);
            this.labelPerc.Name = "labelPerc";
            this.labelPerc.Size = new System.Drawing.Size(35, 13);
            this.labelPerc.TabIndex = 4;
            this.labelPerc.Text = "label2";
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(421, 158);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(35, 13);
            this.labelSpeed.TabIndex = 5;
            this.labelSpeed.Text = "label3";
            // 
            // labelDownloaded
            // 
            this.labelDownloaded.AutoSize = true;
            this.labelDownloaded.Location = new System.Drawing.Point(12, 138);
            this.labelDownloaded.Name = "labelDownloaded";
            this.labelDownloaded.Size = new System.Drawing.Size(35, 13);
            this.labelDownloaded.TabIndex = 6;
            this.labelDownloaded.Text = "label2";
            // 
            // UpdateProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 178);
            this.ControlBox = false;
            this.Controls.Add(this.labelDownloaded);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.labelPerc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.downloadLabel);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.progressBarUpdate);
            this.Name = "UpdateProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateProgress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarUpdate;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label downloadLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPerc;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Label labelDownloaded;
    }
}