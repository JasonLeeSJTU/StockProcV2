namespace StockProc
{
    partial class StockDetail
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
            this.dataGridView_StockDetail = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_StockDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_StockDetail
            // 
            this.dataGridView_StockDetail.AllowUserToAddRows = false;
            this.dataGridView_StockDetail.AllowUserToDeleteRows = false;
            this.dataGridView_StockDetail.AllowUserToResizeColumns = false;
            this.dataGridView_StockDetail.AllowUserToResizeRows = false;
            this.dataGridView_StockDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_StockDetail.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_StockDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_StockDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_StockDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_StockDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView_StockDetail.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_StockDetail.Name = "dataGridView_StockDetail";
            this.dataGridView_StockDetail.RowHeadersWidth = 60;
            this.dataGridView_StockDetail.RowTemplate.Height = 23;
            this.dataGridView_StockDetail.Size = new System.Drawing.Size(1360, 611);
            this.dataGridView_StockDetail.TabIndex = 0;
            this.dataGridView_StockDetail.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_StockDetail_ColumnHeaderMouseClick);
            // 
            // StockDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 611);
            this.Controls.Add(this.dataGridView_StockDetail);
            this.Name = "StockDetail";
            this.Text = "StockDetail";
            this.Load += new System.EventHandler(this.StockDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_StockDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridView_StockDetail;
    }
}