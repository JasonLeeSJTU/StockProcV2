namespace StockProc
{
    partial class WeekK
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeekK));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dataSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.rowMergeView_weekK = new StockProc.RowMergeView();
            this.year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.week = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Monday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tuesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wednesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Thursday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Friday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rowMergeView_weekK)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataSelection});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 26);
            // 
            // dataSelection
            // 
            this.dataSelection.Name = "dataSelection";
            this.dataSelection.Size = new System.Drawing.Size(160, 22);
            this.dataSelection.Text = "周累计和增长比";
            this.dataSelection.Click += new System.EventHandler(this.dataSelection_Click);
            // 
            // rowMergeView_weekK
            // 
            this.rowMergeView_weekK.AllowUserToAddRows = false;
            this.rowMergeView_weekK.AllowUserToDeleteRows = false;
            this.rowMergeView_weekK.AllowUserToResizeColumns = false;
            this.rowMergeView_weekK.AllowUserToResizeRows = false;
            this.rowMergeView_weekK.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.rowMergeView_weekK.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.rowMergeView_weekK.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rowMergeView_weekK.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.year,
            this.month,
            this.week,
            this.date,
            this.Monday,
            this.Tuesday,
            this.Wednesday,
            this.Thursday,
            this.Friday,
            this.Total});
            this.rowMergeView_weekK.ContextMenuStrip = this.contextMenuStrip1;
            this.rowMergeView_weekK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowMergeView_weekK.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.rowMergeView_weekK.Location = new System.Drawing.Point(0, 0);
            this.rowMergeView_weekK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rowMergeView_weekK.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.rowMergeView_weekK.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("rowMergeView_weekK.MergeColumnNames")));
            this.rowMergeView_weekK.Name = "rowMergeView_weekK";
            this.rowMergeView_weekK.ReadOnly = true;
            this.rowMergeView_weekK.RowHeadersWidth = 60;
            this.rowMergeView_weekK.RowTemplate.Height = 23;
            this.rowMergeView_weekK.Size = new System.Drawing.Size(1057, 716);
            this.rowMergeView_weekK.TabIndex = 0;
            this.rowMergeView_weekK.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.rowMergeView_weekK_CellPainting);
            this.rowMergeView_weekK.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.rowMergeView_weekK_ColumnHeaderMouseClick);
            // 
            // year
            // 
            this.year.DataPropertyName = "col0";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.year.DefaultCellStyle = dataGridViewCellStyle2;
            this.year.HeaderText = "年份";
            this.year.Name = "year";
            this.year.ReadOnly = true;
            // 
            // month
            // 
            this.month.DataPropertyName = "col01";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.month.DefaultCellStyle = dataGridViewCellStyle3;
            this.month.HeaderText = "月份";
            this.month.Name = "month";
            this.month.ReadOnly = true;
            // 
            // week
            // 
            this.week.DataPropertyName = "col1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.week.DefaultCellStyle = dataGridViewCellStyle4;
            this.week.HeaderText = "周数";
            this.week.Name = "week";
            this.week.ReadOnly = true;
            // 
            // date
            // 
            this.date.DataPropertyName = "col2";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.date.DefaultCellStyle = dataGridViewCellStyle5;
            this.date.HeaderText = "项目";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // Monday
            // 
            this.Monday.DataPropertyName = "col3";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Monday.DefaultCellStyle = dataGridViewCellStyle6;
            this.Monday.HeaderText = "周一（亿）";
            this.Monday.Name = "Monday";
            this.Monday.ReadOnly = true;
            // 
            // Tuesday
            // 
            this.Tuesday.DataPropertyName = "col4";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Tuesday.DefaultCellStyle = dataGridViewCellStyle7;
            this.Tuesday.HeaderText = "周二（亿）";
            this.Tuesday.Name = "Tuesday";
            this.Tuesday.ReadOnly = true;
            // 
            // Wednesday
            // 
            this.Wednesday.DataPropertyName = "col5";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Wednesday.DefaultCellStyle = dataGridViewCellStyle8;
            this.Wednesday.HeaderText = "周三（亿）";
            this.Wednesday.Name = "Wednesday";
            this.Wednesday.ReadOnly = true;
            // 
            // Thursday
            // 
            this.Thursday.DataPropertyName = "col6";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Thursday.DefaultCellStyle = dataGridViewCellStyle9;
            this.Thursday.HeaderText = "周四（亿）";
            this.Thursday.Name = "Thursday";
            this.Thursday.ReadOnly = true;
            // 
            // Friday
            // 
            this.Friday.DataPropertyName = "col7";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Friday.DefaultCellStyle = dataGridViewCellStyle10;
            this.Friday.HeaderText = "周五（亿）";
            this.Friday.Name = "Friday";
            this.Friday.ReadOnly = true;
            // 
            // Total
            // 
            this.Total.DataPropertyName = "col8";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Total.DefaultCellStyle = dataGridViewCellStyle11;
            this.Total.HeaderText = "累计（亿）";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            // 
            // WeekK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 716);
            this.Controls.Add(this.rowMergeView_weekK);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WeekK";
            this.Text = "周K基准量";
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rowMergeView_weekK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public RowMergeView rowMergeView_weekK;
        private bool isColor = false;
        private System.Windows.Forms.DataGridViewTextBoxColumn year;
        private System.Windows.Forms.DataGridViewTextBoxColumn month;
        private System.Windows.Forms.DataGridViewTextBoxColumn week;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Monday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tuesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wednesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Thursday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Friday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataSelection;
    }
}