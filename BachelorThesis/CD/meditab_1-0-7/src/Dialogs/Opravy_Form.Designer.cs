namespace MediTab
{
    partial class Opravy_Form
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
            this.zavrit_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // zavrit_Button
            // 
            this.zavrit_Button.AutoSize = true;
            this.zavrit_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.zavrit_Button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.zavrit_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zavrit_Button.Location = new System.Drawing.Point(0, 440);
            this.zavrit_Button.Margin = new System.Windows.Forms.Padding(4);
            this.zavrit_Button.Name = "zavrit_Button";
            this.zavrit_Button.Size = new System.Drawing.Size(1179, 46);
            this.zavrit_Button.TabIndex = 0;
            this.zavrit_Button.Text = "Zavřít";
            this.zavrit_Button.UseVisualStyleBackColor = false;
            this.zavrit_Button.Click += new System.EventHandler(this.zavrit_Button_Click);
            // 
            // Opravy_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 486);
            this.Controls.Add(this.zavrit_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Opravy_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opravy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button zavrit_Button;
        private System.Windows.Forms.Panel panel;
    }
}