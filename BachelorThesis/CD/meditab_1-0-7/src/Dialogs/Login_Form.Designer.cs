namespace MediTab
{
    partial class Login_Form
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
            this.login_TextBox = new System.Windows.Forms.TextBox();
            this.ok_Button = new System.Windows.Forms.Button();
            this.zrusit_Button = new System.Windows.Forms.Button();
            this.chyba_Label = new System.Windows.Forms.Label();
            this.pass_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // login_TextBox
            // 
            this.login_TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.login_TextBox.Location = new System.Drawing.Point(144, 39);
            this.login_TextBox.Margin = new System.Windows.Forms.Padding(7);
            this.login_TextBox.Name = "login_TextBox";
            this.login_TextBox.Size = new System.Drawing.Size(237, 41);
            this.login_TextBox.TabIndex = 0;
            this.login_TextBox.Click += new System.EventHandler(this.login_TextBox_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.ok_Button.Location = new System.Drawing.Point(17, 139);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(7);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(175, 51);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = false;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // zrusit_Button
            // 
            this.zrusit_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.zrusit_Button.Location = new System.Drawing.Point(206, 139);
            this.zrusit_Button.Margin = new System.Windows.Forms.Padding(7);
            this.zrusit_Button.Name = "zrusit_Button";
            this.zrusit_Button.Size = new System.Drawing.Size(175, 51);
            this.zrusit_Button.TabIndex = 3;
            this.zrusit_Button.Text = "Zrušit";
            this.zrusit_Button.UseVisualStyleBackColor = false;
            this.zrusit_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // chyba_Label
            // 
            this.chyba_Label.AutoSize = true;
            this.chyba_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chyba_Label.ForeColor = System.Drawing.Color.Red;
            this.chyba_Label.Location = new System.Drawing.Point(12, 6);
            this.chyba_Label.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.chyba_Label.Name = "chyba_Label";
            this.chyba_Label.Size = new System.Drawing.Size(0, 31);
            this.chyba_Label.TabIndex = 3;
            // 
            // pass_TextBox
            // 
            this.pass_TextBox.Location = new System.Drawing.Point(144, 89);
            this.pass_TextBox.Margin = new System.Windows.Forms.Padding(7);
            this.pass_TextBox.Name = "pass_TextBox";
            this.pass_TextBox.PasswordChar = '*';
            this.pass_TextBox.Size = new System.Drawing.Size(237, 41);
            this.pass_TextBox.TabIndex = 1;
            this.pass_TextBox.Click += new System.EventHandler(this.pass_TextBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 36);
            this.label1.TabIndex = 5;
            this.label1.Text = "Uživatel:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 36);
            this.label2.TabIndex = 6;
            this.label2.Text = "Heslo:";
            // 
            // Login_Form
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 36F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 212);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pass_TextBox);
            this.Controls.Add(this.chyba_Label);
            this.Controls.Add(this.zrusit_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.login_TextBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Přihlášení";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox login_TextBox;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button zrusit_Button;
        private System.Windows.Forms.Label chyba_Label;
        public System.Windows.Forms.TextBox pass_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}