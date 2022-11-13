namespace MediTab
{
    partial class Loading_Form
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
            this.loading_Label = new System.Windows.Forms.Label();
            this.info_Label = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // loading_Label
            // 
            this.loading_Label.AutoSize = true;
            this.loading_Label.BackColor = System.Drawing.SystemColors.Window;
            this.loading_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loading_Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.loading_Label.Location = new System.Drawing.Point(12, 9);
            this.loading_Label.Name = "loading_Label";
            this.loading_Label.Size = new System.Drawing.Size(209, 46);
            this.loading_Label.TabIndex = 0;
            this.loading_Label.Text = "Načítání...";
            // 
            // info_Label
            // 
            this.info_Label.AutoSize = true;
            this.info_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.info_Label.Location = new System.Drawing.Point(15, 55);
            this.info_Label.Name = "info_Label";
            this.info_Label.Size = new System.Drawing.Size(148, 36);
            this.info_Label.TabIndex = 1;
            this.info_Label.Text = "Initializing";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 108);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar.MarqueeAnimationSpeed = 1;
            this.progressBar.Maximum = 6;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(449, 31);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 2;
            this.progressBar.Value = 2;
            // 
            // Loading_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(500, 150);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.info_Label);
            this.Controls.Add(this.loading_Label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Loading_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Načítání";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loading_Label;
        private System.Windows.Forms.Label info_Label;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}