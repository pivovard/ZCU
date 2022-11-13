namespace MediTab
{
    partial class BilanceHod_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.zavrit_Button = new System.Windows.Forms.Button();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label = new System.Windows.Forms.Label();
            this.celkem_TextBox = new System.Windows.Forms.TextBox();
            this.hodnoty_DataGridView = new System.Windows.Forms.DataGridView();
            this.hodina_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hodnota_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hodnoty_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // zavrit_Button
            // 
            this.zavrit_Button.AutoSize = true;
            this.zavrit_Button.BackColor = System.Drawing.Color.OrangeRed;
            this.zavrit_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.zavrit_Button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.zavrit_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zavrit_Button.Location = new System.Drawing.Point(0, 607);
            this.zavrit_Button.Margin = new System.Windows.Forms.Padding(4);
            this.zavrit_Button.Name = "zavrit_Button";
            this.zavrit_Button.Size = new System.Drawing.Size(582, 46);
            this.zavrit_Button.TabIndex = 1;
            this.zavrit_Button.Text = "Zavřít";
            this.zavrit_Button.UseVisualStyleBackColor = false;
            this.zavrit_Button.Click += new System.EventHandler(this.zavrit_Button_Click);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Controls.Add(this.label);
            this.flowLayoutPanel.Controls.Add(this.celkem_TextBox);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 565);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(582, 42);
            this.flowLayoutPanel.TabIndex = 2;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(20, 6);
            this.label.Margin = new System.Windows.Forms.Padding(20, 6, 7, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(179, 29);
            this.label.TabIndex = 0;
            this.label.Text = "Celkem za den:";
            // 
            // celkem_TextBox
            // 
            this.celkem_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.celkem_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.celkem_TextBox.Location = new System.Drawing.Point(210, 4);
            this.celkem_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.celkem_TextBox.Name = "celkem_TextBox";
            this.celkem_TextBox.ReadOnly = true;
            this.celkem_TextBox.Size = new System.Drawing.Size(132, 34);
            this.celkem_TextBox.TabIndex = 1;
            // 
            // hodnoty_DataGridView
            // 
            this.hodnoty_DataGridView.AllowUserToAddRows = false;
            this.hodnoty_DataGridView.AllowUserToDeleteRows = false;
            this.hodnoty_DataGridView.AllowUserToResizeColumns = false;
            this.hodnoty_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.hodnoty_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.hodnoty_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hodnoty_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hodina_Column,
            this.hodnota_Column});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.hodnoty_DataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.hodnoty_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hodnoty_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.hodnoty_DataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.hodnoty_DataGridView.MultiSelect = false;
            this.hodnoty_DataGridView.Name = "hodnoty_DataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.hodnoty_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.hodnoty_DataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hodnoty_DataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.hodnoty_DataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hodnoty_DataGridView.RowTemplate.Height = 35;
            this.hodnoty_DataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.hodnoty_DataGridView.Size = new System.Drawing.Size(582, 565);
            this.hodnoty_DataGridView.TabIndex = 3;
            this.hodnoty_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.hodnoty_DataGridView_CellClick);
            // 
            // hodina_Column
            // 
            this.hodina_Column.HeaderText = "Hodina";
            this.hodina_Column.Name = "hodina_Column";
            this.hodina_Column.ReadOnly = true;
            this.hodina_Column.Width = 230;
            // 
            // hodnota_Column
            // 
            this.hodnota_Column.HeaderText = "ml";
            this.hodnota_Column.Name = "hodnota_Column";
            this.hodnota_Column.ReadOnly = true;
            this.hodnota_Column.Width = 250;
            // 
            // BilanceHod_Form
            // 
            this.AcceptButton = this.zavrit_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 653);
            this.Controls.Add(this.hodnoty_DataGridView);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.zavrit_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BilanceHod_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hodinová Bilance";
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hodnoty_DataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button zavrit_Button;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox celkem_TextBox;
        private System.Windows.Forms.DataGridView hodnoty_DataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn hodina_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn hodnota_Column;
    }
}