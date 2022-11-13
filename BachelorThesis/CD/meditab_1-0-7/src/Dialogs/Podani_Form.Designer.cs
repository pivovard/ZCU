namespace MediTab
{
    partial class Podani_Form
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
            this.potvrdit_Button = new System.Windows.Forms.Button();
            this.zrusit_Button = new System.Windows.Forms.Button();
            this.mnozstvi_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.poznamka_TextBox = new System.Windows.Forms.TextBox();
            this.nepodat_Button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.up_Button = new System.Windows.Forms.Button();
            this.down_Button = new System.Windows.Forms.Button();
            this.nazev_Label = new System.Windows.Forms.Label();
            this.davkovani_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mnozstvi_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // potvrdit_Button
            // 
            this.potvrdit_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.potvrdit_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.potvrdit_Button.Location = new System.Drawing.Point(18, 262);
            this.potvrdit_Button.Margin = new System.Windows.Forms.Padding(7);
            this.potvrdit_Button.Name = "potvrdit_Button";
            this.potvrdit_Button.Size = new System.Drawing.Size(175, 51);
            this.potvrdit_Button.TabIndex = 1;
            this.potvrdit_Button.Text = "Potvrdit";
            this.potvrdit_Button.UseVisualStyleBackColor = false;
            // 
            // zrusit_Button
            // 
            this.zrusit_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.zrusit_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.zrusit_Button.Location = new System.Drawing.Point(388, 262);
            this.zrusit_Button.Margin = new System.Windows.Forms.Padding(7);
            this.zrusit_Button.Name = "zrusit_Button";
            this.zrusit_Button.Size = new System.Drawing.Size(175, 51);
            this.zrusit_Button.TabIndex = 2;
            this.zrusit_Button.Text = "Zrušit";
            this.zrusit_Button.UseVisualStyleBackColor = false;
            // 
            // mnozstvi_NumericUpDown
            // 
            this.mnozstvi_NumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnozstvi_NumericUpDown.Location = new System.Drawing.Point(18, 137);
            this.mnozstvi_NumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.mnozstvi_NumericUpDown.Name = "mnozstvi_NumericUpDown";
            this.mnozstvi_NumericUpDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mnozstvi_NumericUpDown.Size = new System.Drawing.Size(364, 35);
            this.mnozstvi_NumericUpDown.TabIndex = 0;
            this.mnozstvi_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mnozstvi_NumericUpDown.Click += new System.EventHandler(this.mnozstvi_NumericUpDown_Click);
            this.mnozstvi_NumericUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mnozstvi_NumericUpDown_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "Zadej podání:";
            // 
            // poznamka_TextBox
            // 
            this.poznamka_TextBox.Location = new System.Drawing.Point(18, 215);
            this.poznamka_TextBox.Name = "poznamka_TextBox";
            this.poznamka_TextBox.Size = new System.Drawing.Size(545, 31);
            this.poznamka_TextBox.TabIndex = 11;
            this.poznamka_TextBox.Click += new System.EventHandler(this.poznamka_TextBox_Click);
            // 
            // nepodat_Button
            // 
            this.nepodat_Button.BackColor = System.Drawing.Color.MediumPurple;
            this.nepodat_Button.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.nepodat_Button.Location = new System.Drawing.Point(203, 262);
            this.nepodat_Button.Name = "nepodat_Button";
            this.nepodat_Button.Size = new System.Drawing.Size(175, 51);
            this.nepodat_Button.TabIndex = 12;
            this.nepodat_Button.Text = "Nepodat";
            this.nepodat_Button.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 25);
            this.label2.TabIndex = 13;
            this.label2.Text = "Poznámka:";
            // 
            // up_Button
            // 
            this.up_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.up_Button.Location = new System.Drawing.Point(388, 122);
            this.up_Button.Name = "up_Button";
            this.up_Button.Size = new System.Drawing.Size(57, 33);
            this.up_Button.TabIndex = 14;
            this.up_Button.Text = "+";
            this.up_Button.UseVisualStyleBackColor = true;
            this.up_Button.Click += new System.EventHandler(this.up_Button_Click);
            // 
            // down_Button
            // 
            this.down_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.down_Button.Location = new System.Drawing.Point(388, 157);
            this.down_Button.Name = "down_Button";
            this.down_Button.Size = new System.Drawing.Size(57, 33);
            this.down_Button.TabIndex = 15;
            this.down_Button.Text = "-";
            this.down_Button.UseVisualStyleBackColor = true;
            this.down_Button.Click += new System.EventHandler(this.down_Button_Click);
            // 
            // nazev_Label
            // 
            this.nazev_Label.AutoSize = true;
            this.nazev_Label.Location = new System.Drawing.Point(15, 9);
            this.nazev_Label.Name = "nazev_Label";
            this.nazev_Label.Size = new System.Drawing.Size(73, 25);
            this.nazev_Label.TabIndex = 16;
            this.nazev_Label.Text = "Nazev";
            // 
            // davkovani_Label
            // 
            this.davkovani_Label.AutoSize = true;
            this.davkovani_Label.Location = new System.Drawing.Point(15, 40);
            this.davkovani_Label.Name = "davkovani_Label";
            this.davkovani_Label.Size = new System.Drawing.Size(113, 25);
            this.davkovani_Label.TabIndex = 17;
            this.davkovani_Label.Text = "Davkovani";
            // 
            // Podani_Form
            // 
            this.AcceptButton = this.potvrdit_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.zrusit_Button;
            this.ClientSize = new System.Drawing.Size(576, 332);
            this.Controls.Add(this.davkovani_Label);
            this.Controls.Add(this.nazev_Label);
            this.Controls.Add(this.down_Button);
            this.Controls.Add(this.up_Button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nepodat_Button);
            this.Controls.Add(this.poznamka_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mnozstvi_NumericUpDown);
            this.Controls.Add(this.zrusit_Button);
            this.Controls.Add(this.potvrdit_Button);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Podani_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Podání";
            ((System.ComponentModel.ISupportInitialize)(this.mnozstvi_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button potvrdit_Button;
        private System.Windows.Forms.Button zrusit_Button;
        public System.Windows.Forms.NumericUpDown mnozstvi_NumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button nepodat_Button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button up_Button;
        private System.Windows.Forms.Button down_Button;
        public System.Windows.Forms.TextBox poznamka_TextBox;
        private System.Windows.Forms.Label nazev_Label;
        private System.Windows.Forms.Label davkovani_Label;
    }
}