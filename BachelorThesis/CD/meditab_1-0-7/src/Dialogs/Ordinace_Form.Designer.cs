namespace MediTab
{
    partial class Ordinace_Form
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttons_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pridat_Button = new System.Windows.Forms.Button();
            this.smazat_Button = new System.Windows.Forms.Button();
            this.lek_Label = new System.Windows.Forms.Label();
            this.ordinace_ListView = new System.Windows.Forms.ListView();
            this.zacatek_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.konec_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.stav_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.davka_ColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detail_FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.zacatek_Label = new System.Windows.Forms.Label();
            this.konec_CheckBox = new System.Windows.Forms.CheckBox();
            this.datumOd_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.datumDo_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.stav_Label = new System.Windows.Forms.Label();
            this.stav_ComboBox = new System.Windows.Forms.ComboBox();
            this.mnozstvi_Label = new System.Windows.Forms.Label();
            this.davka_TextBox = new System.Windows.Forms.TextBox();
            this.jednotky_Label = new System.Windows.Forms.Label();
            this.jednotky_ComboBox = new System.Windows.Forms.ComboBox();
            this.pozn_Label = new System.Windows.Forms.Label();
            this.poznamka_TextBox = new System.Windows.Forms.TextBox();
            this.kontinual_CheckBox = new System.Windows.Forms.CheckBox();
            this.podavajici_Label = new System.Windows.Forms.Label();
            this.podat_Button = new System.Windows.Forms.Button();
            this.nepodat_Button = new System.Windows.Forms.Button();
            this.zavrit_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.buttons_FlowLayoutPanel.SuspendLayout();
            this.detail_FlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttons_FlowLayoutPanel, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lek_Label, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.ordinace_ListView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.detail_FlowLayoutPanel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.zavrit_Button, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1048, 502);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttons_FlowLayoutPanel
            // 
            this.buttons_FlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttons_FlowLayoutPanel.AutoSize = true;
            this.buttons_FlowLayoutPanel.Controls.Add(this.pridat_Button);
            this.buttons_FlowLayoutPanel.Controls.Add(this.smazat_Button);
            this.buttons_FlowLayoutPanel.Location = new System.Drawing.Point(0, 437);
            this.buttons_FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttons_FlowLayoutPanel.Name = "buttons_FlowLayoutPanel";
            this.buttons_FlowLayoutPanel.Padding = new System.Windows.Forms.Padding(30, 10, 10, 10);
            this.buttons_FlowLayoutPanel.Size = new System.Drawing.Size(256, 65);
            this.buttons_FlowLayoutPanel.TabIndex = 3;
            // 
            // pridat_Button
            // 
            this.pridat_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pridat_Button.AutoSize = true;
            this.pridat_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.pridat_Button.Location = new System.Drawing.Point(33, 13);
            this.pridat_Button.Name = "pridat_Button";
            this.pridat_Button.Size = new System.Drawing.Size(102, 39);
            this.pridat_Button.TabIndex = 0;
            this.pridat_Button.Text = "Přidat";
            this.pridat_Button.UseVisualStyleBackColor = false;
            this.pridat_Button.Click += new System.EventHandler(this.pridat_Button_Click);
            // 
            // smazat_Button
            // 
            this.smazat_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.smazat_Button.AutoSize = true;
            this.smazat_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.smazat_Button.Location = new System.Drawing.Point(141, 13);
            this.smazat_Button.Name = "smazat_Button";
            this.smazat_Button.Size = new System.Drawing.Size(102, 39);
            this.smazat_Button.TabIndex = 1;
            this.smazat_Button.Text = "Smazat";
            this.smazat_Button.UseVisualStyleBackColor = false;
            this.smazat_Button.Click += new System.EventHandler(this.smazat_Button_Click);
            // 
            // lek_Label
            // 
            this.lek_Label.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lek_Label, 2);
            this.lek_Label.Location = new System.Drawing.Point(10, 10);
            this.lek_Label.Margin = new System.Windows.Forms.Padding(10);
            this.lek_Label.Name = "lek_Label";
            this.lek_Label.Size = new System.Drawing.Size(103, 24);
            this.lek_Label.TabIndex = 4;
            this.lek_Label.Text = "Nazev leku";
            // 
            // ordinace_ListView
            // 
            this.ordinace_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.zacatek_ColumnHeader,
            this.konec_ColumnHeader,
            this.stav_ColumnHeader,
            this.davka_ColumnHeader});
            this.ordinace_ListView.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ordinace_ListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ordinace_ListView.FullRowSelect = true;
            this.ordinace_ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ordinace_ListView.HideSelection = false;
            this.ordinace_ListView.Location = new System.Drawing.Point(3, 47);
            this.ordinace_ListView.MultiSelect = false;
            this.ordinace_ListView.Name = "ordinace_ListView";
            this.ordinace_ListView.Size = new System.Drawing.Size(470, 387);
            this.ordinace_ListView.TabIndex = 5;
            this.ordinace_ListView.UseCompatibleStateImageBehavior = false;
            this.ordinace_ListView.View = System.Windows.Forms.View.Details;
            this.ordinace_ListView.Click += new System.EventHandler(this.ordinace_ListView_SelectedIndexChanged);
            // 
            // zacatek_ColumnHeader
            // 
            this.zacatek_ColumnHeader.Text = "Čas podání";
            this.zacatek_ColumnHeader.Width = 165;
            // 
            // konec_ColumnHeader
            // 
            this.konec_ColumnHeader.Text = "(konec infuze)";
            this.konec_ColumnHeader.Width = 165;
            // 
            // stav_ColumnHeader
            // 
            this.stav_ColumnHeader.Text = "Stav";
            this.stav_ColumnHeader.Width = 55;
            // 
            // davka_ColumnHeader
            // 
            this.davka_ColumnHeader.Text = "Dávka";
            this.davka_ColumnHeader.Width = 80;
            // 
            // detail_FlowLayoutPanel
            // 
            this.detail_FlowLayoutPanel.Controls.Add(this.zacatek_Label);
            this.detail_FlowLayoutPanel.Controls.Add(this.konec_CheckBox);
            this.detail_FlowLayoutPanel.Controls.Add(this.datumOd_DateTimePicker);
            this.detail_FlowLayoutPanel.Controls.Add(this.datumDo_DateTimePicker);
            this.detail_FlowLayoutPanel.Controls.Add(this.stav_Label);
            this.detail_FlowLayoutPanel.Controls.Add(this.stav_ComboBox);
            this.detail_FlowLayoutPanel.Controls.Add(this.mnozstvi_Label);
            this.detail_FlowLayoutPanel.Controls.Add(this.davka_TextBox);
            this.detail_FlowLayoutPanel.Controls.Add(this.jednotky_Label);
            this.detail_FlowLayoutPanel.Controls.Add(this.jednotky_ComboBox);
            this.detail_FlowLayoutPanel.Controls.Add(this.pozn_Label);
            this.detail_FlowLayoutPanel.Controls.Add(this.poznamka_TextBox);
            this.detail_FlowLayoutPanel.Controls.Add(this.kontinual_CheckBox);
            this.detail_FlowLayoutPanel.Controls.Add(this.podavajici_Label);
            this.detail_FlowLayoutPanel.Controls.Add(this.podat_Button);
            this.detail_FlowLayoutPanel.Controls.Add(this.nepodat_Button);
            this.detail_FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detail_FlowLayoutPanel.Location = new System.Drawing.Point(476, 44);
            this.detail_FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.detail_FlowLayoutPanel.Name = "detail_FlowLayoutPanel";
            this.detail_FlowLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            this.detail_FlowLayoutPanel.Size = new System.Drawing.Size(572, 393);
            this.detail_FlowLayoutPanel.TabIndex = 6;
            // 
            // zacatek_Label
            // 
            this.zacatek_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.zacatek_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zacatek_Label.Location = new System.Drawing.Point(13, 10);
            this.zacatek_Label.Name = "zacatek_Label";
            this.zacatek_Label.Size = new System.Drawing.Size(270, 34);
            this.zacatek_Label.TabIndex = 0;
            this.zacatek_Label.Text = "Čas podání\r\n(začátek infuze, změna rychlosti)";
            // 
            // konec_CheckBox
            // 
            this.konec_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.konec_CheckBox.AutoSize = true;
            this.konec_CheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.konec_CheckBox.Location = new System.Drawing.Point(289, 20);
            this.konec_CheckBox.Name = "konec_CheckBox";
            this.konec_CheckBox.Size = new System.Drawing.Size(117, 21);
            this.konec_CheckBox.TabIndex = 1;
            this.konec_CheckBox.Text = "(konec infuze)";
            this.konec_CheckBox.UseVisualStyleBackColor = true;
            this.konec_CheckBox.Click += new System.EventHandler(this.konec_CheckBox_CheckedChanged);
            // 
            // datumOd_DateTimePicker
            // 
            this.datumOd_DateTimePicker.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datumOd_DateTimePicker.CustomFormat = "dd.MM.yyyy   HH:mm";
            this.datumOd_DateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datumOd_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datumOd_DateTimePicker.Location = new System.Drawing.Point(13, 54);
            this.datumOd_DateTimePicker.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.datumOd_DateTimePicker.Name = "datumOd_DateTimePicker";
            this.datumOd_DateTimePicker.Size = new System.Drawing.Size(270, 38);
            this.datumOd_DateTimePicker.TabIndex = 2;
            this.datumOd_DateTimePicker.CloseUp += new System.EventHandler(this.ValueChanged);
            this.datumOd_DateTimePicker.MouseEnter += new System.EventHandler(this.DateTimePicker_MouseEnter);
            this.datumOd_DateTimePicker.MouseLeave += new System.EventHandler(this.DateTimePicker_MouseLeave);
            // 
            // datumDo_DateTimePicker
            // 
            this.datumDo_DateTimePicker.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datumDo_DateTimePicker.CustomFormat = "dd.MM.yyyy   HH:mm";
            this.datumDo_DateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datumDo_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datumDo_DateTimePicker.Location = new System.Drawing.Point(289, 54);
            this.datumDo_DateTimePicker.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.datumDo_DateTimePicker.Name = "datumDo_DateTimePicker";
            this.datumDo_DateTimePicker.Size = new System.Drawing.Size(270, 38);
            this.datumDo_DateTimePicker.TabIndex = 3;
            this.datumDo_DateTimePicker.CloseUp += new System.EventHandler(this.ValueChanged);
            this.datumDo_DateTimePicker.MouseEnter += new System.EventHandler(this.DateTimePicker_MouseEnter);
            this.datumDo_DateTimePicker.MouseLeave += new System.EventHandler(this.DateTimePicker_MouseLeave);
            // 
            // stav_Label
            // 
            this.stav_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stav_Label.AutoSize = true;
            this.stav_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stav_Label.Location = new System.Drawing.Point(13, 124);
            this.stav_Label.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.stav_Label.Name = "stav_Label";
            this.stav_Label.Size = new System.Drawing.Size(116, 18);
            this.stav_Label.TabIndex = 4;
            this.stav_Label.Text = "Stav podání léku";
            // 
            // stav_ComboBox
            // 
            this.stav_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stav_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stav_ComboBox.FormattingEnabled = true;
            this.stav_ComboBox.Location = new System.Drawing.Point(135, 112);
            this.stav_ComboBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.stav_ComboBox.Name = "stav_ComboBox";
            this.stav_ComboBox.Size = new System.Drawing.Size(424, 30);
            this.stav_ComboBox.TabIndex = 5;
            this.stav_ComboBox.SelectedIndexChanged += new System.EventHandler(this.stav_ComboBox_SelectedValueChanged);
            this.stav_ComboBox.Click += new System.EventHandler(this.ValueChanged);
            // 
            // mnozstvi_Label
            // 
            this.mnozstvi_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mnozstvi_Label.AutoSize = true;
            this.mnozstvi_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnozstvi_Label.Location = new System.Drawing.Point(13, 174);
            this.mnozstvi_Label.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.mnozstvi_Label.Name = "mnozstvi_Label";
            this.mnozstvi_Label.Size = new System.Drawing.Size(68, 18);
            this.mnozstvi_Label.TabIndex = 6;
            this.mnozstvi_Label.Text = "Množství";
            // 
            // davka_TextBox
            // 
            this.davka_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.davka_TextBox.Location = new System.Drawing.Point(87, 164);
            this.davka_TextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.davka_TextBox.Name = "davka_TextBox";
            this.davka_TextBox.Size = new System.Drawing.Size(167, 28);
            this.davka_TextBox.TabIndex = 7;
            this.davka_TextBox.Click += new System.EventHandler(this.davka_TextBox_Click);
            // 
            // jednotky_Label
            // 
            this.jednotky_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.jednotky_Label.AutoSize = true;
            this.jednotky_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jednotky_Label.Location = new System.Drawing.Point(260, 174);
            this.jednotky_Label.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.jednotky_Label.Name = "jednotky_Label";
            this.jednotky_Label.Size = new System.Drawing.Size(97, 18);
            this.jednotky_Label.TabIndex = 8;
            this.jednotky_Label.Text = "měr. jednotky";
            // 
            // jednotky_ComboBox
            // 
            this.jednotky_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jednotky_ComboBox.FormattingEnabled = true;
            this.jednotky_ComboBox.Location = new System.Drawing.Point(363, 162);
            this.jednotky_ComboBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.jednotky_ComboBox.Name = "jednotky_ComboBox";
            this.jednotky_ComboBox.Size = new System.Drawing.Size(196, 30);
            this.jednotky_ComboBox.TabIndex = 9;
            this.jednotky_ComboBox.Click += new System.EventHandler(this.ValueChanged);
            // 
            // pozn_Label
            // 
            this.pozn_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pozn_Label.AutoSize = true;
            this.pozn_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pozn_Label.Location = new System.Drawing.Point(13, 222);
            this.pozn_Label.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.pozn_Label.Name = "pozn_Label";
            this.pozn_Label.Size = new System.Drawing.Size(47, 18);
            this.pozn_Label.TabIndex = 10;
            this.pozn_Label.Text = "Pozn.";
            // 
            // poznamka_TextBox
            // 
            this.poznamka_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.poznamka_TextBox.Location = new System.Drawing.Point(66, 212);
            this.poznamka_TextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.poznamka_TextBox.Name = "poznamka_TextBox";
            this.poznamka_TextBox.Size = new System.Drawing.Size(493, 28);
            this.poznamka_TextBox.TabIndex = 11;
            this.poznamka_TextBox.Click += new System.EventHandler(this.poznamka_TextBox_Click);
            // 
            // kontinual_CheckBox
            // 
            this.kontinual_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kontinual_CheckBox.AutoSize = true;
            this.kontinual_CheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kontinual_CheckBox.Location = new System.Drawing.Point(13, 260);
            this.kontinual_CheckBox.Margin = new System.Windows.Forms.Padding(3, 10, 350, 10);
            this.kontinual_CheckBox.Name = "kontinual_CheckBox";
            this.kontinual_CheckBox.Size = new System.Drawing.Size(198, 22);
            this.kontinual_CheckBox.TabIndex = 12;
            this.kontinual_CheckBox.Text = "Kontinuální podání / infuze";
            this.kontinual_CheckBox.UseVisualStyleBackColor = true;
            this.kontinual_CheckBox.Click += new System.EventHandler(this.ValueChanged);
            // 
            // podavajici_Label
            // 
            this.podavajici_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.podavajici_Label.AutoSize = true;
            this.detail_FlowLayoutPanel.SetFlowBreak(this.podavajici_Label, true);
            this.podavajici_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.podavajici_Label.Location = new System.Drawing.Point(13, 304);
            this.podavajici_Label.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.podavajici_Label.Name = "podavajici_Label";
            this.podavajici_Label.Size = new System.Drawing.Size(79, 18);
            this.podavajici_Label.TabIndex = 13;
            this.podavajici_Label.Text = "Podávající:";
            // 
            // podat_Button
            // 
            this.podat_Button.AutoSize = true;
            this.podat_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.podat_Button.Location = new System.Drawing.Point(13, 335);
            this.podat_Button.Name = "podat_Button";
            this.podat_Button.Size = new System.Drawing.Size(92, 34);
            this.podat_Button.TabIndex = 14;
            this.podat_Button.Text = "Podat";
            this.podat_Button.UseVisualStyleBackColor = false;
            this.podat_Button.Click += new System.EventHandler(this.podat_Button_Click);
            // 
            // nepodat_Button
            // 
            this.nepodat_Button.AutoSize = true;
            this.nepodat_Button.BackColor = System.Drawing.Color.MediumOrchid;
            this.nepodat_Button.Location = new System.Drawing.Point(111, 335);
            this.nepodat_Button.Name = "nepodat_Button";
            this.nepodat_Button.Size = new System.Drawing.Size(92, 34);
            this.nepodat_Button.TabIndex = 15;
            this.nepodat_Button.Text = "Nepodat";
            this.nepodat_Button.UseVisualStyleBackColor = false;
            this.nepodat_Button.Click += new System.EventHandler(this.nepodat_Button_Click);
            // 
            // zavrit_Button
            // 
            this.zavrit_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zavrit_Button.AutoSize = true;
            this.zavrit_Button.Location = new System.Drawing.Point(916, 453);
            this.zavrit_Button.Margin = new System.Windows.Forms.Padding(10, 10, 30, 10);
            this.zavrit_Button.Name = "zavrit_Button";
            this.zavrit_Button.Size = new System.Drawing.Size(102, 39);
            this.zavrit_Button.TabIndex = 2;
            this.zavrit_Button.Text = "Zavřít";
            this.zavrit_Button.UseVisualStyleBackColor = true;
            this.zavrit_Button.Click += new System.EventHandler(this.zavrit_Button_Click);
            // 
            // Ordinace_Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1048, 502);
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Ordinace_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editace podání vybraného léku";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Podani_Form_FormClosing);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.buttons_FlowLayoutPanel.ResumeLayout(false);
            this.buttons_FlowLayoutPanel.PerformLayout();
            this.detail_FlowLayoutPanel.ResumeLayout(false);
            this.detail_FlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button zavrit_Button;
        private System.Windows.Forms.FlowLayoutPanel buttons_FlowLayoutPanel;
        private System.Windows.Forms.Button pridat_Button;
        private System.Windows.Forms.Button smazat_Button;
        private System.Windows.Forms.Label lek_Label;
        private System.Windows.Forms.ListView ordinace_ListView;
        private System.Windows.Forms.FlowLayoutPanel detail_FlowLayoutPanel;
        private System.Windows.Forms.Label zacatek_Label;
        private System.Windows.Forms.CheckBox konec_CheckBox;
        private System.Windows.Forms.DateTimePicker datumOd_DateTimePicker;
        private System.Windows.Forms.DateTimePicker datumDo_DateTimePicker;
        private System.Windows.Forms.Label stav_Label;
        private System.Windows.Forms.ComboBox stav_ComboBox;
        private System.Windows.Forms.Label mnozstvi_Label;
        private System.Windows.Forms.TextBox davka_TextBox;
        private System.Windows.Forms.Label jednotky_Label;
        private System.Windows.Forms.ComboBox jednotky_ComboBox;
        private System.Windows.Forms.Label pozn_Label;
        private System.Windows.Forms.TextBox poznamka_TextBox;
        private System.Windows.Forms.CheckBox kontinual_CheckBox;
        private System.Windows.Forms.Label podavajici_Label;
        private System.Windows.Forms.ColumnHeader zacatek_ColumnHeader;
        private System.Windows.Forms.ColumnHeader konec_ColumnHeader;
        private System.Windows.Forms.ColumnHeader stav_ColumnHeader;
        private System.Windows.Forms.ColumnHeader davka_ColumnHeader;
        private System.Windows.Forms.Button podat_Button;
        private System.Windows.Forms.Button nepodat_Button;
    }
}