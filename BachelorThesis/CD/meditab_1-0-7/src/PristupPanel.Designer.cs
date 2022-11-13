namespace MediTab
{
    partial class PristupPanel
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
            this.cislo_TextBox = new System.Windows.Forms.TextBox();
            this.hloubka_TextBox = new System.Windows.Forms.TextBox();
            this.cm_Label = new System.Windows.Forms.Label();
            this.datum_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.dnu_TextBox = new System.Windows.Forms.TextBox();
            this.vymenit_Button = new System.Windows.Forms.Button();
            this.aktualizuj_Button = new System.Windows.Forms.Button();
            this.vymaz_Button = new System.Windows.Forms.Button();
            this.nazev_ComboBox = new System.Windows.Forms.ComboBox();
            this.specifikace_TextBox = new System.Windows.Forms.TextBox();
            this.stav_TextBox = new System.Windows.Forms.TextBox();
            this.material_ComboBox = new System.Windows.Forms.ComboBox();
            this.umisteni_ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cislo_TextBox
            // 
            this.cislo_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.cislo_TextBox.Location = new System.Drawing.Point(0, 0);
            this.cislo_TextBox.Name = "cislo_TextBox";
            this.cislo_TextBox.ReadOnly = true;
            this.cislo_TextBox.Size = new System.Drawing.Size(51, 22);
            this.cislo_TextBox.TabIndex = 0;
            this.cislo_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cislo_TextBox.Click += new System.EventHandler(this.numericTextBox_Click);
            // 
            // hloubka_TextBox
            // 
            this.hloubka_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.hloubka_TextBox.Location = new System.Drawing.Point(0, 0);
            this.hloubka_TextBox.Name = "hloubka_TextBox";
            this.hloubka_TextBox.ReadOnly = true;
            this.hloubka_TextBox.Size = new System.Drawing.Size(80, 22);
            this.hloubka_TextBox.TabIndex = 0;
            this.hloubka_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hloubka_TextBox.Click += new System.EventHandler(this.numericTextBox_Click);
            // 
            // cm_Label
            // 
            this.cm_Label.AutoSize = true;
            this.cm_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cm_Label.Location = new System.Drawing.Point(0, 0);
            this.cm_Label.Name = "cm_Label";
            this.cm_Label.Size = new System.Drawing.Size(40, 23);
            this.cm_Label.TabIndex = 0;
            this.cm_Label.Text = "cm";
            // 
            // datum_DateTimePicker
            // 
            this.datum_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datum_DateTimePicker.Location = new System.Drawing.Point(0, 0);
            this.datum_DateTimePicker.Name = "datum_DateTimePicker";
            this.datum_DateTimePicker.Size = new System.Drawing.Size(155, 22);
            this.datum_DateTimePicker.TabIndex = 0;
            // 
            // dnu_TextBox
            // 
            this.dnu_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.dnu_TextBox.Location = new System.Drawing.Point(0, 0);
            this.dnu_TextBox.Name = "dnu_TextBox";
            this.dnu_TextBox.ReadOnly = true;
            this.dnu_TextBox.Size = new System.Drawing.Size(80, 22);
            this.dnu_TextBox.TabIndex = 0;
            this.dnu_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // vymenit_Button
            // 
            this.vymenit_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.vymenit_Button.AutoSize = true;
            this.vymenit_Button.Location = new System.Drawing.Point(0, 0);
            this.vymenit_Button.Name = "vymenit_Button";
            this.vymenit_Button.Size = new System.Drawing.Size(75, 23);
            this.vymenit_Button.TabIndex = 0;
            this.vymenit_Button.Text = "Vyměnit";
            this.vymenit_Button.UseVisualStyleBackColor = true;
            this.vymenit_Button.Click += new System.EventHandler(this.vymenit_Button_Click);
            // 
            // aktualizuj_Button
            // 
            this.aktualizuj_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.aktualizuj_Button.AutoSize = true;
            this.aktualizuj_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.aktualizuj_Button.Location = new System.Drawing.Point(0, 0);
            this.aktualizuj_Button.Name = "aktualizuj_Button";
            this.aktualizuj_Button.Size = new System.Drawing.Size(75, 23);
            this.aktualizuj_Button.TabIndex = 0;
            this.aktualizuj_Button.Text = "Aktualizuj";
            this.aktualizuj_Button.UseVisualStyleBackColor = false;
            this.aktualizuj_Button.Click += new System.EventHandler(this.aktualizuj_Button_Click);
            // 
            // vymaz_Button
            // 
            this.vymaz_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.vymaz_Button.AutoSize = true;
            this.vymaz_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.vymaz_Button.Location = new System.Drawing.Point(0, 0);
            this.vymaz_Button.Name = "vymaz_Button";
            this.vymaz_Button.Size = new System.Drawing.Size(75, 23);
            this.vymaz_Button.TabIndex = 0;
            this.vymaz_Button.Text = "Vymaž";
            this.vymaz_Button.UseVisualStyleBackColor = false;
            this.vymaz_Button.Click += new System.EventHandler(this.delete_Button_Click);
            // 
            // nazev_ComboBox
            // 
            this.nazev_ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nazev_ComboBox.FormattingEnabled = true;
            this.nazev_ComboBox.Location = new System.Drawing.Point(0, 0);
            this.nazev_ComboBox.Name = "nazev_ComboBox";
            this.nazev_ComboBox.Size = new System.Drawing.Size(280, 25);
            this.nazev_ComboBox.TabIndex = 0;
            this.nazev_ComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nazev_ComboBox_KeyPress);
            // 
            // specifikace_TextBox
            // 
            this.specifikace_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.specifikace_TextBox.Location = new System.Drawing.Point(0, 0);
            this.specifikace_TextBox.Name = "specifikace_TextBox";
            this.specifikace_TextBox.ReadOnly = true;
            this.specifikace_TextBox.Size = new System.Drawing.Size(280, 22);
            this.specifikace_TextBox.TabIndex = 0;
            // 
            // stav_TextBox
            // 
            this.stav_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.stav_TextBox.Location = new System.Drawing.Point(0, 0);
            this.stav_TextBox.Name = "stav_TextBox";
            this.stav_TextBox.Size = new System.Drawing.Size(130, 22);
            this.stav_TextBox.TabIndex = 0;
            // 
            // material_ComboBox
            // 
            this.material_ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.material_ComboBox.FormattingEnabled = true;
            this.material_ComboBox.Location = new System.Drawing.Point(0, 0);
            this.material_ComboBox.Name = "material_ComboBox";
            this.material_ComboBox.Size = new System.Drawing.Size(250, 25);
            this.material_ComboBox.TabIndex = 0;
            // 
            // umisteni_ComboBox
            // 
            this.umisteni_ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.umisteni_ComboBox.FormattingEnabled = true;
            this.umisteni_ComboBox.Location = new System.Drawing.Point(0, 0);
            this.umisteni_ComboBox.Name = "umisteni_ComboBox";
            this.umisteni_ComboBox.Size = new System.Drawing.Size(250, 25);
            this.umisteni_ComboBox.TabIndex = 0;
            // 
            // PristupPanel
            // 
            this.AutoSize = true;
            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox cislo_TextBox;
        private System.Windows.Forms.TextBox hloubka_TextBox;
        private System.Windows.Forms.Label cm_Label;
        private System.Windows.Forms.DateTimePicker datum_DateTimePicker;
        private System.Windows.Forms.TextBox dnu_TextBox;
        private System.Windows.Forms.Button vymenit_Button;
        private System.Windows.Forms.Button aktualizuj_Button;
        private System.Windows.Forms.Button vymaz_Button;
        private System.Windows.Forms.ComboBox nazev_ComboBox;
        private System.Windows.Forms.TextBox specifikace_TextBox;
        private System.Windows.Forms.TextBox stav_TextBox;
        private System.Windows.Forms.ComboBox material_ComboBox;
        private System.Windows.Forms.ComboBox umisteni_ComboBox;


    }
}
