namespace MediTab
{
    partial class Main_Form
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.prihlasen_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.spaceToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.verze_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.odhlasit_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.napoveda_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pacienti_ListView = new System.Windows.Forms.ListView();
            this.prijmeni_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.jmeno_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.identifikace_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chorobopis_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.konec_Button = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prihlasen_ToolStripStatusLabel,
            this.spaceToolStripStatusLabel,
            this.verze_ToolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 487);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip.Size = new System.Drawing.Size(1299, 43);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "Status";
            // 
            // prihlasen_ToolStripStatusLabel
            // 
            this.prihlasen_ToolStripStatusLabel.Name = "prihlasen_ToolStripStatusLabel";
            this.prihlasen_ToolStripStatusLabel.Size = new System.Drawing.Size(136, 38);
            this.prihlasen_ToolStripStatusLabel.Text = "Přihlášen:";
            // 
            // spaceToolStripStatusLabel
            // 
            this.spaceToolStripStatusLabel.Name = "spaceToolStripStatusLabel";
            this.spaceToolStripStatusLabel.Size = new System.Drawing.Size(956, 38);
            this.spaceToolStripStatusLabel.Spring = true;
            // 
            // verze_ToolStripStatusLabel
            // 
            this.verze_ToolStripStatusLabel.Name = "verze_ToolStripStatusLabel";
            this.verze_ToolStripStatusLabel.Size = new System.Drawing.Size(187, 38);
            this.verze_ToolStripStatusLabel.Text = "MediTab 1.0.0";
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.odhlasit_ToolStripMenuItem,
            this.napoveda_ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1299, 46);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "Menu";
            // 
            // odhlasit_ToolStripMenuItem
            // 
            this.odhlasit_ToolStripMenuItem.Name = "odhlasit_ToolStripMenuItem";
            this.odhlasit_ToolStripMenuItem.Size = new System.Drawing.Size(131, 42);
            this.odhlasit_ToolStripMenuItem.Text = "Odhlásit";
            this.odhlasit_ToolStripMenuItem.Click += new System.EventHandler(this.odhlasit_ToolStripMenuItem_Click);
            // 
            // napoveda_ToolStripMenuItem
            // 
            this.napoveda_ToolStripMenuItem.Name = "napoveda_ToolStripMenuItem";
            this.napoveda_ToolStripMenuItem.Size = new System.Drawing.Size(154, 42);
            this.napoveda_ToolStripMenuItem.Text = "Nápověda";
            this.napoveda_ToolStripMenuItem.Click += new System.EventHandler(this.napoveda_ToolStripMenuItem_Click);
            // 
            // pacienti_ListView
            // 
            this.pacienti_ListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.pacienti_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.prijmeni_ColumnHeader,
            this.jmeno_ColumnHeader,
            this.identifikace_ColumnHeader,
            this.chorobopis_ColumnHeader});
            this.pacienti_ListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pacienti_ListView.Enabled = false;
            this.pacienti_ListView.Font = new System.Drawing.Font("Times New Roman", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pacienti_ListView.FullRowSelect = true;
            this.pacienti_ListView.GridLines = true;
            this.pacienti_ListView.Location = new System.Drawing.Point(0, 46);
            this.pacienti_ListView.Margin = new System.Windows.Forms.Padding(4);
            this.pacienti_ListView.MultiSelect = false;
            this.pacienti_ListView.Name = "pacienti_ListView";
            this.pacienti_ListView.Size = new System.Drawing.Size(1196, 441);
            this.pacienti_ListView.TabIndex = 2;
            this.pacienti_ListView.UseCompatibleStateImageBehavior = false;
            this.pacienti_ListView.View = System.Windows.Forms.View.Details;
            this.pacienti_ListView.Click += new System.EventHandler(this.pacienti_ListView_Click);
            // 
            // prijmeni_ColumnHeader
            // 
            this.prijmeni_ColumnHeader.Text = "Příjmení";
            this.prijmeni_ColumnHeader.Width = 300;
            // 
            // jmeno_ColumnHeader
            // 
            this.jmeno_ColumnHeader.Text = "Jméno";
            this.jmeno_ColumnHeader.Width = 300;
            // 
            // identifikace_ColumnHeader
            // 
            this.identifikace_ColumnHeader.Text = "Identifikace";
            this.identifikace_ColumnHeader.Width = 200;
            // 
            // chorobopis_ColumnHeader
            // 
            this.chorobopis_ColumnHeader.Text = "Chorobopis";
            this.chorobopis_ColumnHeader.Width = 250;
            // 
            // konec_Button
            // 
            this.konec_Button.AutoSize = true;
            this.konec_Button.Dock = System.Windows.Forms.DockStyle.Right;
            this.konec_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.konec_Button.Location = new System.Drawing.Point(1196, 46);
            this.konec_Button.Margin = new System.Windows.Forms.Padding(4);
            this.konec_Button.Name = "konec_Button";
            this.konec_Button.Size = new System.Drawing.Size(103, 441);
            this.konec_Button.TabIndex = 3;
            this.konec_Button.Text = "Konec";
            this.konec_Button.UseVisualStyleBackColor = true;
            this.konec_Button.Click += new System.EventHandler(this.konec_Button_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1299, 530);
            this.Controls.Add(this.pacienti_ListView);
            this.Controls.Add(this.konec_Button);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main_Form";
            this.Text = "MediTab";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_Form_FormClosed);
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel verze_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel prihlasen_ToolStripStatusLabel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem napoveda_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel spaceToolStripStatusLabel;
        private System.Windows.Forms.ListView pacienti_ListView;
        private System.Windows.Forms.ColumnHeader chorobopis_ColumnHeader;
        private System.Windows.Forms.ColumnHeader prijmeni_ColumnHeader;
        private System.Windows.Forms.ColumnHeader jmeno_ColumnHeader;
        private System.Windows.Forms.ColumnHeader identifikace_ColumnHeader;
        private System.Windows.Forms.Button konec_Button;
        private System.Windows.Forms.ToolStripMenuItem odhlasit_ToolStripMenuItem;
    }
}

