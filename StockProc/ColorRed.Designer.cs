namespace StockProc
{
    partial class ColorRed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorRed));
            this.colorRedrowMergeView = new StockProc.RowMergeView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.colorRedrowMergeView)).BeginInit();
            this.SuspendLayout();
            // 
            // colorRedrowMergeView
            // 
            this.colorRedrowMergeView.AllowUserToAddRows = false;
            this.colorRedrowMergeView.AllowUserToDeleteRows = false;
            this.colorRedrowMergeView.AllowUserToResizeColumns = false;
            this.colorRedrowMergeView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colorRedrowMergeView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.colorRedrowMergeView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.colorRedrowMergeView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10});
            this.colorRedrowMergeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorRedrowMergeView.Location = new System.Drawing.Point(0, 0);
            this.colorRedrowMergeView.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.colorRedrowMergeView.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("colorRedrowMergeView.MergeColumnNames")));
            this.colorRedrowMergeView.Name = "colorRedrowMergeView";
            this.colorRedrowMergeView.RowHeadersWidth = 60;
            this.colorRedrowMergeView.RowTemplate.Height = 23;
            this.colorRedrowMergeView.Size = new System.Drawing.Size(1043, 450);
            this.colorRedrowMergeView.TabIndex = 0;
            this.colorRedrowMergeView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.colorRedrowMergeView_CellPainting);
            this.colorRedrowMergeView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.colorRedrowMergeView_ColumnHeaderMouseClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "id";
            this.Column1.HeaderText = "股票代码";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "name";
            this.Column2.HeaderText = "股票名称";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "date";
            this.Column3.HeaderText = "日期";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "item";
            this.Column4.HeaderText = "项目";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "one";
            this.Column5.HeaderText = "周一（亿）";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "two";
            this.Column6.HeaderText = "周二（亿）";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "three";
            this.Column7.HeaderText = "周三（亿）";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "four";
            this.Column8.HeaderText = "周四（亿）";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "five";
            this.Column9.HeaderText = "周五（亿）";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "total";
            this.Column10.HeaderText = "累计（亿）";
            this.Column10.Name = "Column10";
            // 
            // ColorRed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 450);
            this.Controls.Add(this.colorRedrowMergeView);
            this.Name = "ColorRed";
            this.Text = "ColorRed";
            this.Load += new System.EventHandler(this.ColorRed_Load);
            ((System.ComponentModel.ISupportInitialize)(this.colorRedrowMergeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public RowMergeView colorRedrowMergeView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private bool isColor = false;
    }
}