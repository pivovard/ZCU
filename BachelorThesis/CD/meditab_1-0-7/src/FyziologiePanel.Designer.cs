namespace MediTab
{
    partial class FyziologiePanel
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
            this.datum_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.teplota_TextBox = new System.Windows.Forms.TextBox();
            this.stk_TextBox = new System.Windows.Forms.TextBox();
            this.dtk_TextBox = new System.Windows.Forms.TextBox();
            this.tep_TextBox = new System.Windows.Forms.TextBox();
            this.dech_TextBox = new System.Windows.Forms.TextBox();
            this.upravit_Button = new System.Windows.Forms.Button();
            this.smazat_Button = new System.Windows.Forms.Button();
            this.pridat_Button = new System.Windows.Forms.Button();
            this.hmotnost_TextBox = new System.Windows.Forms.TextBox();
            this.vyska_TextBox = new System.Windows.Forms.TextBox();
            this.bmi_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // datum_DateTimePicker
            // 
            this.datum_DateTimePicker.CustomFormat = "dd.MM.yyyy HH:mm";
            this.datum_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datum_DateTimePicker.Location = new System.Drawing.Point(0, 0);
            this.datum_DateTimePicker.Name = "datum_DateTimePicker";
            this.datum_DateTimePicker.Size = new System.Drawing.Size(220, 20);
            this.datum_DateTimePicker.TabIndex = 0;
            // 
            // teplota_TextBox
            // 
            this.teplota_TextBox.Location = new System.Drawing.Point(0, 0);
            this.teplota_TextBox.Name = "teplota_TextBox";
            this.teplota_TextBox.Size = new System.Drawing.Size(80, 20);
            this.teplota_TextBox.TabIndex = 0;
            this.teplota_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.teplota_TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.teplota_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // stk_TextBox
            // 
            this.stk_TextBox.Location = new System.Drawing.Point(0, 0);
            this.stk_TextBox.Name = "stk_TextBox";
            this.stk_TextBox.Size = new System.Drawing.Size(70, 20);
            this.stk_TextBox.TabIndex = 0;
            this.stk_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.stk_TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.stk_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // dtk_TextBox
            // 
            this.dtk_TextBox.Location = new System.Drawing.Point(0, 0);
            this.dtk_TextBox.Name = "dtk_TextBox";
            this.dtk_TextBox.Size = new System.Drawing.Size(70, 20);
            this.dtk_TextBox.TabIndex = 0;
            this.dtk_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dtk_TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.dtk_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // tep_TextBox
            // 
            this.tep_TextBox.Location = new System.Drawing.Point(0, 0);
            this.tep_TextBox.Name = "tep_TextBox";
            this.tep_TextBox.Size = new System.Drawing.Size(70, 20);
            this.tep_TextBox.TabIndex = 0;
            this.tep_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tep_TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.tep_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // dech_TextBox
            // 
            this.dech_TextBox.Location = new System.Drawing.Point(0, 0);
            this.dech_TextBox.Name = "dech_TextBox";
            this.dech_TextBox.Size = new System.Drawing.Size(70, 20);
            this.dech_TextBox.TabIndex = 0;
            this.dech_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dech_TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.dech_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // upravit_Button
            // 
            this.upravit_Button.AutoSize = true;
            this.upravit_Button.Location = new System.Drawing.Point(0, 0);
            this.upravit_Button.Name = "upravit_Button";
            this.upravit_Button.Size = new System.Drawing.Size(75, 23);
            this.upravit_Button.TabIndex = 0;
            this.upravit_Button.Text = "Upravit";
            this.upravit_Button.UseVisualStyleBackColor = true;
            this.upravit_Button.Click += new System.EventHandler(this.upravit_Button_Click);
            // 
            // smazat_Button
            // 
            this.smazat_Button.AutoSize = true;
            this.smazat_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.smazat_Button.Location = new System.Drawing.Point(0, 0);
            this.smazat_Button.Name = "smazat_Button";
            this.smazat_Button.Size = new System.Drawing.Size(75, 23);
            this.smazat_Button.TabIndex = 0;
            this.smazat_Button.Text = "Smazat";
            this.smazat_Button.UseVisualStyleBackColor = false;
            this.smazat_Button.Click += new System.EventHandler(this.smazat_Button_Click);
            // 
            // pridat_Button
            // 
            this.pridat_Button.AutoSize = true;
            this.pridat_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.pridat_Button.Location = new System.Drawing.Point(0, 0);
            this.pridat_Button.Name = "pridat_Button";
            this.pridat_Button.Size = new System.Drawing.Size(75, 23);
            this.pridat_Button.TabIndex = 0;
            this.pridat_Button.Text = "Přidat";
            this.pridat_Button.UseVisualStyleBackColor = false;
            this.pridat_Button.Click += new System.EventHandler(this.pridat_Button_Click);
            // 
            // hmotnost_TextBox
            // 
            this.hmotnost_TextBox.Location = new System.Drawing.Point(0, 0);
            this.hmotnost_TextBox.Name = "hmotnost_TextBox";
            this.hmotnost_TextBox.Size = new System.Drawing.Size(80, 20);
            this.hmotnost_TextBox.TabIndex = 0;
            this.hmotnost_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hmotnost_TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.hmotnost_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // vyska_TextBox
            // 
            this.vyska_TextBox.Location = new System.Drawing.Point(0, 0);
            this.vyska_TextBox.Name = "vyska_TextBox";
            this.vyska_TextBox.Size = new System.Drawing.Size(80, 20);
            this.vyska_TextBox.TabIndex = 0;
            this.vyska_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.vyska_TextBox.Click += new System.EventHandler(this.TextBox_Click);
            this.vyska_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // bmi_TextBox
            // 
            this.bmi_TextBox.Location = new System.Drawing.Point(0, 0);
            this.bmi_TextBox.Name = "bmi_TextBox";
            this.bmi_TextBox.ReadOnly = true;
            this.bmi_TextBox.Size = new System.Drawing.Size(80, 20);
            this.bmi_TextBox.TabIndex = 0;
            this.bmi_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FyziologiePanel
            // 
            this.AutoSize = true;
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker datum_DateTimePicker;
        private System.Windows.Forms.TextBox teplota_TextBox;
        private System.Windows.Forms.TextBox stk_TextBox;
        private System.Windows.Forms.TextBox dtk_TextBox;
        private System.Windows.Forms.TextBox tep_TextBox;
        private System.Windows.Forms.TextBox dech_TextBox;
        private System.Windows.Forms.Button upravit_Button;
        private System.Windows.Forms.Button smazat_Button;
        private System.Windows.Forms.Button pridat_Button;
        private System.Windows.Forms.TextBox hmotnost_TextBox;
        private System.Windows.Forms.TextBox vyska_TextBox;
        private System.Windows.Forms.TextBox bmi_TextBox;
    }
}
