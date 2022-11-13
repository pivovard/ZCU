namespace MediTab
{
    partial class Lek_Form
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
            this.pridej_Button = new System.Windows.Forms.Button();
            this.zrusit_Button = new System.Windows.Forms.Button();
            this.lek_ComboBox = new System.Windows.Forms.ComboBox();
            this.davkovani_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.vyhledat_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pridej_Button
            // 
            this.pridej_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.pridej_Button.Location = new System.Drawing.Point(498, 185);
            this.pridej_Button.Margin = new System.Windows.Forms.Padding(7);
            this.pridej_Button.Name = "pridej_Button";
            this.pridej_Button.Size = new System.Drawing.Size(175, 51);
            this.pridej_Button.TabIndex = 0;
            this.pridej_Button.Text = "Přidat lék";
            this.pridej_Button.UseVisualStyleBackColor = false;
            this.pridej_Button.Click += new System.EventHandler(this.pridej_Button_Click);
            // 
            // zrusit_Button
            // 
            this.zrusit_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.zrusit_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.zrusit_Button.Location = new System.Drawing.Point(687, 185);
            this.zrusit_Button.Margin = new System.Windows.Forms.Padding(7);
            this.zrusit_Button.Name = "zrusit_Button";
            this.zrusit_Button.Size = new System.Drawing.Size(175, 51);
            this.zrusit_Button.TabIndex = 1;
            this.zrusit_Button.Text = "Zrušit";
            this.zrusit_Button.UseVisualStyleBackColor = false;
            this.zrusit_Button.Click += new System.EventHandler(this.zrusit_Button_Click);
            // 
            // lek_ComboBox
            // 
            this.lek_ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lek_ComboBox.FormattingEnabled = true;
            this.lek_ComboBox.Location = new System.Drawing.Point(18, 48);
            this.lek_ComboBox.Name = "lek_ComboBox";
            this.lek_ComboBox.Size = new System.Drawing.Size(844, 44);
            this.lek_ComboBox.TabIndex = 2;
            this.lek_ComboBox.Click += new System.EventHandler(this.lek_ComboBox_Click);
            this.lek_ComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lek_ComboBox_KeyPress);
            // 
            // davkovani_TextBox
            // 
            this.davkovani_TextBox.Location = new System.Drawing.Point(18, 134);
            this.davkovani_TextBox.Name = "davkovani_TextBox";
            this.davkovani_TextBox.Size = new System.Drawing.Size(844, 41);
            this.davkovani_TextBox.TabIndex = 3;
            this.davkovani_TextBox.Click += new System.EventHandler(this.davkovani_TextBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 36);
            this.label1.TabIndex = 4;
            this.label1.Text = "Název léku";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 36);
            this.label2.TabIndex = 5;
            this.label2.Text = "Dávkování";
            // 
            // vyhledat_Button
            // 
            this.vyhledat_Button.Enabled = false;
            this.vyhledat_Button.Location = new System.Drawing.Point(309, 185);
            this.vyhledat_Button.Margin = new System.Windows.Forms.Padding(7);
            this.vyhledat_Button.Name = "vyhledat_Button";
            this.vyhledat_Button.Size = new System.Drawing.Size(175, 51);
            this.vyhledat_Button.TabIndex = 6;
            this.vyhledat_Button.Text = "Vyhledej";
            this.vyhledat_Button.UseVisualStyleBackColor = true;
            this.vyhledat_Button.Visible = false;
            this.vyhledat_Button.Click += new System.EventHandler(this.vyhledat_Button_Click);
            // 
            // Lek_Form
            // 
            this.AcceptButton = this.pridej_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 36F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.zrusit_Button;
            this.ClientSize = new System.Drawing.Size(881, 270);
            this.Controls.Add(this.vyhledat_Button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.davkovani_TextBox);
            this.Controls.Add(this.lek_ComboBox);
            this.Controls.Add(this.zrusit_Button);
            this.Controls.Add(this.pridej_Button);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Lek_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Přidání léku";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pridej_Button;
        private System.Windows.Forms.Button zrusit_Button;
        public System.Windows.Forms.ComboBox lek_ComboBox;
        public System.Windows.Forms.TextBox davkovani_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button vyhledat_Button;
    }
}