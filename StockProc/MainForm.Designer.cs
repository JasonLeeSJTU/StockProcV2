namespace StockProc
{
	partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView_all = new System.Windows.Forms.DataGridView();
            this.menuStrip_mainMenu = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.Open_zhuanhuan_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aB0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseMinusBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AMinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MeanLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WeekKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CalcWeekKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLastDay = new System.Windows.Forms.ToolStripMenuItem();
            this.Red3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Red4ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Red5ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_progressBar = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_all)).BeginInit();
            this.menuStrip_mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "Open Excel Files";
            // 
            // dataGridView_all
            // 
            this.dataGridView_all.AllowUserToAddRows = false;
            this.dataGridView_all.AllowUserToDeleteRows = false;
            this.dataGridView_all.AllowUserToResizeColumns = false;
            this.dataGridView_all.AllowUserToResizeRows = false;
            this.dataGridView_all.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_all.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_all.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_all.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_all.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_all.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_all.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView_all.Location = new System.Drawing.Point(0, 25);
            this.dataGridView_all.Name = "dataGridView_all";
            this.dataGridView_all.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_all.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_all.RowHeadersWidth = 60;
            this.dataGridView_all.RowTemplate.Height = 23;
            this.dataGridView_all.Size = new System.Drawing.Size(817, 698);
            this.dataGridView_all.TabIndex = 1;
            this.dataGridView_all.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_all_CellDoubleClick);
            this.dataGridView_all.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_all_ColumnHeaderMouseClick);
            // 
            // menuStrip_mainMenu
            // 
            this.menuStrip_mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.CToolStripMenuItem,
            this.aB0ToolStripMenuItem,
            this.CloseMinusBToolStripMenuItem,
            this.KToolStripMenuItem,
            this.CloseAToolStripMenuItem,
            this.AMinToolStripMenuItem,
            this.MeanLineToolStripMenuItem,
            this.WeekKToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip_mainMenu.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_mainMenu.Name = "menuStrip_mainMenu";
            this.menuStrip_mainMenu.Size = new System.Drawing.Size(817, 25);
            this.menuStrip_mainMenu.TabIndex = 2;
            this.menuStrip_mainMenu.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFile,
            this.Open_zhuanhuan_ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // OpenFile
            // 
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(208, 22);
            this.OpenFile.Text = "打开";
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // Open_zhuanhuan_ToolStripMenuItem
            // 
            this.Open_zhuanhuan_ToolStripMenuItem.Name = "Open_zhuanhuan_ToolStripMenuItem";
            this.Open_zhuanhuan_ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.Open_zhuanhuan_ToolStripMenuItem.Text = "打开转换宝六十分钟数据";
            this.Open_zhuanhuan_ToolStripMenuItem.Click += new System.EventHandler(this.Open_zhuanhuan_ToolStripMenuItem_Click);
            // 
            // CToolStripMenuItem
            // 
            this.CToolStripMenuItem.Name = "CToolStripMenuItem";
            this.CToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.CToolStripMenuItem.Text = "C=B-A";
            this.CToolStripMenuItem.Visible = false;
            this.CToolStripMenuItem.Click += new System.EventHandler(this.CToolStripMenuItem_Click);
            // 
            // aB0ToolStripMenuItem
            // 
            this.aB0ToolStripMenuItem.Name = "aB0ToolStripMenuItem";
            this.aB0ToolStripMenuItem.Size = new System.Drawing.Size(57, 21);
            this.aB0ToolStripMenuItem.Text = "A-B=0";
            this.aB0ToolStripMenuItem.Visible = false;
            this.aB0ToolStripMenuItem.Click += new System.EventHandler(this.aB0ToolStripMenuItem_Click);
            // 
            // CloseMinusBToolStripMenuItem
            // 
            this.CloseMinusBToolStripMenuItem.Name = "CloseMinusBToolStripMenuItem";
            this.CloseMinusBToolStripMenuItem.Size = new System.Drawing.Size(81, 21);
            this.CloseMinusBToolStripMenuItem.Text = "收盘价 > B";
            this.CloseMinusBToolStripMenuItem.Visible = false;
            this.CloseMinusBToolStripMenuItem.Click += new System.EventHandler(this.CloseMinusBToolStripMenuItem_Click);
            // 
            // KToolStripMenuItem
            // 
            this.KToolStripMenuItem.Name = "KToolStripMenuItem";
            this.KToolStripMenuItem.Size = new System.Drawing.Size(40, 21);
            this.KToolStripMenuItem.Text = "黄K";
            this.KToolStripMenuItem.Visible = false;
            this.KToolStripMenuItem.Click += new System.EventHandler(this.KToolStripMenuItem_Click);
            // 
            // CloseAToolStripMenuItem
            // 
            this.CloseAToolStripMenuItem.Name = "CloseAToolStripMenuItem";
            this.CloseAToolStripMenuItem.Size = new System.Drawing.Size(77, 21);
            this.CloseAToolStripMenuItem.Text = "收盘价 - A";
            this.CloseAToolStripMenuItem.Visible = false;
            this.CloseAToolStripMenuItem.Click += new System.EventHandler(this.CloseAToolStripMenuItem_Click);
            // 
            // AMinToolStripMenuItem
            // 
            this.AMinToolStripMenuItem.Name = "AMinToolStripMenuItem";
            this.AMinToolStripMenuItem.Size = new System.Drawing.Size(77, 21);
            this.AMinToolStripMenuItem.Text = "A - 最低价";
            this.AMinToolStripMenuItem.Visible = false;
            this.AMinToolStripMenuItem.Click += new System.EventHandler(this.AMinToolStripMenuItem_Click);
            // 
            // MeanLineToolStripMenuItem
            // 
            this.MeanLineToolStripMenuItem.Name = "MeanLineToolStripMenuItem";
            this.MeanLineToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.MeanLineToolStripMenuItem.Text = "平均线";
            this.MeanLineToolStripMenuItem.Click += new System.EventHandler(this.MeanLineToolStripMenuItem_Click);
            // 
            // WeekKToolStripMenuItem
            // 
            this.WeekKToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CalcWeekKToolStripMenuItem,
            this.menuLastDay,
            this.Red3ToolStripMenuItem,
            this.Red4ToolStripMenuItem1,
            this.Red5ToolStripMenuItem2});
            this.WeekKToolStripMenuItem.Name = "WeekKToolStripMenuItem";
            this.WeekKToolStripMenuItem.Size = new System.Drawing.Size(76, 21);
            this.WeekKToolStripMenuItem.Text = "周K基准量";
            this.WeekKToolStripMenuItem.Click += new System.EventHandler(this.WeekKToolStripMenuItem_Click);
            // 
            // CalcWeekKToolStripMenuItem
            // 
            this.CalcWeekKToolStripMenuItem.Name = "CalcWeekKToolStripMenuItem";
            this.CalcWeekKToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.CalcWeekKToolStripMenuItem.Text = "计算";
            this.CalcWeekKToolStripMenuItem.Click += new System.EventHandler(this.CalcWeekKToolStripMenuItem_Click);
            // 
            // menuLastDay
            // 
            this.menuLastDay.Name = "menuLastDay";
            this.menuLastDay.Size = new System.Drawing.Size(119, 22);
            this.menuLastDay.Text = "末日";
            this.menuLastDay.Click += new System.EventHandler(this.menuLastDay_Click);
            // 
            // Red3ToolStripMenuItem
            // 
            this.Red3ToolStripMenuItem.Name = "Red3ToolStripMenuItem";
            this.Red3ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.Red3ToolStripMenuItem.Text = "3个红格";
            this.Red3ToolStripMenuItem.Click += new System.EventHandler(this.Red3ToolStripMenuItem_Click);
            // 
            // Red4ToolStripMenuItem1
            // 
            this.Red4ToolStripMenuItem1.Name = "Red4ToolStripMenuItem1";
            this.Red4ToolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.Red4ToolStripMenuItem1.Text = "4个红格";
            this.Red4ToolStripMenuItem1.Click += new System.EventHandler(this.Red4ToolStripMenuItem1_Click);
            // 
            // Red5ToolStripMenuItem2
            // 
            this.Red5ToolStripMenuItem2.Name = "Red5ToolStripMenuItem2";
            this.Red5ToolStripMenuItem2.Size = new System.Drawing.Size(119, 22);
            this.Red5ToolStripMenuItem2.Text = "5个红格";
            this.Red5ToolStripMenuItem2.Click += new System.EventHandler(this.Red5ToolStripMenuItem2_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.AboutToolStripMenuItem.Text = "关于";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // timer_progressBar
            // 
            this.timer_progressBar.Interval = 1000;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 723);
            this.Controls.Add(this.dataGridView_all);
            this.Controls.Add(this.menuStrip_mainMenu);
            this.MainMenuStrip = this.menuStrip_mainMenu;
            this.Name = "MainForm";
            this.Text = "股票数据处理系统";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_all)).EndInit();
            this.menuStrip_mainMenu.ResumeLayout(false);
            this.menuStrip_mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.DataGridView dataGridView_all;
        private System.Windows.Forms.MenuStrip menuStrip_mainMenu;
        private System.Windows.Forms.ToolStripMenuItem CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aB0ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseMinusBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem KToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AMinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MeanLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WeekKToolStripMenuItem;
        private System.Windows.Forms.Timer timer_progressBar;
        private System.Windows.Forms.ToolStripMenuItem CalcWeekKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Red3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Red4ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem Red5ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFile;
        private System.Windows.Forms.ToolStripMenuItem menuLastDay;
        private System.Windows.Forms.ToolStripMenuItem Open_zhuanhuan_ToolStripMenuItem;
    }
}

