namespace StockProc
{
    partial class LastDay
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
            this.dataGridView_LastDay = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_LastDay)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_LastDay
            // 
            this.dataGridView_LastDay.AllowUserToAddRows = false;
            this.dataGridView_LastDay.AllowUserToDeleteRows = false;
            this.dataGridView_LastDay.AllowUserToResizeColumns = false;
            this.dataGridView_LastDay.AllowUserToResizeRows = false;
            this.dataGridView_LastDay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_LastDay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_LastDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_LastDay.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_LastDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_LastDay.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView_LastDay.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_LastDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView_LastDay.Name = "dataGridView_LastDay";
            this.dataGridView_LastDay.RowHeadersWidth = 60;
            this.dataGridView_LastDay.RowTemplate.Height = 23;
            this.dataGridView_LastDay.Size = new System.Drawing.Size(1868, 741);
            this.dataGridView_LastDay.TabIndex = 0;
            this.dataGridView_LastDay.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_LastDay_CellDoubleClick);
            this.dataGridView_LastDay.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView_LastDay_CellPainting);
            this.dataGridView_LastDay.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_LastDay_ColumnHeaderMouseClick);
            // 
            // LastDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1868, 741);
            this.Controls.Add(this.dataGridView_LastDay);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LastDay";
            this.Text = "末日";
            this.Load += new System.EventHandler(this.LastDay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_LastDay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridView_LastDay;
        private bool isColor = false;
    }
}