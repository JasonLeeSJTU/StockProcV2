namespace StockProc
{
    partial class DateSelectedWeekK
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateSelectedWeekK));
            this.rowMergeView_dateSelected = new StockProc.RowMergeView();
            this.year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.week = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.monday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tuesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wednesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thursday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.friday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.increasePercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.rowMergeView_dateSelected)).BeginInit();
            this.SuspendLayout();
            // 
            // rowMergeView_dateSelected
            // 
            this.rowMergeView_dateSelected.AllowUserToAddRows = false;
            this.rowMergeView_dateSelected.AllowUserToDeleteRows = false;
            this.rowMergeView_dateSelected.AllowUserToResizeColumns = false;
            this.rowMergeView_dateSelected.AllowUserToResizeRows = false;
            this.rowMergeView_dateSelected.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.rowMergeView_dateSelected.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.rowMergeView_dateSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rowMergeView_dateSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.year,
            this.month,
            this.week,
            this.monday,
            this.tuesday,
            this.wednesday,
            this.thursday,
            this.friday,
            this.sum,
            this.increasePercent});
            this.rowMergeView_dateSelected.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.rowMergeView_dateSelected.DefaultCellStyle = dataGridViewCellStyle2;
            this.rowMergeView_dateSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowMergeView_dateSelected.Location = new System.Drawing.Point(0, 0);
            this.rowMergeView_dateSelected.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rowMergeView_dateSelected.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.rowMergeView_dateSelected.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("rowMergeView_dateSelected.MergeColumnNames")));
            this.rowMergeView_dateSelected.Name = "rowMergeView_dateSelected";
            this.rowMergeView_dateSelected.ReadOnly = true;
            this.rowMergeView_dateSelected.RowTemplate.Height = 23;
            this.rowMergeView_dateSelected.Size = new System.Drawing.Size(1057, 716);
            this.rowMergeView_dateSelected.TabIndex = 0;
            this.rowMergeView_dateSelected.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.rowMergeView_dateSelected_CellPainting);
            this.rowMergeView_dateSelected.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.rowMergeView_dateSelected_ColumnHeaderMouseClick);
            // 
            // year
            // 
            this.year.DataPropertyName = "col_year";
            this.year.HeaderText = "年份";
            this.year.Name = "year";
            this.year.ReadOnly = true;
            // 
            // month
            // 
            this.month.DataPropertyName = "col_month";
            this.month.HeaderText = "月份";
            this.month.Name = "month";
            this.month.ReadOnly = true;
            // 
            // week
            // 
            this.week.DataPropertyName = "col_week";
            this.week.HeaderText = "周数";
            this.week.Name = "week";
            this.week.ReadOnly = true;
            // 
            // monday
            // 
            this.monday.DataPropertyName = "col_mon";
            this.monday.HeaderText = "周一（亿）";
            this.monday.Name = "monday";
            this.monday.ReadOnly = true;
            // 
            // tuesday
            // 
            this.tuesday.DataPropertyName = "col_tue";
            this.tuesday.HeaderText = "周二（亿）";
            this.tuesday.Name = "tuesday";
            this.tuesday.ReadOnly = true;
            // 
            // wednesday
            // 
            this.wednesday.DataPropertyName = "col_wed";
            this.wednesday.HeaderText = "周三（亿）";
            this.wednesday.Name = "wednesday";
            this.wednesday.ReadOnly = true;
            // 
            // thursday
            // 
            this.thursday.DataPropertyName = "col_thur";
            this.thursday.HeaderText = "周四（亿）";
            this.thursday.Name = "thursday";
            this.thursday.ReadOnly = true;
            // 
            // friday
            // 
            this.friday.DataPropertyName = "col_fri";
            this.friday.HeaderText = "周五（亿）";
            this.friday.Name = "friday";
            this.friday.ReadOnly = true;
            // 
            // sum
            // 
            this.sum.DataPropertyName = "col_sum";
            this.sum.HeaderText = "累计（亿）";
            this.sum.Name = "sum";
            this.sum.ReadOnly = true;
            // 
            // increasePercent
            // 
            this.increasePercent.DataPropertyName = "col_increasePercent";
            this.increasePercent.HeaderText = "增长比";
            this.increasePercent.Name = "increasePercent";
            this.increasePercent.ReadOnly = true;
            this.increasePercent.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.increasePercent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DateSelectedWeekK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 716);
            this.Controls.Add(this.rowMergeView_dateSelected);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DateSelectedWeekK";
            ((System.ComponentModel.ISupportInitialize)(this.rowMergeView_dateSelected)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public RowMergeView rowMergeView_dateSelected;
        private bool isColor = false;
        private System.Windows.Forms.DataGridViewTextBoxColumn year;
        private System.Windows.Forms.DataGridViewTextBoxColumn month;
        private System.Windows.Forms.DataGridViewTextBoxColumn week;
        private System.Windows.Forms.DataGridViewTextBoxColumn monday;
        private System.Windows.Forms.DataGridViewTextBoxColumn tuesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn wednesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn thursday;
        private System.Windows.Forms.DataGridViewTextBoxColumn friday;
        private System.Windows.Forms.DataGridViewTextBoxColumn sum;
        private System.Windows.Forms.DataGridViewTextBoxColumn increasePercent;
    }
}