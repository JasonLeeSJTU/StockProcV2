using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace StockProc
{
    public partial class MainForm : Form
    {
        String FileName;
        double[] xValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 };//Used for SLOPE
        public static DataTable[] dtCloned;//Store all the values of each file after escaped
        public static DataTable[] dtClonedFive; //多于5行, 即存储
        public static bool[][] color_for_ma;
        //DataTable[] dtClonedCopy;
        DataTable dtTablesAllFiles;
        DataTable dtTableMean;
        DataTable[] dtTableMeanLine;
        public static DataTable[] dtTableMonth;
        public static DataTable[] dtTableWeek;
        DataTable dtTableWeekRed;
        public static DataTable m_dtLastDay; // 末日;

        DataTable dt;// a temp datatable
        double[][] cValues;//Store the C values of each file
        double[][] closeMinusB;// closed price minus B
        bool[][] K;// yellow K;
        int fileNumber;//Store the files number opened
        //int fileNumber_zhuanhuanbao;
        int newFileNumber;// number of opened files after escape all the files that do not have enough rows
        int newFileNumberFive;
        public static String[][] StockInfo;//Store the information of each stock
        public static String[][] StockInfoFive; //Temp stock information
        int[] TotalRows;//Store the number of rows of all the opened file
        double[][] aValues;
        bool fileOpened = false;
        int[] escape;// Escape all the files that do not have enough rows
        int[] escapeFive;
        bool AMinusBClicked = false;// if the menu A - B is clicked
        bool CloseMinusBClicked = false;
        int[] AMinusBTableIndex;
        int[] CloseMinusBTableIndex;
        OleDbConnection oleExcelConnection = default(OleDbConnection);
        bool isStockDetail = false;// click cell to open stock detail
        bool isMeanLine = false;// click cell to open mean line results
        bool isMeanLineCalculated = false;
        ProgressBar pg;
        int currentFileNumber = 0;
        bool isWeekK = false;
        bool isWeekKCalculated = false;
        public static double[] mma60forPingFen; // for color in pingfen in LastDay dialog

        // 使用模态对话框显示进度条
        //public static AbortableBackgroundWorker worker = new AbortableBackgroundWorker();
        public static BackgroundWorker worker;
        ProgressBar workerPB;
        bool isProgressBarCanceled = false;

        public MainForm()
        {
            //worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            //worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            //worker.WorkerReportsProgress = true;
            //worker.WorkerSupportsCancellation = true;


            InitializeComponent();

            dataGridView_all.Visible = false;
            fileOpened = false;
        }

        //Show row number for datagridveiw.
        public static void ShowRowNumber(DataGridView dgv)
        {
            //int rowNumber = dgv.Rows.Count;
            //int i;
            //for (i = 0; i < rowNumber; i++)
            //{
            //    dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            //}
            foreach (DataGridViewRow row in dgv.Rows)
            {
                dgv.Rows[row.Index].HeaderCell.Value = (row.Index + 1).ToString();
            }
        }


        private void dataGridView_all_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView_all.Rows)
            {
                this.dataGridView_all.Rows[row.Index].HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        //If double clicked, show the detail of the current stock
        private void dataGridView_all_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = 0;
            int RowIndex1 = 0;
            if (isStockDetail)
            {
                RowIndex1 = dtTablesAllFiles.Rows.IndexOf(((DataRowView)this.dataGridView_all.CurrentRow.DataBoundItem).Row);
            }
            if (isMeanLine || isWeekK)
            {
                RowIndex1 = dtTableMean.Rows.IndexOf(((DataRowView)this.dataGridView_all.CurrentRow.DataBoundItem).Row);
            }

            if (dt != null)
            {
                int RowIndex2 = dt.Rows.IndexOf(((DataRowView)this.dataGridView_all.CurrentRow.DataBoundItem).Row);
                RowIndex = RowIndex1 >= 0 ? RowIndex1 : RowIndex2;
            }
            else
            {
                RowIndex = RowIndex1;
            }

            
            //sd.Text = dtTablesAllFiles.Rows[RowIndex][1].ToString() + "  " + dtTablesAllFiles.Rows[RowIndex][2].ToString();
            if (isStockDetail)
            {
                StockDetail sd = new StockDetail();

                if (AMinusBClicked)
                {
                    sd.Text = StockInfo[AMinusBTableIndex[RowIndex]][0] + "  " + StockInfo[AMinusBTableIndex[RowIndex]][1];
                    sd.dataGridView_StockDetail.DataSource = dtCloned[AMinusBTableIndex[RowIndex]];
                }
                else if (CloseMinusBClicked)
                {
                    sd.Text = StockInfo[CloseMinusBTableIndex[RowIndex]][0] + "  " + StockInfo[CloseMinusBTableIndex[RowIndex]][1];
                    sd.dataGridView_StockDetail.DataSource = dtCloned[CloseMinusBTableIndex[RowIndex]];
                }
                else
                {
                    //sd.Text = StockInfo[RowIndex][0] + "  " + StockInfo[RowIndex][1];
                    sd.Text = StockInfoFive[RowIndex][0] + "  " + StockInfoFive[RowIndex][1];
                    //sd.dataGridView_StockDetail.DataSource = dtCloned[RowIndex];
                    sd.dataGridView_StockDetail.DataSource = dtClonedFive[RowIndex];
                }

                //sd.dataGridView_StockDetail.Columns["黄K"].SortMode = DataGridViewColumnSortMode.Automatic;
                //sd.dataGridView_StockDetail.Columns["C"].DefaultCellStyle.Format = "0.0000\\%";
                //sd.dataGridView_StockDetail.Columns["收盘-A"].DefaultCellStyle.Format = "0.0000\\%";
                //sd.dataGridView_StockDetail.Columns["收盘-B"].DefaultCellStyle.Format = "0.0000\\%";
                //sd.dataGridView_StockDetail.Columns["A-最低"].DefaultCellStyle.Format = "0.0000\\%";
                dataGridView_all.Sort(dataGridView_all.Columns[0], ListSortDirection.Descending);
                sd.Show();
                //ShowRowNumber(sd.dataGridView_StockDetail);
            }
            else if (isMeanLine)
            {
                StockDetail sd = new StockDetail();

                //sd.Text = StockInfo[RowIndex][0] + "  " + StockInfo[RowIndex][1];
                sd.Text = StockInfoFive[RowIndex][0] + "  " + StockInfoFive[RowIndex][1];
                sd.dataGridView_StockDetail.DataSource = dtTableMeanLine[RowIndex];
                //sd.dataGridView_StockDetail.Columns["ss"].Width = 6;
                sd.dataGridView_StockDetail.Columns[14].DefaultCellStyle.BackColor = Color.LightBlue;
                
                sd.Show();
                ShowRowNumber(sd.dataGridView_StockDetail);
            }
            else if (isWeekK)
            {
                MonthK mk = new MonthK();
                //mk.Text = StockInfo[RowIndex][0] + "  " + StockInfo[RowIndex][1];
                mk.Text = StockInfoFive[RowIndex][0] + "  " + StockInfoFive[RowIndex][1];
                mk.rowMergeView1.DataSource = dtTableMonth[RowIndex];
                mk.rowMergeView1.ColumnHeadersHeight = 40;
                mk.rowMergeView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                mk.rowMergeView1.MergeColumnNames.Add("year");
                //mk.rowMergeView1.MergeColumnNames.Add("month");
                mk.StartPosition = FormStartPosition.Manual;
                mk.Location = new Point(10, 10);
                mk.Show();
                ShowRowNumber(mk.rowMergeView1);

                WeekK wk = new WeekK();
                wk.m_iTableIdx = RowIndex;
                //wk.Text = StockInfo[RowIndex][0] + "  " + StockInfo[RowIndex][1];
                wk.Text = StockInfoFive[RowIndex][0] + "  " + StockInfoFive[RowIndex][1];
                wk.rowMergeView_weekK.DataSource = dtTableWeek[RowIndex];
                wk.rowMergeView_weekK.ColumnHeadersHeight = 40;
                wk.rowMergeView_weekK.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                wk.rowMergeView_weekK.MergeColumnNames.Add("year");
                wk.rowMergeView_weekK.MergeColumnNames.Add("month");
                wk.rowMergeView_weekK.MergeColumnNames.Add("week");
                wk.StartPosition = FormStartPosition.Manual;
                wk.Location = new Point(500, 10);

                wk.Show();
                ShowRowNumber(wk.rowMergeView_weekK);
            }
            ShowRowNumber(dataGridView_all);
        }

        //Show C values
        private void CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AMinusBClicked = false;
            CloseMinusBClicked = false;
            isStockDetail = true;
            isMeanLine = false;
            isWeekK = false;

            if (!dtTablesAllFiles.Columns.Contains("C"))
            {
                dtTablesAllFiles.Columns.Add("C", typeof(double));

                int idx = 0;
                for (int i = 0; i < newFileNumber; i++)
                {
                    while (escape[idx] > 0)
                    {
                       idx++;// escaped files
                    }

                    dtTablesAllFiles.Rows[i][6] = cValues[idx][TotalRows[idx] - 1] / (cValues[idx][TotalRows[idx] - 1] + aValues[idx][TotalRows[idx] - 1]);
                    idx++;
                }
            }
            this.dataGridView_all.DataSource = dtTablesAllFiles;
            dataGridView_all.Sort(dataGridView_all.Columns[6], ListSortDirection.Ascending);
            dataGridView_all.Columns[6].DefaultCellStyle.Format = "0.0000\\%";
            ShowRowNumber(dataGridView_all);
        }

        // show the result if A - B = 0
        private void aB0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AMinusBClicked = true;
            CloseMinusBClicked = false;
            isStockDetail = true;
            isMeanLine = false;
            isWeekK = false;

            dt = dtTablesAllFiles.Copy();
            int columnNumber = dtTablesAllFiles.Columns.Count;
            for (int i = 6; i < columnNumber; i++)
            {
                dt.Columns.RemoveAt(i);//Only reserve the first 6 columns
            }
            dt.Columns.Add("A - B = 0", typeof(bool));// add the 7th column
            dt.Columns[6].DefaultValue = true;

            int idx = 0;
            //for (int i = 0; i < newFileNumber; i++)
            //{
            //    while (escape[idx] > 0)
            //    {
            //        idx++;// escaped files
            //    }
            //    if (cValues[idx][TotalRows[idx] - 1] != 0)// remove all the stock that C not equals 0
            //    {
            //        dt.Rows[i].Delete();
            //    }
            //    idx++;
            //}
            int rowIndex = 0;
            int RemainRow = 0;
            foreach (DataRow row in dt.Select())
            {
                while (escape[idx] > 0)
                {
                    idx++;
                }
                if (cValues[idx][TotalRows[idx] - 1] != 0)// remove all the stock that C not equals 0
                {
                    row.Delete();
                    rowIndex++;
                }
                else
                {
                    AMinusBTableIndex[RemainRow++] = rowIndex++;
                }
                idx++;
            }
            dt.AcceptChanges();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][6] = true;
            }

            this.dataGridView_all.DataSource = dt;
            ShowRowNumber(dataGridView_all);
        }

        // show the result if close - B > 0
        private void CloseMinusBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AMinusBClicked = false;
            CloseMinusBClicked = true;
            isStockDetail = true;
            isMeanLine = false;
            isWeekK = false;

            dt = dtTablesAllFiles.Copy();
            int columnNumber = dtTablesAllFiles.Columns.Count;
            for (int i = 6; i < columnNumber; i++)
            {
                dt.Columns.RemoveAt(i);//Only reserve the first 6 columns
            }
            dt.Columns.Add("收盘 > B", typeof(bool));// add the 7th column
            dt.Columns[6].DefaultValue = 1;

            int idx = 0;
            int rowIndex = 0;
            int RemainRow = 0;
            foreach (DataRow row in dt.Select())
            {
                while (escape[idx] > 0)
                {
                    idx++;
                }
                if (closeMinusB[idx][TotalRows[idx] - 1] <= 0)
                {
                    row.Delete();
                    rowIndex++;
                }
                else
                {
                    CloseMinusBTableIndex[RemainRow++] = rowIndex++;
                }
                idx++;
            }
            dt.AcceptChanges();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][6] = true;
            }

            this.dataGridView_all.DataSource = dt;
            ShowRowNumber(dataGridView_all);
        }

        // yellow K
        private void KToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AMinusBClicked = false;
            CloseMinusBClicked = false;
            isStockDetail = true;
            isMeanLine = false;
            isWeekK = false;

            dt = dtTablesAllFiles.Copy();
            int columnNumber = dtTablesAllFiles.Columns.Count;
            for (int i = 6; i < columnNumber; i++)
            {
                dt.Columns.RemoveAt(i);//Only reserve the first 6 columns
            }
            dt.Columns.Add("黄K", typeof(bool));// add the 7th column
            dt.Columns[6].DefaultValue = 1;

            int idx = 0;
            int rowIndex = 0;
            int RemainRow = 0;
            foreach (DataRow row in dt.Select())
            {
                int rowCount = dtCloned[idx].Rows.Count;
                double close = (double)dtCloned[idx].Rows[rowCount - 1][4];
                double closeOneDayAgo = (double)dtCloned[idx].Rows[rowCount - 2][4];
                double open = (double)dtCloned[idx].Rows[rowCount - 1][1];
                if (!(close > 1.098 * closeOneDayAgo && close > open))// remove all the stock that not K
                {
                    row.Delete();
                    rowIndex++;
                }
                else
                {
                    CloseMinusBTableIndex[RemainRow++] = rowIndex++;
                }
                idx++;
            }
            dt.AcceptChanges();


            this.dataGridView_all.DataSource = dt;
            //DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();

            //col.HeaderText = "黄K";
            //dataGridView_all.Columns.Add(col);

            for (int i = 0; i < dataGridView_all.Rows.Count; i++)
            {
                dataGridView_all.Rows[i].Cells[6].Value = true;
            }

            //dataGridView_all.Columns[6].Visible = false;
            ShowRowNumber(dataGridView_all);
        }

        //收盘价-数值A 找出最接近于0的股票 排序出来
        private void CloseAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AMinusBClicked = false;
            CloseMinusBClicked = false;
            isStockDetail = true;
            isMeanLine = false;
            isWeekK = false;

            dt = dtTablesAllFiles.Copy();
            int columnNumber = dtTablesAllFiles.Columns.Count;
            for (int i = 6; i < columnNumber; i++)
            {
                dt.Columns.RemoveAt(i);//Only reserve the first 6 columns
            }
            dt.Columns.Add("收盘 - A", typeof(double));// add the 7th column

            for (int i = 0; i < newFileNumber; i++)
            {
                int rowCount = dtCloned[i].Rows.Count;
                dt.Rows[i][6] = Math.Round((double)dtCloned[i].Rows[rowCount - 1][4] - (double)dtCloned[i].Rows[rowCount - 1][7], 2);
            }

            this.dataGridView_all.DataSource = dt;
            dataGridView_all.Sort(dataGridView_all.Columns[6], ListSortDirection.Ascending);
            dataGridView_all.Columns[6].DefaultCellStyle.Format = "0.0000\\%";
            ShowRowNumber(dataGridView_all);
        }

        // A-最低价
        private void AMinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AMinusBClicked = false;
            CloseMinusBClicked = false;
            isStockDetail = true;
            isMeanLine = false;
            isWeekK = false;

            dt = dtTablesAllFiles.Copy();
            int columnNumber = dtTablesAllFiles.Columns.Count;
            for (int i = 6; i < columnNumber; i++)
            {
                dt.Columns.RemoveAt(i);//Only reserve the first 6 columns
            }
            dt.Columns.Add("A-最低", typeof(double));// add the 7th column

            for (int i = 0; i < newFileNumber; i++)
            {
                int rowCount = dtCloned[i].Rows.Count;
                dt.Rows[i][6] = (double)dtCloned[i].Rows[rowCount - 1][13];
            }

            this.dataGridView_all.DataSource = dt;
            dataGridView_all.Sort(dataGridView_all.Columns[6], ListSortDirection.Ascending);
            dataGridView_all.Columns[6].DefaultCellStyle.Format = "0.0000\\%";
            ShowRowNumber(dataGridView_all);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info inf = new Info();
            inf.StartPosition = FormStartPosition.CenterParent;
            inf.ShowDialog();
        }

        // Mean line
        // This is another function
        // It needs to open one file
        private void MeanLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！");
                return;
            }

            if (!isMeanLineCalculated)
            {
                //pg = new ProgressBar();
                //pg.progressBar_MeanLine.Value = 0;
                //pg.progressBar_MeanLine.Maximum = newFileNumber;
                //pg.progressBar_MeanLine.Minimum = 0;
                ////timer_progressBar.Enabled = true;
                //currentFileNumber = 0;
                //pg.Text = "正在计算平均线......";
                //pg.Show();
               

                isStockDetail = false;
                isWeekK = false;
                isMeanLine = true;
                isMeanLineCalculated = true;
                //dataGridView_all.DataSource = dtTableMean;
                //dtTableMeanLine = new DataTable[newFileNumber];
                dtTableMeanLine = new DataTable[fileNumber];
                isProgressBarCanceled = false;

                workerPB = new ProgressBar();
                workerPB.progressBar_MeanLine.Value = 0;
                //workerPB.progressBar_MeanLine.Maximum = newFileNumber;
                workerPB.progressBar_MeanLine.Maximum = fileNumber;
                workerPB.progressBar_MeanLine.Minimum = 0;
                currentFileNumber = 0;

                worker = new BackgroundWorker();
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork_MeanLine);
                worker.RunWorkerAsync();
                workerPB.Text = "正在计算平均线 ……";
                workerPB.StartPosition = FormStartPosition.CenterParent;
                workerPB.ShowDialog();


                // calculated in function worker_DoWork_MeanLine;
            }
            else
            {
                isStockDetail = false;
                isMeanLine = true;
                isWeekK = false;
            }

            if (!isProgressBarCanceled)
            {
                dataGridView_all.DataSource = dtTableMean;
                ShowRowNumber(dataGridView_all);
            }
            else
            {
                if (!isWeekKCalculated)
                {
                    isStockDetail = true;
                    isMeanLine = false;
                    isMeanLineCalculated = false;
                }
                else
                {
                    isWeekK = true;
                }
                isProgressBarCanceled = false;
            }
        }

        //MA = (C1+C2+C3+C4+C5+....+Cn)/n 
        private double ma(int days, int today, DataTable dt)
        {
            double sum = 0;
            for (int i = 0; i < days; i++)
            {
                sum += (double)dt.Rows[today - i][4];
            }
            return sum / days;
        }

        //MMA5=（今天5天平均线价格X 5+今天收盘价格-5天前收盘价格）/5 
        private double mma(int days, int today, DataTable dt)
        {
            double value = ma(days, today, dt);
            value = value * days + (double)dt.Rows[today][4] - (double)dt.Rows[today - days][4];
            return value / days;
        }

        private void WeekKToolStripMenuItem_Click1(object sender, EventArgs e)
        {
            //open Excel files
            this.openFileDialog.Multiselect = false; // Disable select multiple files
            this.openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            //Using MS Office Interop is slow and calling .Value2 is an expensive operation
            //So we use OLEDB
            string sConnection = null;
            DataTable dtTablesList = default(DataTable);
            DataTable dtTablesValue = default(DataTable);
            //OleDbCommand oleExcelCommand = default(OleDbCommand);
            //OleDbDataReader oleExcelReader = default(OleDbDataReader);
            //OleDbConnection oleExcelConnection = default(OleDbConnection);
            string sSheetName = null;
            System.Data.OleDb.OleDbDataAdapter adp;

            //fileNumber = 0;

            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string FileName = this.openFileDialog.FileName;
                    dataGridView_all.Visible = false;
                    AMinusBClicked = false;
                    CloseMinusBClicked = false;
                    if (oleExcelConnection != null && oleExcelConnection.State == ConnectionState.Open)
                    {
                        oleExcelConnection.Close();
                    }

                    try
                    {
                        Process[] ps = Process.GetProcesses();
                        foreach (Process p in ps)
                        {
                            if (p.MainWindowTitle.Contains("Excel"))
                            {
                                fileOpened = false;
                                MessageBox.Show("请先关闭已经打开的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        openFileDialog.OpenFile();
                        fileOpened = true;
                    }
                    catch
                    {
                        fileOpened = false;
                        MessageBox.Show("请先关闭已经打开的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    fileOpened = false;

                    sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Mode=Read;Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";
                    oleExcelConnection = new OleDbConnection(sConnection);
                    bool isconopen = false;
                    fileOpened = false;
                    try
                    {
                        oleExcelConnection.Open();
                        isconopen = true;
                        fileOpened = true;
                    }
                    catch
                    {
                        oleExcelConnection.Close();
                        isconopen = false;
                        fileOpened = false;
                        MessageBox.Show("请打开标准格式的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!isconopen)
                    {
                        return;
                    }

                    dtTablesList = oleExcelConnection.GetSchema("Tables");

                    if (dtTablesList.Rows.Count > 0)
                    {
                        sSheetName = dtTablesList.Rows[0]["TABLE_NAME"].ToString();
                    }

                    adp = new OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", sSheetName), oleExcelConnection);
                    dtTablesList.Clear();
                    dtTablesList.Dispose();

                    adp.Fill(dtTablesList);
                    dtTablesValue = dtTablesList.Copy();
                    int i = 0;
                    for (i = 0; i < 9; i++)//delete the first 9 columns in the datatable
                    {
                        dtTablesValue.Columns.RemoveAt(0);//delete 9 times
                    }
                    //change the columns' name
                    for (i = 0; i < 7; i++)
                    {
                        dtTablesValue.Columns[i].ColumnName = Regex.Replace(dtTablesValue.Rows[1][i].ToString(), @"\s+", "");
                    }
                    //Get the stock number and name, they are the first and second values
                    string str = dtTablesValue.Rows[0][0].ToString();
                    string[] StockInfo = str.Split(' ');
                    dtTablesValue.Rows.RemoveAt(0);
                    dtTablesValue.Rows.RemoveAt(0);//delete the first two rows;
                    dtTablesValue.Rows.RemoveAt(dtTablesValue.Rows.Count - 1);//delete the last row.

                    DataTable dtTable = dtTablesValue.Clone();

                    dtTable.Columns[0].DataType = typeof(DateTime);
                    dtTable.Columns[1].DataType = typeof(double);
                    dtTable.Columns[2].DataType = typeof(double);
                    dtTable.Columns[3].DataType = typeof(double);
                    dtTable.Columns[4].DataType = typeof(double);
                    dtTable.Columns[5].DataType = typeof(double);
                    dtTable.Columns[6].DataType = typeof(double);


                    foreach (DataRow row in dtTablesValue.Rows)
                    {
                        dtTable.ImportRow(row);
                    }


                    DataTable dtMonth = new DataTable("dtmonth");
                    dtMonth.Columns.Add("col1");
                    dtMonth.Columns.Add("col2");
                    dtMonth.Columns.Add("col3");
                    dtMonth.Columns.Add("col4");
                    dtMonth.Columns.Add("col5");

                    int month = 0;
                    int year = 0;
                    int dayCount = 0;
                    double moneyCount = 0;
                    int TotalRows = dtTable.Rows.Count;
                    for (i=0; i<TotalRows; i++)
                    {
                        int y = ((DateTime)dtTable.Rows[i][0]).Year;
                        int m = ((DateTime)dtTable.Rows[i][0]).Month;
                        if (i == 0)
                        {
                            year = y;
                            month = m;
                        }
                        if (year == y)// the same year
                        {
                            if (month == m)// the same month
                            {
                                moneyCount += (double)dtTable.Rows[i][6];
                                dayCount++;
                            }
                            else
                            {
                                DataRow row = dtMonth.NewRow();
                                row[0] = year;
                                row[1] = month;
                                row[2] = dayCount;
                                row[3] = Math.Round(moneyCount/100000000,2);
                                row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                                dtMonth.Rows.Add(row);
                                month = m;
                                dayCount = 1;
                                moneyCount = (double)dtTable.Rows[i][6];
                            }
                        }
                        else
                        {
                            DataRow row = dtMonth.NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = dayCount;
                            row[3] = Math.Round(moneyCount / 100000000, 2);
                            row[4] = Math.Round(moneyCount / dayCount/100000000,2);
                            dtMonth.Rows.Add(row);

                            dayCount = 1;
                            moneyCount = (double)dtTable.Rows[i][6];
                            year = y;
                            month = m;
                        }

                        if (i == TotalRows - 1)
                        {
                            DataRow row = dtMonth.NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = dayCount;
                            row[3] = Math.Round(moneyCount / 100000000, 2);
                            row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                            dtMonth.Rows.Add(row);
                        }
                    }

                    MonthK mk = new MonthK();
                    mk.rowMergeView1.DataSource = dtMonth;
                    mk.rowMergeView1.ColumnHeadersHeight = 40;
                    mk.rowMergeView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    mk.rowMergeView1.MergeColumnNames.Add("year");
                    //mk.rowMergeView1.MergeColumnNames.Add("month");
                    mk.StartPosition = FormStartPosition.Manual;
                    mk.Location = new Point(10, 10);
                    mk.Show();

                    // calculate week K
                    DataTable dtWeek = new DataTable("dtweek");
                    dtWeek.Columns.Add("col0");
                    dtWeek.Columns.Add("col01");
                    dtWeek.Columns.Add("col1");
                    dtWeek.Columns.Add("col2");
                    dtWeek.Columns.Add("col3");
                    dtWeek.Columns.Add("col4");
                    dtWeek.Columns.Add("col5");
                    dtWeek.Columns.Add("col6");
                    dtWeek.Columns.Add("col7");
                    dtWeek.Columns.Add("col8");

                    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                    Calendar cal = dfi.Calendar;
                    int week=0, day=0;
                    int RowIdxCurrent = 0;
                    bool calStand = false;
                    double standard = 0; //base value
                    double moneyTotal = 0;
                    double difTotal = 0;
                    for (i = 0; i < TotalRows; i++)
                    {
                        DateTime dt = (DateTime)dtTable.Rows[i][0];
                        int y = dt.Year;
                        int m = dt.Month;
                        int w = cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        day = (int)dt.DayOfWeek;

                        if (year == y)//the same year
                        {
                            if (month == m)// the same month
                            {
                                if (!calStand)// get the base value of last month
                                {
                                    for (int ii = 0; ii < dtMonth.Rows.Count; ii++)
                                    {
                                        if (year.ToString() == dtMonth.Rows[ii][0].ToString() && month.ToString() == dtMonth.Rows[ii][1].ToString())
                                        {
                                            if (i == 0)
                                            {
                                                standard = 0;
                                                calStand = true;
                                                break;
                                            }
                                            else
                                            {
                                                standard = Convert.ToDouble(dtMonth.Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                                calStand = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (week == w)
                                {
                                    double money = (double)dtTable.Rows[i][6];
                                    switch (day)
                                    {
                                        case 1:
                                            dtWeek.Rows[RowIdxCurrent][4] = Math.Round(money/100000000,2);
                                            dtWeek.Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 2:
                                            dtWeek.Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 3:
                                            dtWeek.Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 4:
                                            dtWeek.Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 5:
                                            dtWeek.Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    dtWeek.Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2); ;
                                    dtWeek.Rows[RowIdxCurrent + 1][9] = difTotal;
                                    moneyTotal = 0;
                                    difTotal = 0;
                                    RowIdxCurrent += 2;

                                    week = w;

                                    DataRow dwRow = dtWeek.NewRow();
                                    dwRow[0] = year;
                                    dwRow[1] = month;
                                    dwRow[2] = "第" + week.ToString() + "周";
                                    dwRow[3] = "成交量";
                                    dtWeek.Rows.Add(dwRow);

                                    DataRow row = dtWeek.NewRow();
                                    row[0] = year;
                                    row[1] = month;
                                    row[2] = "第" + week.ToString() + "周";
                                    row[3] = "成-基";
                                    dtWeek.Rows.Add(row);

                                    double money = (double)dtTable.Rows[i][6];
                                    switch (day)
                                    {
                                        case 1:
                                            dtWeek.Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 2:
                                            dtWeek.Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 3:
                                            dtWeek.Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 4:
                                            dtWeek.Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 5:
                                            dtWeek.Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                            dtWeek.Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else//month != m
                            {
                                if (dtWeek.Rows.Count > 0)
                                {
                                    dtWeek.Rows[RowIdxCurrent][9] = Math.Round(moneyTotal/100000000,2);
                                    dtWeek.Rows[RowIdxCurrent + 1][9] = difTotal;
                                    RowIdxCurrent += 2;
                                }

                                month = m;
                                week = w;
                                calStand = false;
                                moneyTotal = 0;
                                difTotal = 0;

                                DataRow dwRow = dtWeek.NewRow();
                                dwRow[0] = year;
                                dwRow[1] = month;
                                dwRow[2] = "第" + week.ToString() + "周";
                                dwRow[3] = "成交量";
                                dtWeek.Rows.Add(dwRow);

                                DataRow row = dtWeek.NewRow();
                                row[0] = year;
                                row[1] = month;
                                row[2] = "第" + week.ToString() + "周";
                                row[3] = "成-基";
                                dtWeek.Rows.Add(row);


                                if (!calStand)// get the base value of last month
                                {
                                    for (int ii = 0; ii < dtMonth.Rows.Count; ii++)
                                    {
                                        if (year == Convert.ToDouble(dtMonth.Rows[ii][0].ToString()) && month == Convert.ToDouble(dtMonth.Rows[ii][1].ToString()))
                                        {
                                            if (i == 0)
                                            {
                                                standard = 0;
                                                calStand = true;
                                                break;
                                            }
                                            else
                                            {
                                                standard = Convert.ToDouble(dtMonth.Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                                calStand = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                double money = (double)dtTable.Rows[i][6];
                                switch (day)
                                {
                                    case 1:
                                        dtWeek.Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 2);
                                        dtWeek.Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 2:
                                        dtWeek.Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                        dtWeek.Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 3:
                                        dtWeek.Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                        dtWeek.Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 4:
                                        dtWeek.Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                        dtWeek.Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 5:
                                        dtWeek.Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                        dtWeek.Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else//year != y
                        {
                            if (i != 0)
                            {
                                dtWeek.Rows[RowIdxCurrent][9] = Math.Round(moneyTotal/100000000,2);
                                dtWeek.Rows[RowIdxCurrent + 1][9] = difTotal;
                                RowIdxCurrent += 2;
                            }

                            year = y;
                            month = m;
                            week = w;
                            moneyTotal = 0;
                            difTotal = 0;
                            DataRow dwRow = dtWeek.NewRow();
                            dwRow[0] = year;
                            dwRow[1] = month;
                            dwRow[2] = "第" + week.ToString() + "周";
                            dwRow[3] = "成交量";
                            dtWeek.Rows.Add(dwRow);

                            DataRow row = dtWeek.NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = "第" + week.ToString() + "周";
                            row[3] = "成-基";
                            dtWeek.Rows.Add(row);
                            calStand = false;

                            if (!calStand)// get the base value of last month
                            {
                                for (int ii = 0; ii < dtMonth.Rows.Count; ii++)
                                {
                                    if (year.ToString() == dtMonth.Rows[ii][0].ToString() && month.ToString() == dtMonth.Rows[ii][1].ToString())
                                    {
                                        if (i == 0)
                                        {
                                            standard = 0;
                                            calStand = true;
                                            break;
                                        }
                                        else
                                        {
                                            standard = Convert.ToDouble(dtMonth.Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                            calStand = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            double money = (double)dtTable.Rows[i][6];
                            switch (day)
                            {
                                case 1:
                                    dtWeek.Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 2);
                                    dtWeek.Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 2:
                                    dtWeek.Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                    dtWeek.Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 3:
                                    dtWeek.Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                    dtWeek.Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 4:
                                    dtWeek.Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                    dtWeek.Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 5:
                                    dtWeek.Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                    dtWeek.Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard,2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money/100000000,2) - standard,2);
                                    break;
                                default:
                                    break;
                            }

                        }

                        if (i == TotalRows-1)
                        {
                            dtWeek.Rows[RowIdxCurrent][9] = Math.Round(moneyTotal/100000000,2);
                            dtWeek.Rows[RowIdxCurrent + 1][9] = difTotal;
                        }
                    }

                    WeekK wk = new WeekK();
                    wk.rowMergeView_weekK.DataSource = dtWeek;
                    wk.rowMergeView_weekK.ColumnHeadersHeight = 40;
                    wk.rowMergeView_weekK.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    wk.rowMergeView_weekK.MergeColumnNames.Add("year");
                    wk.rowMergeView_weekK.MergeColumnNames.Add("month");
                    wk.rowMergeView_weekK.MergeColumnNames.Add("week");
                    wk.StartPosition = FormStartPosition.Manual;
                    wk.Location = new Point(500, 10);
                    
                    wk.Show();
                }
                finally
                {
                    oleExcelConnection.Close();
                }
            }




            //WeekK wk = new WeekK();

            //DataTable dt = new DataTable();
            //dt.Columns.Add("col1");
            //dt.Columns.Add("col2");
            //dt.Columns.Add("col3");
            //dt.Columns.Add("col4");
            //dt.Columns.Add("col5");
            //dt.Columns.Add("col6");
            //dt.Columns.Add("col7");
            //dt.Columns.Add("col8");
            //dt.Rows.Add("中国", "上海", "5000", "7000", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("中国", "北京", "3000", "5600", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("美国", "纽约", "6000", "8600", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("美国", "华劢顿", "8000", "9000", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("英国", "伦敦", "7000", "8800", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("英国", "伦敦", "7000", "8800", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("xx", "伦敦", "7000", "8800", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("xx", "伦敦", "7000", "8800", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("y", "伦敦", "7000", "8800", "5000", "7000", "5000", "7000");
            //dt.Rows.Add("y", "伦敦", "7000", "8800", "5000", "7000", "5000", "7000");
            //wk.rowMergeView_weekK.DataSource = dt;
            //wk.rowMergeView_weekK.ColumnHeadersHeight = 40;
            //wk.rowMergeView_weekK.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //wk.rowMergeView_weekK.MergeColumnNames.Add("week");
            ////wk.rowMergeView_weekK.AddSpanHeader(2, 2, "XXXX");

            //wk.Show();
        }

        private void OpenExcelFileToolStripMenuItem_Click1(object sender, EventArgs e)
        {
            //open Excel files
            this.openFileDialog.Multiselect = true; //Enable select multiple files
            this.openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            //Using MS Office Interop is slow and calling .Value2 is an expensive operation
            //So we use OLEDB
            /*
            Excel.Application excel = null;
            Excel.Workbook wb = null;
            Excel.Worksheet st = null;
            Excel.Range range = null;
            */
            string sConnection = null;
            DataTable dtTablesList = default(DataTable);
            DataTable dtTablesValue = default(DataTable);
            //OleDbCommand oleExcelCommand = default(OleDbCommand);
            //OleDbDataReader oleExcelReader = default(OleDbDataReader);

            string sSheetName = null;
            System.Data.OleDb.OleDbDataAdapter adp;

            //fileNumber = 0;

            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string Name = this.openFileDialog.FileName;
                fileNumber = openFileDialog.FileNames.Length;
                dataGridView_all.Visible = false;
                AMinusBClicked = false;
                CloseMinusBClicked = false;
                if (oleExcelConnection != null && oleExcelConnection.State == ConnectionState.Open)
                {
                    oleExcelConnection.Close();
                }

                try
                {
                    Process[] ps = Process.GetProcesses();
                    foreach (Process p in ps)
                    {
                        if (p.MainWindowTitle.Contains("Excel"))
                        {
                            fileOpened = false;
                            MessageBox.Show("请先关闭已经打开的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    openFileDialog.OpenFile();
                    fileOpened = true;
                }
                catch
                {
                    fileOpened = false;
                    MessageBox.Show("请先关闭已经打开的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //if (openFileDialog.OpenFile() != null)
                {
                    //fileNumber = openFileDialog.FileNames.Length;
                    fileOpened = false;

                    StockInfo = new string[fileNumber][];//Stock number and name of each file
                    DataTable[] dtClonedTemp = new DataTable[fileNumber];
                    TotalRows = new int[fileNumber];
                    cValues = new double[fileNumber][];//C values
                    closeMinusB = new double[fileNumber][];
                    K = new bool[fileNumber][];
                    aValues = new double[fileNumber][];
                    escape = new int[fileNumber];
                    Array.Clear(escape, 0, fileNumber);// Initial escape with 0

                    for (int fi = 0; fi < fileNumber; fi++)
                    {
                        try
                        {
                            FileName = openFileDialog.FileNames[fi].ToString();
                            //FileNames.Add(FileName);

                            /*
                            excel = new Excel.Application();
                            wb = excel.Workbooks.Open(FileName);
                            wb.SaveAs("C:\\Users\\Jason\\Downloads\\文献\\A股股票\\xx.xls");
                            wb.Close();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                            excel.Quit();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);*/
                            //string fileExtension = System.IO.Path.GetExtension(FileName);
                            sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Mode=Read;Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";
                            oleExcelConnection = new OleDbConnection(sConnection);
                            bool isconopen = false;
                            fileOpened = false;
                            try
                            {
                                oleExcelConnection.Open();
                                isconopen = true;
                                fileOpened = true;
                            }
                            catch
                            {
                                oleExcelConnection.Close();
                                isconopen = false;
                                fileOpened = false;
                                MessageBox.Show("请打开标准格式的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (!isconopen)
                            {
                                return;
                            }

                            dtTablesList = oleExcelConnection.GetSchema("Tables");

                            if (dtTablesList.Rows.Count > 0)
                            {
                                sSheetName = dtTablesList.Rows[0]["TABLE_NAME"].ToString();
                            }

                            adp = new OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", sSheetName), oleExcelConnection);
                            dtTablesList.Clear();
                            dtTablesList.Dispose();

                            adp.Fill(dtTablesList);
                            dtTablesValue = dtTablesList.Copy();
                            int i = 0;
                            for (i = 0; i < 9; i++)//delete the first 9 columns in the datatable
                            {
                                dtTablesValue.Columns.RemoveAt(0);//delete 9 times
                            }
                            //change the columns' name
                            for (i = 0; i < 7; i++)
                            {
                                dtTablesValue.Columns[i].ColumnName = Regex.Replace(dtTablesValue.Rows[1][i].ToString(), @"\s+", "");
                            }
                            //Get the stock number and name, they are the first and second values
                            string str = dtTablesValue.Rows[0][0].ToString();
                            StockInfo[fi] = str.Split(' ');
                            dtTablesValue.Rows.RemoveAt(0);
                            dtTablesValue.Rows.RemoveAt(0);//delete the first two rows;
                            dtTablesValue.Rows.RemoveAt(dtTablesValue.Rows.Count - 1);//delete the last row.

                            dtClonedTemp[fi] = dtTablesValue.Clone();
                            dtClonedTemp[fi].Columns[0].DataType = typeof(DateTime);
                            dtClonedTemp[fi].Columns[1].DataType = typeof(double);
                            dtClonedTemp[fi].Columns[2].DataType = typeof(double);
                            dtClonedTemp[fi].Columns[3].DataType = typeof(double);
                            dtClonedTemp[fi].Columns[4].DataType = typeof(double);
                            dtClonedTemp[fi].Columns[5].DataType = typeof(double);
                            dtClonedTemp[fi].Columns[6].DataType = typeof(double);
                            foreach (DataRow row in dtTablesValue.Rows)
                            {
                                dtClonedTemp[fi].ImportRow(row);
                            }


                            //Calculate SLOPE of 21 days
                            //Mimic Excel SLOPE function
                            //Slope(b) = (NΣXY - (ΣX)(ΣY)) / (NΣX2 - (ΣX)2)
                            TotalRows[fi] = dtClonedTemp[fi].Rows.Count;
                            aValues[fi] = new double[TotalRows[fi]];//Store A values;
                            cValues[fi] = new double[TotalRows[fi]];//Store C values;
                            closeMinusB[fi] = new double[TotalRows[fi]];
                            K[fi] = new bool[TotalRows[fi]];

                            if (TotalRows[fi] < 75)
                            {
                                oleExcelConnection.Close();
                                escape[fi] = 1;
                                //fileNumber--;
                                continue;
                            }

                            int j, k;
                            double sumXY = 0, sumX = 0, sumY = 0, sumX2 = 0;
                            double[] slopes = new double[TotalRows[fi]];//Store slopes of every 21 days
                            double[] bValues = new double[TotalRows[fi]];//Store B values;
                            double[] yValues = new double[21];
                            double[] closeMinusA = new double[TotalRows[fi]];
                            for (i = 0; i < TotalRows[fi]; i++)//the first two rows of range should be escaped
                            {
                                k = i;
                                sumXY = 0; sumX = 0; sumY = 0; sumX2 = 0;

                                for (j = 0; j < 21; j++)
                                {
                                    if (k >= TotalRows[fi])
                                        break;//Already touch the end row

                                    yValues[j] = (double)dtClonedTemp[fi].Rows[k][4];
                                    k++;

                                    sumXY += xValues[j] * yValues[j];
                                    sumX += xValues[j];
                                    sumY += yValues[j];
                                    sumX2 += xValues[j] * xValues[j];
                                }

                                slopes[k - 1] = (sumXY * 21 - sumX * sumY) / (21 * sumX2 - sumX * sumX);
                                slopes[k - 1] = slopes[k - 1] * 20 + (double)dtClonedTemp[fi].Rows[k - 1][4];
                                if (k == TotalRows[fi])
                                    break;
                            }

                            //Calculate the 55 days EMA
                            double sumTemp = 0;
                            for (i = 0; i < TotalRows[fi]; i++)//the first two rows of range should be escaped
                            {
                                k = i + 20;
                                if (k == 20)//Only calculate for once
                                {
                                    sumTemp = 0;
                                    for (j = 0; j < 55; j++)
                                    {
                                        sumTemp += slopes[k++];//slopes index start from 22
                                    }

                                    aValues[fi][74] = sumTemp / 55;
                                }

                                k = i + 75; //aValues index starts from 74
                                aValues[fi][k] = (slopes[k] + aValues[fi][k - 1] * 27) / 28;
                                aValues[fi][k - 1] = Math.Round(aValues[fi][k - 1], 2);//Round the value after it has been used
                                if (k + 1 == TotalRows[fi])
                                {
                                    aValues[fi][k] = Math.Round(aValues[fi][k], 2);//Round the value of the end row
                                    break;
                                }
                            }

                            //Calculate B values
                            double[] bTemp = new double[60];
                            for (i = 0; i < TotalRows[fi]; i++)
                            {
                                k = i;
                                for (j = 0; j < 60; j++)
                                {
                                    bTemp[j] = aValues[fi][k++];
                                }
                                bValues[k - 1] = bTemp.Max();
                                if (k == TotalRows[fi])
                                {
                                    break;
                                }
                            }

                            dtClonedTemp[fi].Columns.Add("A", typeof(double));
                            dtClonedTemp[fi].Columns.Add("B", typeof(double));
                            dtClonedTemp[fi].Columns.Add("黄K", typeof(bool));
                            dtClonedTemp[fi].Columns.Add("C", typeof(double));
                            dtClonedTemp[fi].Columns.Add("收盘-A", typeof(double));
                            dtClonedTemp[fi].Columns.Add("收盘-B", typeof(double));
                            dtClonedTemp[fi].Columns.Add("A-最低", typeof(double));
                            for (i = 0; i < TotalRows[fi]; i++)
                            {
                                cValues[fi][i] = Math.Round(bValues[i] - aValues[fi][i], 2);//Calculate C values
                                dtClonedTemp[fi].Rows[i][7] = aValues[fi][i];
                                dtClonedTemp[fi].Rows[i][8] = bValues[i];
                                closeMinusB[fi][i] = Math.Round((double)dtClonedTemp[fi].Rows[i][4] - bValues[i], 2);
                                closeMinusA[i] = Math.Round((double)dtClonedTemp[fi].Rows[i][4] - aValues[fi][i], 2);
                                if (i == 0)
                                {
                                    K[fi][i] = false;
                                }
                                else
                                {
                                    double open = (double)dtClonedTemp[fi].Rows[i][1];
                                    double closeOneDayAgo = (double)dtClonedTemp[fi].Rows[i - 1][4];
                                    double close = (double)dtClonedTemp[fi].Rows[i][4];
                                    if (close > 1.098 * closeOneDayAgo && close > open)// calculate K
                                    {
                                        K[fi][i] = true;
                                    }
                                }


                                dtClonedTemp[fi].Rows[i][9] = K[fi][i];
                                if (bValues[i] == 0)
                                {
                                    //dtClonedTemp[fi].Rows[i][10] = 0;
                                    //dtClonedTemp[fi].Rows[i][12] = 0;
                                }
                                else
                                {
                                    dtClonedTemp[fi].Rows[i][10] = (cValues[fi][i]) / bValues[i];
                                    dtClonedTemp[fi].Rows[i][12] = closeMinusB[fi][i] / bValues[i];
                                }
                                if (aValues[fi][i] == 0)
                                {
                                    //dtClonedTemp[fi].Rows[i][11] = 0;
                                    //dtClonedTemp[fi].Rows[i][13] = 0;
                                }
                                else
                                {
                                    dtClonedTemp[fi].Rows[i][11] = closeMinusA[i] / aValues[fi][i];
                                    dtClonedTemp[fi].Rows[i][13] = (aValues[fi][i] - (double)dtClonedTemp[fi].Rows[i][3]) / aValues[fi][i];
                                }


                            }
                            /*
                            excel = new Excel.Application();
                            wb = excel.Workbooks.Open(FileName);
                            st = (Excel.Worksheet)wb.Worksheets.get_Item(1);
                            range = st.UsedRange;

                            String str = (String)(range.Cells[1, 1] as Excel.Range).Value2;//Get the cell value in cell[1,1]
                            //Get the stock number and name, they are the first and second values
                            StockInfo = str.Split(' ');

                            Double date = (Double)(range.Cells[3, 1] as Excel.Range).Value2;
                            //DateTime dtime = DateTime.ParseExact(date.ToString(), "yyyyMMdd", null); //This does not work
                            dtime = DateTime.FromOADate(date);
                            */

                            /*
                            //Calculate SLOPE of 21 days
                            //Mimic Excel SLOPE function
                            //Slope(b) = (NΣXY - (ΣX)(ΣY)) / (NΣX2 - (ΣX)2)
                            int TotalRows = range.Rows.Count - 1;
                            int i, j, k;
                            double sumXY = 0, sumX = 0, sumY = 0, sumX2 = 0;
                            double[] slopes = new double[TotalRows];//Store slopes of every 21 days
                            double[] aValues = new double[TotalRows];//Store A values;
                            double[] bValues = new double[TotalRows];//Store B values;
                            double[] yValues = new double[21];
                            for (i = 3; i < TotalRows; i++)//the first two rows of range should be escaped
                            {
                                k = i;
                                sumXY = 0; sumX = 0; sumY = 0; sumX2 = 0;

                                for (j = 0; j < 21; j++)
                                {
                                    if (k > TotalRows)
                                        continue;//Already touch the end row

                                    yValues[j] = (Double)(range.Cells[k, 5] as Excel.Range).Value2;
                                    k++;

                                    sumXY += xValues[j] * yValues[j];
                                    sumX += xValues[j];
                                    sumY += yValues[j];
                                    sumX2 += xValues[j] * xValues[j];
                                }

                                //k = i - 2 + 20 - 1;
                                k = k - 2;
                                slopes[k] = (sumXY * 21 - sumX * sumY) / (21 * sumX2 - sumX * sumX);
                                slopes[k] = slopes[k] * 20 + (Double)(range.Cells[k + 1, 5] as Excel.Range).Value2;
                                if (k + 1 == TotalRows)
                                    break;
                            }

                            //Calculate the 55 days EMA
                            double sumTemp = 0;
                            for (i = 23; i < TotalRows; i++)//the first two rows of range should be escaped
                            {
                                k = i - 1;
                                if (k == 22)//Only calculate for once
                                {
                                    sumTemp = 0;
                                    for (j = 0; j < 55; j++)
                                    {
                                        sumTemp += slopes[k++];//slopes index start from 22
                                    }
                                }

                                aValues[76] = sumTemp / 55;
                                //aValues[76] = Math.Round(aValues[76], 2);
                                k = i + 54; //aValues index starts from 76
                                aValues[k] = (slopes[k] + aValues[k - 1] * 27) / 28;
                                aValues[k - 1] = Math.Round(aValues[k - 1], 2);//Round the value after it has been used
                                if (k + 1 == TotalRows)
                                {
                                    aValues[k] = Math.Round(aValues[k], 2);//Round the value of the end row
                                    break;
                                }
                            }
                            */
                            //Display the result
                            //Initializing Columns
                            //dataGridView_all.ColumnCount = range.Columns.Count;
                            //Convert range to Datatable
                            //DataTable dt = new DataTable();


                        }
                        finally
                        {
                            oleExcelConnection.Close();
                            /*
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(st);
                            wb.Close();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                            excel.Quit();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                            */
                        }
                    }

                    dtTablesAllFiles = new DataTable("All");
                    dtTablesAllFiles.Columns.Add("日期", typeof(DateTime));
                    //dtTablesAllFiles.Columns[0].DataType = typeof(DateTime);
                    dtTablesAllFiles.Columns.Add("股票代码", typeof(string));
                    dtTablesAllFiles.Columns.Add("股票名称", typeof(string));
                    dtTablesAllFiles.Columns.Add("收盘价", typeof(string));
                    dtTablesAllFiles.Columns.Add("A", typeof(double));
                    dtTablesAllFiles.Columns.Add("B", typeof(double));

                    newFileNumber = fileNumber - escape.Sum();

                    dtCloned = new DataTable[newFileNumber];
                    //dtClonedCopy = new DataTable[newFileNumber];
                    int idx = 0;
                    for (int i = 0; i < newFileNumber; i++)
                    {
                        while (escape[idx] > 0)
                        {
                            idx++;// escaped files
                        }

                        dtCloned[i] = dtClonedTemp[idx].Copy();
                        //dtClonedCopy[i] = dtClonedTemp[idx].Copy();
                        idx++;
                    }
                    AMinusBTableIndex = new int[newFileNumber];
                    CloseMinusBTableIndex = new int[newFileNumber];

                    for (int i = 0; i < newFileNumber; i++)
                    {
                        DataRow row = dtTablesAllFiles.NewRow();
                        row[0] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][0];//date
                        row[1] = StockInfo[i][0];//stock number
                        row[2] = StockInfo[i][1];//stock name
                        row[3] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][4];//A
                        row[4] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][7];//A
                        row[5] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][8];//B
                        dtTablesAllFiles.Rows.Add(row);
                    }

                    dataGridView_all.Visible = true;
                    dataGridView_all.DataSource = dtTablesAllFiles;
                    ShowRowNumber(dataGridView_all);
                    //dataGridView_all.Columns[0].HeaderText = StockInfo[0];
                }
            }
        }


        private void MeanLineOpenExcelToolStripMenuItem_Click1(object sender, EventArgs e)
        {
            //dataGridView_all.Visible = false;


            //open Excel files
            this.openFileDialog.Multiselect = false; // Disable select multiple files
            this.openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            //Using MS Office Interop is slow and calling .Value2 is an expensive operation
            //So we use OLEDB
            string sConnection = null;
            DataTable dtTablesList = default(DataTable);
            DataTable dtTablesValue = default(DataTable);
            //OleDbCommand oleExcelCommand = default(OleDbCommand);
            //OleDbDataReader oleExcelReader = default(OleDbDataReader);
            //OleDbConnection oleExcelConnection = default(OleDbConnection);
            string sSheetName = null;
            System.Data.OleDb.OleDbDataAdapter adp;

            //fileNumber = 0;

            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string FileName = this.openFileDialog.FileName;
                    dataGridView_all.Visible = false;
                    AMinusBClicked = false;
                    CloseMinusBClicked = false;
                    if (oleExcelConnection != null && oleExcelConnection.State == ConnectionState.Open)
                    {
                        oleExcelConnection.Close();
                    }

                    try
                    {
                        Process[] ps = Process.GetProcesses();
                        foreach (Process p in ps)
                        {
                            if (p.MainWindowTitle.Contains("Excel"))
                            {
                                fileOpened = false;
                                MessageBox.Show("请先关闭已经打开的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        openFileDialog.OpenFile();
                        fileOpened = true;
                    }
                    catch
                    {
                        fileOpened = false;
                        MessageBox.Show("请先关闭已经打开的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    fileOpened = false;

                    sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Mode=Read;Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";
                    oleExcelConnection = new OleDbConnection(sConnection);
                    bool isconopen = false;
                    fileOpened = false;
                    try
                    {
                        oleExcelConnection.Open();
                        isconopen = true;
                        fileOpened = true;
                    }
                    catch
                    {
                        oleExcelConnection.Close();
                        isconopen = false;
                        fileOpened = false;
                        MessageBox.Show("请打开标准格式的Excel文件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!isconopen)
                    {
                        return;
                    }

                    dtTablesList = oleExcelConnection.GetSchema("Tables");

                    if (dtTablesList.Rows.Count > 0)
                    {
                        sSheetName = dtTablesList.Rows[0]["TABLE_NAME"].ToString();
                    }

                    adp = new OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", sSheetName), oleExcelConnection);
                    dtTablesList.Clear();
                    dtTablesList.Dispose();

                    adp.Fill(dtTablesList);
                    dtTablesValue = dtTablesList.Copy();
                    int i = 0;
                    for (i = 0; i < 9; i++)//delete the first 9 columns in the datatable
                    {
                        dtTablesValue.Columns.RemoveAt(0);//delete 9 times
                    }
                    //change the columns' name
                    for (i = 0; i < 7; i++)
                    {
                        dtTablesValue.Columns[i].ColumnName = Regex.Replace(dtTablesValue.Rows[1][i].ToString(), @"\s+", "");
                    }
                    //Get the stock number and name, they are the first and second values
                    string str = dtTablesValue.Rows[0][0].ToString();
                    string[] StockInfo = str.Split(' ');
                    dtTablesValue.Rows.RemoveAt(0);
                    dtTablesValue.Rows.RemoveAt(0);//delete the first two rows;
                    dtTablesValue.Rows.RemoveAt(dtTablesValue.Rows.Count - 1);//delete the last row.

                    DataTable dtTable = dtTablesValue.Clone();

                    dtTable.Columns[0].DataType = typeof(DateTime);
                    dtTable.Columns[1].DataType = typeof(double);
                    dtTable.Columns[2].DataType = typeof(double);
                    dtTable.Columns[3].DataType = typeof(double);
                    dtTable.Columns[4].DataType = typeof(double);
                    dtTable.Columns[5].DataType = typeof(double);
                    dtTable.Columns[6].DataType = typeof(double);
                    foreach (DataRow row in dtTablesValue.Rows)
                    {
                        dtTable.ImportRow(row);
                    }

                    dtTable.Columns.Add("MMA5", typeof(double));
                    dtTable.Columns.Add("MMA10", typeof(double));
                    dtTable.Columns.Add("MMA20", typeof(double));
                    dtTable.Columns.Add("MMA30", typeof(double));
                    dtTable.Columns.Add("MMA60", typeof(double));
                    dtTable.Columns.Add("MMA120", typeof(double));
                    dtTable.Columns.Add("MMA250", typeof(double));
                    dtTable.Columns.Add(" ");//分割线
                    dtTable.Columns.Add("MA5", typeof(double));
                    dtTable.Columns.Add("MA10", typeof(double));
                    dtTable.Columns.Add("MA20", typeof(double));
                    dtTable.Columns.Add("MA30", typeof(double));
                    dtTable.Columns.Add("MA60", typeof(double));
                    dtTable.Columns.Add("MA120", typeof(double));
                    dtTable.Columns.Add("MA250", typeof(double));


                    int TotalRows = dtTable.Rows.Count;
                    for (i = 0; i < TotalRows; i++)
                    {
                        if (i >= 5)
                        {
                            dtTable.Rows[i][7] = Math.Round((double)dtTable.Rows[i][4] - mma(5, i, dtTable), 2);//ma5
                        }
                        if (i >= 10)
                        {
                            dtTable.Rows[i][8] = Math.Round((double)dtTable.Rows[i][4] - mma(10, i, dtTable), 2);//ma10
                        }
                        if (i >= 20)
                        {
                            dtTable.Rows[i][9] = Math.Round((double)dtTable.Rows[i][4] - mma(20, i, dtTable), 2);//ma20
                        }
                        if (i >= 30)
                        {
                            dtTable.Rows[i][10] = Math.Round((double)dtTable.Rows[i][4] - mma(30, i, dtTable), 2);//ma30
                        }
                        if (i >= 60)
                        {
                            dtTable.Rows[i][11] = Math.Round((double)dtTable.Rows[i][4] - mma(60, i, dtTable), 2);//ma60
                        }
                        if (i >= 120)
                        {
                            dtTable.Rows[i][12] = Math.Round((double)dtTable.Rows[i][4] - mma(120, i, dtTable), 2);//ma120
                        }
                        if (i >= 250)
                        {
                            dtTable.Rows[i][13] = Math.Round((double)dtTable.Rows[i][4] - mma(250, i, dtTable), 2);//ma250
                        }
                        if (i >= 4)
                        {
                            dtTable.Rows[i][15] = Math.Round(ma(5, i, dtTable), 2);//mma5
                        }
                        if (i >= 9)
                        {
                            dtTable.Rows[i][16] = Math.Round(ma(10, i, dtTable), 2);//mma10
                        }
                        if (i >= 19)
                        {
                            dtTable.Rows[i][17] = Math.Round(ma(20, i, dtTable), 2);//mma20
                        }
                        if (i >= 29)
                        {
                            dtTable.Rows[i][18] = Math.Round(ma(30, i, dtTable), 2);//mma30
                        }
                        if (i >= 59)
                        {
                            dtTable.Rows[i][19] = Math.Round(ma(60, i, dtTable), 2);//mma60
                        }
                        if (i >= 119)
                        {
                            dtTable.Rows[i][20] = Math.Round(ma(120, i, dtTable), 2);//mma120
                        }
                        if (i >= 249)
                        {
                            dtTable.Rows[i][21] = Math.Round(ma(250, i, dtTable), 2);//mma250
                        }
                    }

                    StockDetail sd = new StockDetail();
                    sd.dataGridView_StockDetail.DataSource = dtTable;
                    sd.dataGridView_StockDetail.Columns[14].Width = 6;
                    sd.dataGridView_StockDetail.Columns[14].DefaultCellStyle.BackColor = Color.LightBlue;
                    //this.dataGridView_all.DataSource = dtTable;
                    sd.Text = StockInfo[0] + "  " + StockInfo[1];
                    //sd.Show();
                    sd.ShowDialog();
                    //dataGridView_all.Visible = true;
                    ShowRowNumber(sd.dataGridView_StockDetail);
                }
                finally
                {
                    oleExcelConnection.Close();
                }
            }
        }

        private void MeanLineOpenTXTFileToolStripMenuItem_Click1(object sender, EventArgs e)
        {
            isStockDetail = false;
            isMeanLine = true;
            dataGridView_all.DataSource = dtTableMean;
        }

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open Txt files
            this.openFileDialog.Multiselect = true; //Enable select multiple files
            this.openFileDialog.Filter = "Txt Files|*.txt";

            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string FileName = this.openFileDialog.FileName;
                fileNumber = openFileDialog.FileNames.Length;

                pg = new ProgressBar();
                pg.progressBar_MeanLine.Value = 0;
                pg.progressBar_MeanLine.Maximum = fileNumber;
                pg.progressBar_MeanLine.Minimum = 0;
                currentFileNumber = 0;
                pg.Text = "正在打开文件......";
                pg.Show();

                dataGridView_all.Visible = false;
                AMinusBClicked = false;
                CloseMinusBClicked = false;
                isStockDetail = true;
                isMeanLine = false;
                isMeanLineCalculated = false;

                fileOpened = false;

                DataTable[] dtClonedTemp = new DataTable[fileNumber];
                //StockInfo = new string[fileNumber][];//Stock number and name of each file
                string[][] tempStockInfo = new string[fileNumber][];
                //dtCloned = new DataTable[fileNumber];
                TotalRows = new int[fileNumber];
                cValues = new double[fileNumber][];//C values
                closeMinusB = new double[fileNumber][];
                K = new bool[fileNumber][];
                aValues = new double[fileNumber][];
                escape = new int[fileNumber];
                Array.Clear(escape, 0, fileNumber);// Initial escape with 0

                for (int fi = 0; fi < fileNumber; fi++)
                {
                    try
                    {
                        FileName = openFileDialog.FileNames[fi].ToString();
                        currentFileNumber = fi;
                        timer_progressBar_Tick1();

                        var filestream = new System.IO.FileStream(FileName,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read,
                                          System.IO.FileShare.ReadWrite);
                        var file = new System.IO.StreamReader(filestream, System.Text.Encoding.Default, true, 128);
                        string line;
                        int LineNumber = 0;
                        dtClonedTemp[fi] = new DataTable("temp");
                        while ((line = file.ReadLine()) != null)
                        {
                            //string[] data = line.Split(null);

                            if (LineNumber == 0)// the first line
                            {
                                tempStockInfo[fi] = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                                LineNumber++;
                                continue;
                            }

                            string[] data = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                            if (LineNumber == 1)// the second line
                            {
                                for (int c = 0; c < data.Length; c++)
                                {
                                    dtClonedTemp[fi].Columns.Add(data[c], typeof(double));
                                }
                                dtClonedTemp[fi].Columns[0].DataType = typeof(DateTime);
                                LineNumber++;
                                continue;
                            }

                            if (data.Length < 7)// reach the file end
                                break;

                            DataRow row = dtClonedTemp[fi].NewRow();
                            for (int c = 0; c < data.Length; c++)
                            {
                                row[c] = data[c];
                            }
                            dtClonedTemp[fi].Rows.Add(row);
                            LineNumber++;
                        }

                        LineNumber -= 2;
                        if (LineNumber < 75)
                        {
                            escape[fi] = 1;
                            continue;
                        }

                        TotalRows[fi] = LineNumber;
                        aValues[fi] = new double[TotalRows[fi]];//Store A values;
                        cValues[fi] = new double[TotalRows[fi]];//Store C values;
                        closeMinusB[fi] = new double[TotalRows[fi]];
                        K[fi] = new bool[TotalRows[fi]];

                        int i, j, k;
                        double sumXY = 0, sumX = 0, sumY = 0, sumX2 = 0;
                        double[] slopes = new double[TotalRows[fi]];//Store slopes of every 21 days
                        double[] bValues = new double[TotalRows[fi]];//Store B values;
                        double[] yValues = new double[21];
                        double[] closeMinusA = new double[TotalRows[fi]];
                        for (i = 0; i < TotalRows[fi]; i++)//the first two rows of range should be escaped
                        {
                            k = i;
                            sumXY = 0; sumX = 0; sumY = 0; sumX2 = 0;

                            for (j = 0; j < 21; j++)
                            {
                                if (k >= TotalRows[fi])
                                    break;//Already touch the end row

                                yValues[j] = (double)dtClonedTemp[fi].Rows[k][4];
                                k++;

                                sumXY += xValues[j] * yValues[j];
                                sumX += xValues[j];
                                sumY += yValues[j];
                                sumX2 += xValues[j] * xValues[j];
                            }

                            slopes[k - 1] = (sumXY * 21 - sumX * sumY) / (21 * sumX2 - sumX * sumX);
                            slopes[k - 1] = slopes[k - 1] * 20 + (double)dtClonedTemp[fi].Rows[k - 1][4];
                            if (k == TotalRows[fi])
                                break;
                        }

                        //Calculate the 55 days EMA
                        double sumTemp = 0;
                        for (i = 0; i < TotalRows[fi]; i++)//the first two rows of range should be escaped
                        {
                            k = i + 20;
                            if (k == 20)//Only calculate for once
                            {
                                sumTemp = 0;
                                for (j = 0; j < 55; j++)
                                {
                                    sumTemp += slopes[k++];//slopes index start from 22
                                }

                                aValues[fi][74] = sumTemp / 55;
                            }

                            k = i + 75; //aValues index starts from 74
                            if (k == TotalRows[fi])
                            {
                                break;
                            }
                            aValues[fi][k] = (slopes[k] + aValues[fi][k - 1] * 27) / 28;
                            aValues[fi][k - 1] = Math.Round(aValues[fi][k - 1], 2);//Round the value after it has been used
                            if (k + 1 == TotalRows[fi])
                            {
                                aValues[fi][k] = Math.Round(aValues[fi][k], 2);//Round the value of the end row
                                break;
                            }
                        }

                        //Calculate B values
                        double[] bTemp = new double[60];
                        for (i = 0; i < TotalRows[fi]; i++)
                        {
                            k = i;
                            for (j = 0; j < 60; j++)
                            {
                                bTemp[j] = aValues[fi][k++];
                            }
                            bValues[k - 1] = bTemp.Max();
                            if (k == TotalRows[fi])
                            {
                                break;
                            }
                        }

                        dtClonedTemp[fi].Columns.Add("A", typeof(double));
                        dtClonedTemp[fi].Columns.Add("B", typeof(double));
                        dtClonedTemp[fi].Columns.Add("黄K", typeof(bool));
                        dtClonedTemp[fi].Columns.Add("C", typeof(double));
                        dtClonedTemp[fi].Columns.Add("收盘-A", typeof(double));
                        dtClonedTemp[fi].Columns.Add("收盘-B", typeof(double));
                        dtClonedTemp[fi].Columns.Add("A-最低", typeof(double));
                        for (i = 0; i < TotalRows[fi]; i++)
                        {
                            cValues[fi][i] = Math.Round(bValues[i] - aValues[fi][i], 2);//Calculate C values
                            dtClonedTemp[fi].Rows[i][7] = aValues[fi][i];
                            dtClonedTemp[fi].Rows[i][8] = bValues[i];
                            closeMinusB[fi][i] = Math.Round((double)dtClonedTemp[fi].Rows[i][4] - bValues[i], 2);
                            closeMinusA[i] = Math.Round((double)dtClonedTemp[fi].Rows[i][4] - aValues[fi][i], 2);
                            if (i == 0)
                            {
                                K[fi][i] = false;
                            }
                            else
                            {
                                double open = (double)dtClonedTemp[fi].Rows[i][1];
                                double closeOneDayAgo = (double)dtClonedTemp[fi].Rows[i - 1][4];
                                double close = (double)dtClonedTemp[fi].Rows[i][4];
                                if (close > 1.098 * closeOneDayAgo && close > open)// calculate K
                                {
                                    K[fi][i] = true;
                                }
                            }


                            dtClonedTemp[fi].Rows[i][9] = K[fi][i];
                            if (bValues[i] == 0)
                            {
                                //dtClonedTemp[fi].Rows[i][10] = 0;
                                //dtClonedTemp[fi].Rows[i][12] = 0;
                            }
                            else
                            {
                                dtClonedTemp[fi].Rows[i][10] = (cValues[fi][i]) / bValues[i];
                                dtClonedTemp[fi].Rows[i][12] = closeMinusB[fi][i] / bValues[i];
                            }
                            if (aValues[fi][i] == 0)
                            {
                                //dtClonedTemp[fi].Rows[i][11] = 0;
                                //dtClonedTemp[fi].Rows[i][13] = 0;
                            }
                            else
                            {
                                dtClonedTemp[fi].Rows[i][11] = closeMinusA[i] / aValues[fi][i];
                                dtClonedTemp[fi].Rows[i][13] = (aValues[fi][i] - (double)dtClonedTemp[fi].Rows[i][3]) / aValues[fi][i];
                            }
                        }

                    }
                    catch
                    {
                        MessageBox.Show("错误！");
                        return;
                    }
                }

                dtTablesAllFiles = new DataTable("All");
                dtTablesAllFiles.Columns.Add("日期", typeof(DateTime));
                //dtTablesAllFiles.Columns[0].DataType = typeof(DateTime);
                dtTablesAllFiles.Columns.Add("股票代码", typeof(string));
                dtTablesAllFiles.Columns.Add("股票名称", typeof(string));
                dtTablesAllFiles.Columns.Add("收盘价", typeof(string));
                dtTablesAllFiles.Columns.Add("A", typeof(double));
                dtTablesAllFiles.Columns.Add("B", typeof(double));

                newFileNumber = fileNumber - escape.Sum();
                AMinusBTableIndex = new int[newFileNumber];
                CloseMinusBTableIndex = new int[newFileNumber];

                dtCloned = new DataTable[newFileNumber];
                StockInfo = new string[newFileNumber][];
                int idx = 0;
                for (int i = 0; i < newFileNumber; i++)
                {
                    while (escape[idx] > 0)
                    {
                        idx++;// escaped files
                    }

                    dtCloned[i] = dtClonedTemp[idx].Copy();
                    StockInfo[i] = tempStockInfo[idx];
                    idx++;
                }

                //for (int i = 0; i < newFileNumber; i++)
                for (int i = 0; i < fileNumber; i++)
                {
                    DataRow row = dtTablesAllFiles.NewRow();
                    //row[0] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][0];//date
                    row[0] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][0];//date
                    row[1] = StockInfo[i][0];//stock number
                    row[2] = StockInfo[i][1];//stock name
                    //row[3] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][4];//A
                    //row[4] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][7];//A
                    //row[5] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][8];//B

                    row[3] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][4];//A
                    row[4] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][7];//A
                    row[5] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][8];//B

                    dtTablesAllFiles.Rows.Add(row);
                }
                dtTableMean = dtTablesAllFiles.Copy();
                dataGridView_all.Visible = true;
                dataGridView_all.DataSource = dtTablesAllFiles;
                fileOpened = true;
                isStockDetail = true;
                isMeanLine = false;
                ShowRowNumber(dataGridView_all);
            }
        }

        private void timer_progressBar_Tick1()
        {
            if (isMeanLine || isWeekK)
            {
                if (currentFileNumber < fileNumber - 1)
                    //if (currentFileNumber < newFileNumber - 1)
                {
                    pg.progressBar_MeanLine.Value++;
                    //pg.label_meanLine.Text = "正在计算平均线。一共 " + newFileNumber.ToString() + " 个文件，当前处理第 " + (currentFileNumber+1).ToString() + " 个。";
                }
                else
                {
                    //timer_progressBar.Enabled = false;
                    pg.Close();
                }
            }
            else
            {
                if (currentFileNumber < fileNumber - 1)
                {
                    pg.progressBar_MeanLine.Value++;
                }
                else
                {
                    //timer_progressBar.Enabled = false;
                    pg.Close();
                }
            }
        }

        private void WeekKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void CalcWeekKToolStripMenuItem_Click1(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！");
                return;
            }

            if (!isWeekKCalculated)
            {
                pg = new ProgressBar();
                pg.progressBar_MeanLine.Value = 0;
                pg.progressBar_MeanLine.Maximum = newFileNumber;
                pg.progressBar_MeanLine.Minimum = 0;
                //timer_progressBar.Enabled = true;
                currentFileNumber = 0;
                pg.Text = "正在计算周K值......";
                pg.Show();


                isStockDetail = false;
                isWeekK = true;
                isMeanLine = false;
                isWeekKCalculated = true;
                dataGridView_all.DataSource = dtTableMean;
                dtTableWeek = new DataTable[newFileNumber];
                dtTableMonth = new DataTable[newFileNumber];

                for (int fi = 0; fi < newFileNumber; fi++)
                {
                    currentFileNumber = fi;
                    timer_progressBar_Tick1();
                    //dtTableWeek[fi] = dtCloned[fi].Copy();
                    //for (int j = 0; j < 7; j++)
                    //{
                    //    dtTableWeek[fi].Columns.RemoveAt(7);
                    //}

                    dtTableMonth[fi] = new DataTable("dttablemonth");
                    dtTableMonth[fi].Columns.Add("col1");
                    dtTableMonth[fi].Columns.Add("col2");
                    dtTableMonth[fi].Columns.Add("col3");
                    dtTableMonth[fi].Columns.Add("col4");
                    dtTableMonth[fi].Columns.Add("col5");
                    dtTableMonth[fi].Columns[4].DataType = typeof(double);

                    int month = 0;
                    int year = 0;
                    int dayCount = 0;
                    double moneyCount = 0;
                    int TotalRows = dtCloned[fi].Rows.Count;
                    for (int i = 0; i < TotalRows; i++)
                    {
                        int y = ((DateTime)dtCloned[fi].Rows[i][0]).Year;
                        int m = ((DateTime)dtCloned[fi].Rows[i][0]).Month;
                        if (i == 0)
                        {
                            year = y;
                            month = m;
                        }
                        if (year == y)// the same year
                        {
                            if (month == m)// the same month
                            {
                                moneyCount += (double)dtCloned[fi].Rows[i][6];
                                dayCount++;
                            }
                            else
                            {
                                DataRow row = dtTableMonth[fi].NewRow();
                                row[0] = year;
                                row[1] = month;
                                row[2] = dayCount;
                                row[3] = Math.Round(moneyCount / 100000000, 2);
                                row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                                dtTableMonth[fi].Rows.Add(row);
                                month = m;
                                dayCount = 1;
                                moneyCount = (double)dtCloned[fi].Rows[i][6];
                            }
                        }
                        else
                        {
                            DataRow row = dtTableMonth[fi].NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = dayCount;
                            row[3] = Math.Round(moneyCount / 100000000, 2);
                            row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                            dtTableMonth[fi].Rows.Add(row);

                            dayCount = 1;
                            moneyCount = (double)dtCloned[fi].Rows[i][6];
                            year = y;
                            month = m;
                        }

                        if (i == TotalRows - 1)
                        {
                            DataRow row = dtTableMonth[fi].NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = dayCount;
                            row[3] = Math.Round(moneyCount / 100000000, 2);
                            row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                            dtTableMonth[fi].Rows.Add(row);
                        }
                    }

                    // calculate week K
                    dtTableWeek[fi] = new DataTable("dttableweek");
                    dtTableWeek[fi].Columns.Add("col0");
                    dtTableWeek[fi].Columns.Add("col01");
                    dtTableWeek[fi].Columns.Add("col1");
                    dtTableWeek[fi].Columns.Add("col2");
                    dtTableWeek[fi].Columns.Add("col3");
                    dtTableWeek[fi].Columns.Add("col4");
                    dtTableWeek[fi].Columns.Add("col5");
                    dtTableWeek[fi].Columns.Add("col6");
                    dtTableWeek[fi].Columns.Add("col7");
                    dtTableWeek[fi].Columns.Add("col8");
                    dtTableWeek[fi].Columns[9].DataType = typeof(double);

                    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                    Calendar cal = dfi.Calendar;
                    int week = 0, day = 0;
                    int RowIdxCurrent = 0;
                    bool calStand = false;
                    double standard = 0; //base value
                    double moneyTotal = 0;
                    double difTotal = 0;
                    for (int i = 0; i < TotalRows; i++)
                    {
                        DateTime dt = (DateTime)dtCloned[fi].Rows[i][0];
                        int y = dt.Year;
                        int m = dt.Month;
                        int w = cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        day = (int)dt.DayOfWeek;

                        if (year == y)//the same year
                        {
                            if (month == m)// the same month
                            {
                                if (!calStand)// get the base value of last month
                                {
                                    for (int ii = 0; ii < dtTableMonth[fi].Rows.Count; ii++)
                                    {
                                        if (year.ToString() == dtTableMonth[fi].Rows[ii][0].ToString() && month.ToString() == dtTableMonth[fi].Rows[ii][1].ToString())
                                        {
                                            if (i == 0)
                                            {
                                                standard = 0;
                                                calStand = true;
                                                break;
                                            }
                                            else
                                            {
                                                standard = Convert.ToDouble(dtTableMonth[fi].Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                                calStand = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (week == w)
                                {
                                    double money = (double)dtCloned[fi].Rows[i][6];
                                    string temp = money.ToString();
                                    int len = temp.Length;
                                    switch (day)
                                    {
                                        case 1:
                                            if (len < 7)
                                            {
                                                dtTableWeek[fi].Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 9-len);
                                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 9-len) - standard, 9-len);
                                                moneyTotal += money;
                                                difTotal += Math.Round(Math.Round(money / 100000000, 9-len) - standard, 9-len);
                                            }
                                            else
                                            {
                                                dtTableWeek[fi].Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 2);
                                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                                moneyTotal += money;
                                                difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            }
                                            break;
                                        case 2:
                                            temp = money.ToString();
                                            len = temp.Length;
                                            if (len < 7)
                                            {
                                                dtTableWeek[fi].Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 9 - len);
                                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                                                moneyTotal += money;
                                                difTotal += Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                                            }
                                            else
                                            {
                                                dtTableWeek[fi].Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                                moneyTotal += money;
                                                difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            }
                                            break;
                                        case 3:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 4:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 5:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2); ;
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                                    moneyTotal = 0;
                                    difTotal = 0;
                                    RowIdxCurrent += 2;

                                    week = w;

                                    DataRow dwRow = dtTableWeek[fi].NewRow();
                                    dwRow[0] = year;
                                    dwRow[1] = month;
                                    dwRow[2] = "第" + week.ToString() + "周";
                                    dwRow[3] = "成交量";
                                    dtTableWeek[fi].Rows.Add(dwRow);

                                    DataRow row = dtTableWeek[fi].NewRow();
                                    row[0] = year;
                                    row[1] = month;
                                    row[2] = "第" + week.ToString() + "周";
                                    row[3] = "成-基";
                                    dtTableWeek[fi].Rows.Add(row);

                                    double money = (double)dtCloned[fi].Rows[i][6];
                                    switch (day)
                                    {
                                        case 1:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 2:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 3:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 4:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        case 5:
                                            dtTableWeek[fi].Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            moneyTotal += money;
                                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else//month != m
                            {
                                if (dtTableWeek[fi].Rows.Count > 0)
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                                    RowIdxCurrent += 2;
                                }

                                month = m;
                                week = w;
                                calStand = false;
                                moneyTotal = 0;
                                difTotal = 0;

                                DataRow dwRow = dtTableWeek[fi].NewRow();
                                dwRow[0] = year;
                                dwRow[1] = month;
                                dwRow[2] = "第" + week.ToString() + "周";
                                dwRow[3] = "成交量";
                                dtTableWeek[fi].Rows.Add(dwRow);

                                DataRow row = dtTableWeek[fi].NewRow();
                                row[0] = year;
                                row[1] = month;
                                row[2] = "第" + week.ToString() + "周";
                                row[3] = "成-基";
                                dtTableWeek[fi].Rows.Add(row);


                                if (!calStand)// get the base value of last month
                                {
                                    for (int ii = 0; ii < dtTableMonth[fi].Rows.Count; ii++)
                                    {
                                        if (year == Convert.ToDouble(dtTableMonth[fi].Rows[ii][0].ToString()) && month == Convert.ToDouble(dtTableMonth[fi].Rows[ii][1].ToString()))
                                        {
                                            if (i == 0)
                                            {
                                                standard = 0;
                                                calStand = true;
                                                break;
                                            }
                                            else
                                            {
                                                standard = Convert.ToDouble(dtTableMonth[fi].Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                                calStand = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                double money = (double)dtCloned[fi].Rows[i][6];
                                switch (day)
                                {
                                    case 1:
                                        dtTableWeek[fi].Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 2);
                                        dtTableWeek[fi].Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 2:
                                        dtTableWeek[fi].Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                        dtTableWeek[fi].Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 3:
                                        dtTableWeek[fi].Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                        dtTableWeek[fi].Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 4:
                                        dtTableWeek[fi].Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                        dtTableWeek[fi].Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    case 5:
                                        dtTableWeek[fi].Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                        dtTableWeek[fi].Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        moneyTotal += money;
                                        difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else//year != y
                        {
                            if (i != 0)
                            {
                                dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2);
                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                                RowIdxCurrent += 2;
                            }

                            year = y;
                            month = m;
                            week = w;
                            moneyTotal = 0;
                            difTotal = 0;
                            DataRow dwRow = dtTableWeek[fi].NewRow();
                            dwRow[0] = year;
                            dwRow[1] = month;
                            dwRow[2] = "第" + week.ToString() + "周";
                            dwRow[3] = "成交量";
                            dtTableWeek[fi].Rows.Add(dwRow);

                            DataRow row = dtTableWeek[fi].NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = "第" + week.ToString() + "周";
                            row[3] = "成-基";
                            dtTableWeek[fi].Rows.Add(row);
                            calStand = false;

                            if (!calStand)// get the base value of last month
                            {
                                for (int ii = 0; ii < dtTableMonth[fi].Rows.Count; ii++)
                                {
                                    if (year.ToString() == dtTableMonth[fi].Rows[ii][0].ToString() && month.ToString() == dtTableMonth[fi].Rows[ii][1].ToString())
                                    {
                                        if (i == 0)
                                        {
                                            standard = 0;
                                            calStand = true;
                                            break;
                                        }
                                        else
                                        {
                                            standard = Convert.ToDouble(dtTableMonth[fi].Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                            calStand = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            double money = (double)dtCloned[fi].Rows[i][6];
                            switch (day)
                            {
                                case 1:
                                    dtTableWeek[fi].Rows[RowIdxCurrent][4] = Math.Round(money / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][4] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 2:
                                    dtTableWeek[fi].Rows[RowIdxCurrent][5] = Math.Round(money / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][5] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 3:
                                    dtTableWeek[fi].Rows[RowIdxCurrent][6] = Math.Round(money / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][6] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 4:
                                    dtTableWeek[fi].Rows[RowIdxCurrent][7] = Math.Round(money / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][7] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                case 5:
                                    dtTableWeek[fi].Rows[RowIdxCurrent][8] = Math.Round(money / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][8] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    break;
                                default:
                                    break;
                            }

                        }

                        if (i == TotalRows - 1)
                        {
                            dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2);
                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                        }
                    }
                }
            }
            else
            {
                isStockDetail = false;
                isMeanLine = false;
                isWeekK = true;
            }

            dataGridView_all.DataSource = dtTableMean;
            ShowRowNumber(dataGridView_all);
        }

        private void CalcWeekKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fileOpened)
            {
                MessageBox.Show("请先打开文件！");
                return;
            }

            if (!isWeekKCalculated)
            {
                //pg = new ProgressBar();
                //pg.progressBar_MeanLine.Value = 0;
                //pg.progressBar_MeanLine.Maximum = newFileNumber;
                //pg.progressBar_MeanLine.Minimum = 0;
                ////timer_progressBar.Enabled = true;
                //currentFileNumber = 0;
                //pg.Text = "正在计算周K值......";
                //pg.Show();


                isStockDetail = false;
                isWeekK = true;
                isMeanLine = false;
                isWeekKCalculated = true;
                //dtTableWeek = new DataTable[newFileNumber];
                //dtTableMonth = new DataTable[newFileNumber];
                dtTableWeek = new DataTable[fileNumber];
                dtTableMonth = new DataTable[fileNumber];

                isProgressBarCanceled = false;

                workerPB = new ProgressBar();
                workerPB.progressBar_MeanLine.Value = 0;
                //workerPB.progressBar_MeanLine.Maximum = newFileNumber;
                workerPB.progressBar_MeanLine.Maximum = fileNumber;
                workerPB.progressBar_MeanLine.Minimum = 0;
                currentFileNumber = 0;

                worker = new BackgroundWorker();
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork_WeekK);
                worker.RunWorkerAsync();
                workerPB.Text = "正在计算周K值 ……";
                workerPB.StartPosition = FormStartPosition.CenterParent;
                workerPB.ShowDialog();


                // calculated in function worker_DoWork_WeekK;

            
            }
            else
            {
                isStockDetail = false;
                isMeanLine = false;
                isWeekK = true;
            }

            if (!isProgressBarCanceled)
            {
                dataGridView_all.DataSource = dtTableMean;
                ShowRowNumber(dataGridView_all);
            }
            else
            {
                if (!isMeanLineCalculated)
                {
                    isStockDetail = true;
                    isWeekK = false;
                    isWeekKCalculated = false;
                }
                else
                {
                    isMeanLine = true;
                }
                isProgressBarCanceled = false;
            }
        }


        private void Red3ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // whether column index 4,5,6,7,8 has 3 red cells at the last row
            //dtTableWeekRed = new DataTable[newFileNumber];

            if (!isWeekKCalculated)
            {
                MessageBox.Show("请先计算周K值！");
                return;
            }

            dtTableWeekRed = new DataTable("dttableweekred");
            dtTableWeekRed.Columns.Add("id");
            dtTableWeekRed.Columns.Add("name");
            dtTableWeekRed.Columns.Add("date");
            dtTableWeekRed.Columns.Add("item");
            dtTableWeekRed.Columns.Add("one");
            dtTableWeekRed.Columns.Add("two");
            dtTableWeekRed.Columns.Add("three");
            dtTableWeekRed.Columns.Add("four");
            dtTableWeekRed.Columns.Add("five");
            dtTableWeekRed.Columns.Add("total");

            //int[][] isRed = new int[newFileNumber][];
            int[][] isRed = new int[newFileNumberFive][];
            for (int fi = 0; fi < newFileNumberFive; fi++)
                //for (int fi = 0; fi < newFileNumber; fi++)
            {
                isRed[fi]= new int[5]{0,0,0,0,0};

                int end = dtTableWeek[fi].Rows.Count - 1;
                
                if(dtTableWeek[fi].Rows[end][4].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][4].ToString()) > 0)
                {
                    isRed[fi][0] = 1;
                }
                if (dtTableWeek[fi].Rows[end][5].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][5].ToString()) > 0)
                {
                    isRed[fi][1] = 1;
                }
                if (dtTableWeek[fi].Rows[end][6].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][6].ToString()) > 0)
                {
                    isRed[fi][2] = 1;
                }
                if (dtTableWeek[fi].Rows[end][7].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][7].ToString()) > 0)
                {
                    isRed[fi][3] = 1;
                }
                if (dtTableWeek[fi].Rows[end][8].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][8].ToString()) > 0)
                {
                    isRed[fi][4] = 1;
                }

                int sum = isRed[fi].Sum();

                if (sum == 3)
                {
                    //int end1 = dtCloned[fi].Rows.Count - 1;
                    int end1 = dtClonedFive[fi].Rows.Count - 1;
                    DataRow dr = dtTableWeekRed.NewRow();
                    for (int i = 3; i < dtTableWeekRed.Columns.Count; i++)
                    {
                        dr[i] = dtTableWeek[fi].Rows[end-1][i];
                    }
                    dr[0] = StockInfoFive[fi][0];
                    dr[1] = StockInfoFive[fi][1];
                    //dr[2] = ((DateTime)dtCloned[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dr[2] = ((DateTime)dtClonedFive[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dtTableWeekRed.Rows.Add(dr);

                    dr = dtTableWeekRed.NewRow();
                    for (int i = 3; i < dtTableWeekRed.Columns.Count; i++)
                    {
                        dr[i] = dtTableWeek[fi].Rows[end][i];
                    }
                    //dr[0] = StockInfo[fi][0];
                    dr[1] = StockInfoFive[fi][1];
                    //dr[2] = ((DateTime)dtCloned[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dr[2] = ((DateTime)dtClonedFive[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dtTableWeekRed.Rows.Add(dr);
                }
            }

            if (dtTableWeekRed.Rows.Count > 0)
            {
                ColorRed cr = new ColorRed();
                cr.Text = "一周有 3 个红格";
                cr.colorRedrowMergeView.DataSource = dtTableWeekRed;
                cr.colorRedrowMergeView.ColumnHeadersHeight = 40;
                cr.colorRedrowMergeView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                //cr.colorRedrowMergeView.MergeColumnNames.Add("Column1");
                cr.colorRedrowMergeView.MergeColumnNames.Add("Column2");
                //cr.colorRedrowMergeView.MergeColumnNames.Add("Column3");
                cr.Show();
            }
            else
            {
                MessageBox.Show("不存在 一周有 3 个红格 的情况！");
                return;
            }
        }

        private void Red4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!isWeekKCalculated)
            {
                MessageBox.Show("请先计算周K值！");
                return;
            }

            dtTableWeekRed = new DataTable("dttableweekred");
            dtTableWeekRed.Columns.Add("id");
            dtTableWeekRed.Columns.Add("name");
            dtTableWeekRed.Columns.Add("date");
            dtTableWeekRed.Columns.Add("item");
            dtTableWeekRed.Columns.Add("one");
            dtTableWeekRed.Columns.Add("two");
            dtTableWeekRed.Columns.Add("three");
            dtTableWeekRed.Columns.Add("four");
            dtTableWeekRed.Columns.Add("five");
            dtTableWeekRed.Columns.Add("total");

            //int[][] isRed = new int[newFileNumber][];
            //for (int fi = 0; fi < newFileNumber; fi++)
                int[][] isRed = new int[newFileNumberFive][];
            for (int fi = 0; fi < newFileNumberFive; fi++)
            {
                isRed[fi] = new int[5] { 0, 0, 0, 0, 0 };

                int end = dtTableWeek[fi].Rows.Count - 1;

                if (dtTableWeek[fi].Rows[end][4].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][4].ToString()) > 0)
                {
                    isRed[fi][0] = 1;
                }
                if (dtTableWeek[fi].Rows[end][5].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][5].ToString()) > 0)
                {
                    isRed[fi][1] = 1;
                }
                if (dtTableWeek[fi].Rows[end][6].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][6].ToString()) > 0)
                {
                    isRed[fi][2] = 1;
                }
                if (dtTableWeek[fi].Rows[end][7].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][7].ToString()) > 0)
                {
                    isRed[fi][3] = 1;
                }
                if (dtTableWeek[fi].Rows[end][8].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][8].ToString()) > 0)
                {
                    isRed[fi][4] = 1;
                }

                int sum = isRed[fi].Sum();

                if (sum == 4)
                {
                    //int end1 = dtCloned[fi].Rows.Count - 1;
                    int end1 = dtClonedFive[fi].Rows.Count - 1;
                    DataRow dr = dtTableWeekRed.NewRow();
                    for (int i = 3; i < dtTableWeekRed.Columns.Count; i++)
                    {
                        dr[i] = dtTableWeek[fi].Rows[end - 1][i];
                    }
                    dr[0] = StockInfoFive[fi][0];
                    dr[1] = StockInfoFive[fi][1];
                    //dr[2] = ((DateTime)dtCloned[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dr[2] = ((DateTime)dtClonedFive[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dtTableWeekRed.Rows.Add(dr);

                    dr = dtTableWeekRed.NewRow();
                    for (int i = 3; i < dtTableWeekRed.Columns.Count; i++)
                    {
                        dr[i] = dtTableWeek[fi].Rows[end][i];
                    }
                    //dr[0] = StockInfo[fi][0];
                    dr[1] = StockInfoFive[fi][1];
                    //dr[2] = ((DateTime)dtCloned[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dr[2] = ((DateTime)dtClonedFive[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dtTableWeekRed.Rows.Add(dr);
                }
            }

            if (dtTableWeekRed.Rows.Count > 0)
            {
                ColorRed cr = new ColorRed();
                cr.Text = "一周有 4 个红格";
                cr.colorRedrowMergeView.DataSource = dtTableWeekRed;
                cr.colorRedrowMergeView.ColumnHeadersHeight = 40;
                cr.colorRedrowMergeView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                //cr.colorRedrowMergeView.MergeColumnNames.Add("Column1");
                cr.colorRedrowMergeView.MergeColumnNames.Add("Column2");
                //cr.colorRedrowMergeView.MergeColumnNames.Add("Column3");
                cr.Show();
            }
            else
            {
                MessageBox.Show("不存在 一周有 4 个红格 的情况！");
                return;
            }
        }

        private void Red5ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!isWeekKCalculated)
            {
                MessageBox.Show("请先计算周K值！");
                return;
            }

            dtTableWeekRed = new DataTable("dttableweekred");
            dtTableWeekRed.Columns.Add("id");
            dtTableWeekRed.Columns.Add("name");
            dtTableWeekRed.Columns.Add("date");
            dtTableWeekRed.Columns.Add("item");
            dtTableWeekRed.Columns.Add("one");
            dtTableWeekRed.Columns.Add("two");
            dtTableWeekRed.Columns.Add("three");
            dtTableWeekRed.Columns.Add("four");
            dtTableWeekRed.Columns.Add("five");
            dtTableWeekRed.Columns.Add("total");

            //int[][] isRed = new int[newFileNumber][];
            //for (int fi = 0; fi < newFileNumber; fi++)
                int[][] isRed = new int[newFileNumberFive][];
            for (int fi = 0; fi < newFileNumberFive; fi++)
            {
                isRed[fi] = new int[5] { 0, 0, 0, 0, 0 };

                int end = dtTableWeek[fi].Rows.Count - 1;

                if (dtTableWeek[fi].Rows[end][4].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][4].ToString()) > 0)
                {
                    isRed[fi][0] = 1;
                }
                if (dtTableWeek[fi].Rows[end][5].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][5].ToString()) > 0)
                {
                    isRed[fi][1] = 1;
                }
                if (dtTableWeek[fi].Rows[end][6].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][6].ToString()) > 0)
                {
                    isRed[fi][2] = 1;
                }
                if (dtTableWeek[fi].Rows[end][7].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][7].ToString()) > 0)
                {
                    isRed[fi][3] = 1;
                }
                if (dtTableWeek[fi].Rows[end][8].ToString() != "" && Convert.ToDouble(dtTableWeek[fi].Rows[end][8].ToString()) > 0)
                {
                    isRed[fi][4] = 1;
                }

                int sum = isRed[fi].Sum();

                if (sum == 5)
                {
                    //int end1 = dtCloned[fi].Rows.Count - 1;
                    int end1 = dtClonedFive[fi].Rows.Count - 1;
                    DataRow dr = dtTableWeekRed.NewRow();
                    for (int i = 3; i < dtTableWeekRed.Columns.Count; i++)
                    {
                        dr[i] = dtTableWeek[fi].Rows[end - 1][i];
                    }
                    dr[0] = StockInfoFive[fi][0];
                    dr[1] = StockInfoFive[fi][1];
                    //dr[2] = ((DateTime)dtCloned[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dr[2] = ((DateTime)dtClonedFive[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dtTableWeekRed.Rows.Add(dr);

                    dr = dtTableWeekRed.NewRow();
                    for (int i = 3; i < dtTableWeekRed.Columns.Count; i++)
                    {
                        dr[i] = dtTableWeek[fi].Rows[end][i];
                    }
                    //dr[0] = StockInfo[fi][0];
                    dr[1] = StockInfoFive[fi][1];
                    //dr[2] = ((DateTime)dtCloned[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dr[2] = ((DateTime)dtClonedFive[fi].Rows[end1][0]).ToString("yyyy/MM/dd");
                    dtTableWeekRed.Rows.Add(dr);
                }
            }

            if (dtTableWeekRed.Rows.Count > 0)
            {
                ColorRed cr = new ColorRed();
                cr.Text = "一周有 5 个红格";

                cr.colorRedrowMergeView.DataSource = dtTableWeekRed;
                cr.colorRedrowMergeView.ColumnHeadersHeight = 40;
                cr.colorRedrowMergeView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                //cr.colorRedrowMergeView.MergeColumnNames.Add("Column1");
                cr.colorRedrowMergeView.MergeColumnNames.Add("Column2");
                //cr.colorRedrowMergeView.MergeColumnNames.Add("Column3");
                ShowRowNumber(cr.colorRedrowMergeView);
                cr.Show();
            }
            else
            {
                MessageBox.Show("不存在 一周有 5 个红格 的情况！");
                return;
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            //open Txt files
            this.openFileDialog.Multiselect = true; //Enable select multiple files
            this.openFileDialog.Filter = "Txt Files|*.txt";

            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string FileName = this.openFileDialog.FileName;
                fileNumber = openFileDialog.FileNames.Length;

                //pg = new ProgressBar();
                //pg.progressBar_MeanLine.Value = 0;
                //pg.progressBar_MeanLine.Maximum = fileNumber;
                //pg.progressBar_MeanLine.Minimum = 0;
                //currentFileNumber = 0;
                //pg.Text = "正在打开文件 ......";
                //pg.Show();
                //pg.StartPosition = FormStartPosition.CenterParent;
                //pg.ShowDialog();


                dataGridView_all.Visible = false;
                AMinusBClicked = false;
                CloseMinusBClicked = false;

                fileOpened = false;
                isWeekKCalculated = false;
                isMeanLineCalculated = false;
                isMeanLine = false;
                isWeekK = false;

                isProgressBarCanceled = false;
                workerPB = new ProgressBar();
                workerPB.progressBar_MeanLine.Value = 0;
                workerPB.progressBar_MeanLine.Maximum = fileNumber;
                workerPB.progressBar_MeanLine.Minimum = 0;
                currentFileNumber = 0;

                worker = new BackgroundWorker();
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork_OpenFile);
                worker.RunWorkerAsync();
                workerPB.Text = "正在打开文件 ……";
                workerPB.StartPosition = FormStartPosition.CenterParent;
                workerPB.ShowDialog();
                

                // the open file operation is in worker_DoWork();

                if (!isProgressBarCanceled)
                {
                    dataGridView_all.Visible = true;
                    dataGridView_all.DataSource = dtTablesAllFiles;
                    fileOpened = true;
                    isStockDetail = true;
                    isMeanLine = false;
                    ShowRowNumber(dataGridView_all);
                }
            }
        }

        //成-基 and 周累计 and 多空排列
        private void menuLastDay_Click_old(object sender, EventArgs e)
        {
            if (!isWeekKCalculated)
            {
                MessageBox.Show("请先计算周K值！");
                return;
            }

            m_dtLastDay = new DataTable("lastday");
            m_dtLastDay.Columns.Add("日期", typeof(DateTime));//0
            m_dtLastDay.Columns.Add("股票代码", typeof(string));
            m_dtLastDay.Columns.Add("股票名称", typeof(string));
            m_dtLastDay.Columns.Add("上月基准量（亿）", typeof(double));
            m_dtLastDay.Columns.Add("成-基（亿）", typeof(double));//4

            m_dtLastDay.Columns.Add("周累计（亿）", typeof(double));
            m_dtLastDay.Columns.Add("上周累计（亿）", typeof(double));//6
            m_dtLastDay.Columns.Add("上上周累计（亿）", typeof(double));//7
            m_dtLastDay.Columns.Add("增长比 (%)", typeof(double));
            //For 多空排列
            m_dtLastDay.Columns.Add("mma评分", typeof(double));//9
            m_dtLastDay.Columns.Add("mma个数", typeof(int));//10
            //mma变化
            m_dtLastDay.Columns.Add("mma增减", typeof(string));//11


            DataTable dtTableMeanCopy = dtTableMean.Copy();

            //mma60forPingFen = new double[newFileNumber];
            mma60forPingFen = new double[fileNumber];

            //for (int fi = 0; fi < newFileNumber; fi++)
                for (int fi = 0; fi < fileNumber; fi++)
                {
                string str = dtTableMeanCopy.Rows[fi][0].ToString();
                //DateTime date = DateTime.ParseExact(str, "yyyy/M/dd H:mm:ss", CultureInfo.InvariantCulture);
                DateTime date = DateTime.Parse(str);

                DataRow row = m_dtLastDay.NewRow();
                row[0] = date;
                row[1] = dtTableMeanCopy.Rows[fi][1];
                row[2] = dtTableMeanCopy.Rows[fi][2];

                int lastRowIdx = dtTableWeek[fi].Rows.Count - 1;
                // Here should define the day of week
                int day = (int)date.DayOfWeek;
                row[4] = dtTableWeek[fi].Rows[lastRowIdx][day + 3];

                //int lastrow = TotalRows[fi] - 1;
                // 上月基准量 = 成交额 - （成-基）
                //row[4] = Math.Round(Convert.ToDouble(dtCloned[fi].Rows[lastrow][6].ToString())/100000000, 2) - Convert.ToDouble(row[3].ToString());
                // 另一种计算方法，直接从dtTableMonth中读取，倒数第二行，最后一列;
                row[3] = Convert.ToDouble(dtTableMonth[fi].Rows[dtTableMonth[fi].Rows.Count - 2][4].ToString());

                row[5] = dtTableWeek[fi].Rows[lastRowIdx][9];
                row[6] = dtTableWeek[fi].Rows[lastRowIdx - 2][9];
                row[7] = dtTableWeek[fi].Rows[lastRowIdx - 4][9];

                double thisweek = Convert.ToDouble(dtTableWeek[fi].Rows[lastRowIdx][9].ToString());
                double lastweek = Convert.ToDouble(dtTableWeek[fi].Rows[lastRowIdx - 2][9].ToString());
                if (thisweek >= lastweek)
                {
                    row[8] = Math.Round(Math.Abs((thisweek - lastweek) / lastweek) * 100, 2);
                }
                else
                {
                    row[8] = Math.Round(-Math.Abs((thisweek - lastweek) / lastweek) * 100, 2);
                }

                double pingfen = 0;
                double pingfenLastDay = 0;
                int total = 0;
                int lastDayTotal = 0;

                if (isMeanLineCalculated)
                {
                    //单一股票的最后一行
                    int lastrow = dtTableMeanLine[fi].Rows.Count - 1;
                    double mmaValue = 0;

                    for (int i = 0; i < 7; i++)
                    {
                        if (dtTableMeanLine[fi].Rows[lastrow][i + 7].ToString() != null)
                        {
                            total++;

                            mmaValue = dtTableMeanLine[fi].Rows[lastrow].Field<double>(i + 7);
                            if (mmaValue > 0)
                            {
                                pingfen += 1;
                            }
                            else if (mmaValue < 0)
                            {
                                pingfen += 0;
                            }
                            else
                            {
                                pingfen += 0.5;
                            }

                            if (i == 4)
                            {
                                mma60forPingFen[fi] = mmaValue;
                            }
                        }

                        //末日的前一天
                        if (dtTableMeanLine[fi].Rows[lastrow - 1][i + 7].ToString() != null)
                        {
                            lastDayTotal++;

                            mmaValue = dtTableMeanLine[fi].Rows[lastrow - 1].Field<double>(i + 7);
                            if (mmaValue > 0)
                            {
                                pingfenLastDay += 1;
                            }
                            else if (mmaValue < 0)
                            {
                                pingfenLastDay += 0;
                            }
                            else
                            {
                                pingfenLastDay += 0.5;
                            }
                        }
                    }
                }
                

              
                row[9] = pingfen;
                row[10] = total;

                

                if (pingfen > pingfenLastDay)
                {
                    row[11] = "增加";
                }
                else if (pingfen < pingfenLastDay)
                {
                    row[11] = "减少";
                }
                else
                {
                    row[11] = "不变";
                }

                m_dtLastDay.Rows.Add(row);
            }

            LastDay ld = new LastDay();
            ld.dataGridView_LastDay.DataSource = m_dtLastDay;
            //禁止增长比排序;
            //ld.dataGridView_LastDay.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;

            //禁止mma个数排序;
            ld.dataGridView_LastDay.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            ShowRowNumber(ld.dataGridView_LastDay);
            ld.StartPosition = FormStartPosition.Manual;
            ld.Location = new Point(10, 10);
            ld.Show();
        }


        private void menuLastDay_Click(object sender, EventArgs e)
        {
            if (!isWeekKCalculated)
            {
                MessageBox.Show("请先计算周K值！");
                return;
            }

            m_dtLastDay = new DataTable("lastday");
            m_dtLastDay.Columns.Add("日期", typeof(DateTime));//0
            m_dtLastDay.Columns.Add("股票代码", typeof(string));
            m_dtLastDay.Columns.Add("股票名称", typeof(string));
            m_dtLastDay.Columns.Add("上月xpl（亿）", typeof(double));
            m_dtLastDay.Columns.Add("ewv-xpl（亿）", typeof(double));//4
            
            m_dtLastDay.Columns.Add("周累计（亿）", typeof(double));
            m_dtLastDay.Columns.Add("上周累计（亿）", typeof(double));//6
            m_dtLastDay.Columns.Add("上上周累计（亿）", typeof(double));//7
            m_dtLastDay.Columns.Add("增长比 (%)", typeof(double));
            //For 多空排列
            m_dtLastDay.Columns.Add("mma评分", typeof(double));//9
            m_dtLastDay.Columns.Add("mma个数", typeof(int));//10
            //mma变化
            m_dtLastDay.Columns.Add("mma增减", typeof(string));//11
            //显示mma5倒数三天数据
            m_dtLastDay.Columns.Add("T1", typeof(double));//12
            m_dtLastDay.Columns.Add("T2", typeof(double));
            m_dtLastDay.Columns.Add("T3", typeof(double));
            //正推
            m_dtLastDay.Columns.Add("Z5", typeof(double));//15
            m_dtLastDay.Columns.Add("Z10", typeof(double));
            m_dtLastDay.Columns.Add("Z20", typeof(double));
            m_dtLastDay.Columns.Add("Z30", typeof(double));
            m_dtLastDay.Columns.Add("Z60", typeof(double));
            m_dtLastDay.Columns.Add("Z120", typeof(double));
            m_dtLastDay.Columns.Add("Z250", typeof(double));
            //AB
            m_dtLastDay.Columns.Add("AB", typeof(string));//22
            //反推 F5 = ma4 - c5/3; F10 = ma9 - c10/8
            m_dtLastDay.Columns.Add("F5", typeof(double));//23
            m_dtLastDay.Columns.Add("F10", typeof(double));
            m_dtLastDay.Columns.Add("F20", typeof(double));
            m_dtLastDay.Columns.Add("F30", typeof(double));
            m_dtLastDay.Columns.Add("F60", typeof(double));
            m_dtLastDay.Columns.Add("F120", typeof(double));
            m_dtLastDay.Columns.Add("F250", typeof(double));

            //收盘价除以均线值
            m_dtLastDay.Columns.Add("收/均", typeof(double));//30
            m_dtLastDay.Columns.Add("上下差", typeof(double));
            m_dtLastDay.Columns.Add("求平均", typeof(double));//32
            m_dtLastDay.Columns.Add("平均差", typeof(double));
            m_dtLastDay.Columns.Add("收盘价", typeof(double));

            m_dtLastDay.Columns.Add("ma5", typeof(int));//35
            m_dtLastDay.Columns.Add("ma10", typeof(int));
            m_dtLastDay.Columns.Add("ma20", typeof(int));
            m_dtLastDay.Columns.Add("ma30", typeof(int));
            m_dtLastDay.Columns.Add("ma60", typeof(int));
            m_dtLastDay.Columns.Add("ma120", typeof(int));
            m_dtLastDay.Columns.Add("ma250", typeof(int));

            DataTable dtTableMeanCopy = dtTableMean.Copy();

            //mma60forPingFen = new double[newFileNumber];
            mma60forPingFen = new double[newFileNumberFive];
            color_for_ma = new bool[newFileNumberFive][];

            for (int fi = 0; fi < newFileNumberFive; fi++)
                //for (int fi=0; fi < newFileNumber; fi++)
            {
                string str = dtTableMeanCopy.Rows[fi][0].ToString();
                //DateTime date = DateTime.ParseExact(str, "yyyy/M/dd H:mm:ss", CultureInfo.InvariantCulture);
                DateTime date = DateTime.Parse(str);

                DataRow row = m_dtLastDay.NewRow();
                row[0] = date;
                row[1] = dtTableMeanCopy.Rows[fi][1];
                row[2] = dtTableMeanCopy.Rows[fi][2];

                int lastRowIdx = dtTableWeek[fi].Rows.Count - 1;
                // Here should define the day of week
                int day = (int)date.DayOfWeek;
                row[4] = dtTableWeek[fi].Rows[lastRowIdx][day + 3];

                //int lastrow = TotalRows[fi] - 1;
                // 上月基准量 = 成交额 - （成-基）
                //row[4] = Math.Round(Convert.ToDouble(dtCloned[fi].Rows[lastrow][6].ToString())/100000000, 2) - Convert.ToDouble(row[3].ToString());
                // 另一种计算方法，直接从dtTableMonth中读取，倒数第二行，最后一列;
                //row[3] = Convert.ToDouble(dtTableMonth[fi].Rows[dtTableMonth[fi].Rows.Count - 2][4].ToString());
                if (dtTableMonth[fi].Rows.Count > 1) //针对股票数据只出现了一个月的情况
                {
                    row[3] = Convert.ToDouble(dtTableMonth[fi].Rows[dtTableMonth[fi].Rows.Count - 2][4].ToString());
                }
                

                row[5] = dtTableWeek[fi].Rows[lastRowIdx][9];
                //row[6] = dtTableWeek[fi].Rows[lastRowIdx-2][9];
                //row[7] = dtTableWeek[fi].Rows[lastRowIdx-4][9];
                if (lastRowIdx >= 3)
                {
                    row[6] = dtTableWeek[fi].Rows[lastRowIdx - 2][9];
                }
                if (lastRowIdx >= 5)
                {
                    row[7] = dtTableWeek[fi].Rows[lastRowIdx - 4][9];
                }

                double thisweek = Convert.ToDouble(dtTableWeek[fi].Rows[lastRowIdx][9].ToString());
                double lastweek = Convert.ToDouble(dtTableWeek[fi].Rows[lastRowIdx-2][9].ToString());
                if (thisweek >= lastweek)
                {
                    row[8] = Math.Round(Math.Abs((thisweek - lastweek) / lastweek) * 100, 2);
                }
                else
                {
                    row[8] = Math.Round(-Math.Abs((thisweek - lastweek) / lastweek) * 100, 2);
                }

                //收盘价
                //int lastrow = dtCloned[fi].Rows.Count - 1;
                int lastrow = dtClonedFive[fi].Rows.Count - 1;
                //double shoupan = Convert.ToDouble(dtCloned[fi].Rows[lastrow][4].ToString());

                double pingfen = 0;
                double pingfenLastDay = 0;
                int total = 0;
                int lastDayTotal = 0;

                //mma
                if (lastrow > 249)
                {
                    total = 7;
                }
                else if (lastrow > 119)
                {
                    total = 6;
                }
                else if (lastrow > 59)
                {
                    total = 5;
                }
                else if (lastrow > 29)
                {
                    total = 4;
                }
                else if (lastrow > 19)
                {
                    total = 3;
                }
                else if (lastrow > 9)
                {
                    total = 2;
                }
                else if (lastrow > 4)
                {
                    total = 1;
                }
                else
                {
                    total = 0;
                }

                //末日的前一天
                if (lastrow - 1 > 249)
                {
                    lastDayTotal = 7;
                }
                else if (lastrow - 1 > 119)
                {
                    lastDayTotal = 6;
                }
                else if (lastrow - 1 > 59)
                {
                    lastDayTotal = 5;
                }
                else if (lastrow - 1 > 29)
                {
                    lastDayTotal = 4;
                }
                else if (lastrow - 1 > 19)
                {
                    lastDayTotal = 3;
                }
                else if (lastrow - 1 > 9)
                {
                    lastDayTotal = 2;
                }
                else if (lastrow - 1 > 4)
                {
                    lastDayTotal = 1;
                }
                else
                {
                    lastDayTotal = 0;
                }

                double[] mmaValue = new double[total];
                double[] maValue = new double[total];
                for (int mmaIdx=0; mmaIdx<total; mmaIdx++)
                {
                    switch(mmaIdx)
                    {
                        case 0:
                            //mmaValue[mmaIdx] = (double)dtCloned[fi].Rows[lastrow][4] - mma(5, lastrow, dtCloned[fi]);
                            mmaValue[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow][4] - mma(5, lastrow, dtClonedFive[fi]);
                            maValue[mmaIdx] = ma(5, lastrow, dtClonedFive[fi]);
                            row[12] = Math.Round(mmaValue[mmaIdx], 2);
                            if (lastrow-1>4)
                            {
                                //row[13] = Math.Round((double)dtCloned[fi].Rows[lastrow-1][4] - mma(5, lastrow-1, dtCloned[fi]), 2);
                                row[13] = Math.Round((double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(5, lastrow - 1, dtClonedFive[fi]), 2);
                            }
                            if (lastrow-2>4)
                            {
                                //row[14] = Math.Round((double)dtCloned[fi].Rows[lastrow-2][4] - mma(5, lastrow-2, dtCloned[fi]), 2);
                                row[14] = Math.Round((double)dtClonedFive[fi].Rows[lastrow - 2][4] - mma(5, lastrow - 2, dtClonedFive[fi]), 2);
                            }
                            row[15] = Math.Round(mmaValue[mmaIdx], 2);
                            //double mav = ma(3, lastrow - 1, dtCloned[fi]);
                            double mav = ma(4, lastrow, dtClonedFive[fi])*4.0/3 - (double)dtClonedFive[fi].Rows[lastrow-4][4]/3.0;
                            row[23] = Math.Round(mav, 2);
                            //if (mav >= (double)dtCloned[fi].Rows[lastrow][4])
                                if (mav >= (double)dtClonedFive[fi].Rows[lastrow][4])
                                {
                                row[22] = "A";
                            }
                            else
                            {
                                row[22] = "B";
                            }
                            break;
                        case 1:
                            //mmaValue[mmaIdx] = (double)dtCloned[fi].Rows[lastrow][4] - mma(10, lastrow, dtCloned[fi]);
                            mmaValue[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow][4] - mma(10, lastrow, dtClonedFive[fi]);
                            maValue[mmaIdx] = ma(10, lastrow, dtClonedFive[fi]);
                            row[16] = Math.Round(mmaValue[mmaIdx], 2);
                            //row[24] = Math.Round(ma(8, lastrow-1, dtCloned[fi]), 2);
                            row[24] = Math.Round(ma(9, lastrow, dtClonedFive[fi]) * 9.0 / 8 - (double)dtClonedFive[fi].Rows[lastrow - 9][4] / 8.0, 2);
                            break;
                        case 2:
                            //mmaValue[mmaIdx] = (double)dtCloned[fi].Rows[lastrow][4] - mma(20, lastrow, dtCloned[fi]);
                            mmaValue[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow][4] - mma(20, lastrow, dtClonedFive[fi]);
                            maValue[mmaIdx] = ma(20, lastrow, dtClonedFive[fi]);
                            row[17] = Math.Round(mmaValue[mmaIdx], 2);
                            //row[25] = Math.Round(ma(18, lastrow-1, dtCloned[fi]), 2);
                            row[25] = Math.Round(ma(19, lastrow, dtClonedFive[fi]) * 19.0 / 18 - (double)dtClonedFive[fi].Rows[lastrow - 19][4] / 18.0, 2);
                            break;
                        case 3:
                            //mmaValue[mmaIdx] = (double)dtCloned[fi].Rows[lastrow][4] - mma(30, lastrow, dtCloned[fi]);
                            mmaValue[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow][4] - mma(30, lastrow, dtClonedFive[fi]);
                            maValue[mmaIdx] = ma(30, lastrow, dtClonedFive[fi]);
                            row[18] = Math.Round(mmaValue[mmaIdx], 2);
                            //row[26] = Math.Round(ma(28, lastrow-1, dtCloned[fi]), 2);
                            row[26] = Math.Round(ma(29, lastrow, dtClonedFive[fi]) * 29.0 / 28 - (double)dtClonedFive[fi].Rows[lastrow - 29][4] / 28.0, 2);
                            break;
                        case 4:
                            //mmaValue[mmaIdx] = (double)dtCloned[fi].Rows[lastrow][4] - mma(60, lastrow, dtCloned[fi]);
                            mmaValue[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow][4] - mma(60, lastrow, dtClonedFive[fi]);
                            maValue[mmaIdx] = ma(60, lastrow, dtClonedFive[fi]);
                            mma60forPingFen[fi] = mmaValue[mmaIdx];
                            row[19] = Math.Round(mmaValue[mmaIdx], 2);
                            //row[27] = Math.Round(ma(58, lastrow-1, dtCloned[fi]), 2);
                            row[27] = Math.Round(ma(59, lastrow, dtClonedFive[fi]) * 59.0 / 58 - (double)dtClonedFive[fi].Rows[lastrow - 59][4] / 58.0, 2);
                            break;
                        case 5:
                            //mmaValue[mmaIdx] = (double)dtCloned[fi].Rows[lastrow][4] - mma(120, lastrow, dtCloned[fi]);
                            mmaValue[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow][4] - mma(120, lastrow, dtClonedFive[fi]);
                            maValue[mmaIdx] = ma(120, lastrow, dtClonedFive[fi]);
                            row[20] = Math.Round(mmaValue[mmaIdx], 2);
                            //row[28] = Math.Round(ma(118, lastrow-1, dtCloned[fi]), 2);
                            row[28] = Math.Round(ma(119, lastrow, dtClonedFive[fi]) * 119.0 / 118 - (double)dtClonedFive[fi].Rows[lastrow - 119][4] / 118.0, 2);
                            break;
                        case 6:
                            //mmaValue[mmaIdx] = (double)dtCloned[fi].Rows[lastrow][4] - mma(250, lastrow, dtCloned[fi]);
                            mmaValue[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow][4] - mma(250, lastrow, dtClonedFive[fi]);
                            maValue[mmaIdx] = ma(250, lastrow, dtClonedFive[fi]);
                            row[21] = Math.Round(mmaValue[mmaIdx], 2);
                            //row[29] = Math.Round(ma(248, lastrow-1, dtCloned[fi]), 2);
                            row[29] = Math.Round(ma(249, lastrow, dtClonedFive[fi]) * 249.0 / 148 - (double)dtClonedFive[fi].Rows[lastrow - 249][4] / 248.0, 2);
                            break;
                        default:
                            break;
                    }

                    if (mmaValue[mmaIdx] > 0)
                    {
                        pingfen += 1;
                    }
                    else if (mmaValue[mmaIdx] < 0)
                    {
                        pingfen += 0;
                    }
                    else
                    {
                        pingfen += 0.5;
                    }
                }
                row[9] = pingfen;
                row[10] = total;

                //计算均线值：mma求平均
                double mean = 0.0;
                for (int mmaIdx = 0; mmaIdx < total; mmaIdx++)
                {
                    mean += maValue[mmaIdx];
                }
                mean = mean / total;
                row[30] = Math.Round((double)dtClonedFive[fi].Rows[lastrow][4] / mean, 2);
                row[32] = Math.Round(mean, 2);
                row[33] = Math.Round((double)dtClonedFive[fi].Rows[lastrow][4] - mean, 2);
                row[34] = Math.Round((double)dtClonedFive[fi].Rows[lastrow][4], 2);

                //找出ma的最大值和最小值
                double max_ma = maValue[0];
                double min_ma = maValue[0];
                for (int maIdx = 1; maIdx < total; maIdx++)
                {
                    if (maValue[maIdx] > max_ma)
                    {
                        max_ma = maValue[maIdx];
                    }
                    if (maValue[maIdx] < min_ma)
                    {
                        min_ma = maValue[maIdx];
                    }
                }
                row[31] = Math.Round((max_ma - min_ma) / (double)dtClonedFive[fi].Rows[lastrow][4], 5);

                //ma排序
                double[] maValue_temp = (double[])maValue.Clone();
                Array.Sort(maValue_temp);
                color_for_ma[fi] = new bool[total];
                for (int maIdx = 0; maIdx < total; maIdx++)
                {
                    row[35+maIdx] = Array.IndexOf(maValue_temp, maValue[maIdx]) + 1;
                    if (maValue[maIdx] > (double)dtClonedFive[fi].Rows[lastrow][4])
                    {
                        color_for_ma[fi][maIdx] = true;
                    }
                    else
                    {
                        color_for_ma[fi][maIdx] = false;
                    }
                }

                //末日前一天
                double[] mmaValueLastDay = new double[lastDayTotal];
                for (int mmaIdx = 0; mmaIdx < lastDayTotal; mmaIdx++)
                {
                    switch (mmaIdx)
                    {
                        case 0:
                            //mmaValueLastDay[mmaIdx] = (double)dtCloned[fi].Rows[lastrow-1][4] - mma(5, lastrow - 1, dtCloned[fi]);
                            mmaValueLastDay[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(5, lastrow - 1, dtClonedFive[fi]);
                            break;
                        case 1:
                            //mmaValueLastDay[mmaIdx] = (double)dtCloned[fi].Rows[lastrow - 1][4] - mma(10, lastrow - 1, dtCloned[fi]);
                            mmaValueLastDay[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(10, lastrow - 1, dtClonedFive[fi]);
                            break;
                        case 2:
                            //mmaValueLastDay[mmaIdx] = (double)dtCloned[fi].Rows[lastrow - 1][4] - mma(20, lastrow - 1, dtCloned[fi]);
                            mmaValueLastDay[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(20, lastrow - 1, dtClonedFive[fi]);
                            break;
                        case 3:
                            //mmaValueLastDay[mmaIdx] = (double)dtCloned[fi].Rows[lastrow - 1][4] - mma(30, lastrow - 1, dtCloned[fi]);
                            mmaValueLastDay[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(30, lastrow - 1, dtClonedFive[fi]);
                            break;
                        case 4:
                            //mmaValueLastDay[mmaIdx] = (double)dtCloned[fi].Rows[lastrow - 1][4] - mma(60, lastrow - 1, dtCloned[fi]);
                            mmaValueLastDay[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(60, lastrow - 1, dtClonedFive[fi]);
                            break;
                        case 5:
                            //mmaValueLastDay[mmaIdx] = (double)dtCloned[fi].Rows[lastrow - 1][4] - mma(120, lastrow - 1, dtCloned[fi]);
                            mmaValueLastDay[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(120, lastrow - 1, dtClonedFive[fi]);
                            break;
                        case 6:
                            //mmaValueLastDay[mmaIdx] = (double)dtCloned[fi].Rows[lastrow - 1][4] - mma(250, lastrow - 1, dtCloned[fi]);
                            mmaValueLastDay[mmaIdx] = (double)dtClonedFive[fi].Rows[lastrow - 1][4] - mma(250, lastrow - 1, dtClonedFive[fi]);
                            break;
                        default:
                            break;
                    }

                    if (mmaValueLastDay[mmaIdx] > 0)
                    {
                        pingfenLastDay += 1;
                    }
                    else if (mmaValueLastDay[mmaIdx] < 0)
                    {
                        pingfenLastDay += 0;
                    }
                    else
                    {
                        pingfenLastDay += 0.5;
                    }
                }

                if(pingfen>pingfenLastDay)
                {
                    row[11] = "增加";
                }
                else if(pingfen<pingfenLastDay)
                {
                    row[11] = "减少";
                }
                else
                {
                    row[11] = "不变";
                }

                m_dtLastDay.Rows.Add(row);
            }
            LastDay ld = new LastDay();
            ld.dataGridView_LastDay.DataSource = m_dtLastDay;
            //禁止增长比排序;
            //ld.dataGridView_LastDay.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            
            //禁止mma个数排序;
            ld.dataGridView_LastDay.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            ShowRowNumber(ld.dataGridView_LastDay);
            ld.StartPosition = FormStartPosition.Manual;
            //ld.Location = new Point(10, 10);
            //ld.Width = 3000;
            //ld.Size = new System.Drawing.Size(2500, 800);
            ld.Show();
        }

        // For backgroundworker
        void worker_DoWork_OpenFile(object sender, DoWorkEventArgs e)
        {
            // open file and show progress bar

            DataTable[] dtClonedTemp = new DataTable[fileNumber];
            //StockInfo = new string[fileNumber][];//Stock number and name of each file
            string[][] StockInfoTemp = new string[fileNumber][];
            //dtCloned = new DataTable[fileNumber];
            TotalRows = new int[fileNumber];
            ///cValues = new double[fileNumber][];//C values
            ///closeMinusB = new double[fileNumber][];
            ///K = new bool[fileNumber][];
            ///aValues = new double[fileNumber][];
            ///escape = new int[fileNumber];
            ///Array.Clear(escape, 0, fileNumber);// Initial escape with 0
            escapeFive = new int[fileNumber];
            Array.Clear(escapeFive, 0, fileNumber);

            isProgressBarCanceled = false;
            for (int fi = 0; fi < fileNumber; fi++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    isProgressBarCanceled = true;
                    break;
                }

                currentFileNumber = fi;
                //timer_progressBar_Tick1();
                worker.ReportProgress(currentFileNumber);

                try
                {
                    FileName = openFileDialog.FileNames[fi].ToString();

                    var filestream = new System.IO.FileStream(FileName,
                                      System.IO.FileMode.Open,
                                      System.IO.FileAccess.Read,
                                      System.IO.FileShare.ReadWrite);
                    var file = new System.IO.StreamReader(filestream, System.Text.Encoding.Default, true, 128);
                    string line;
                    int LineNumber = 0;
                    dtClonedTemp[fi] = new DataTable("temp");
                    while ((line = file.ReadLine()) != null)
                    {
                        //string[] data = line.Split(null);

                        if (LineNumber == 0)// the first line
                        {
                            StockInfoTemp[fi] = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                            LineNumber++;
                            continue;
                        }

                        string[] data = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                        if (LineNumber == 1)// the second line
                        {
                            for (int c = 0; c < data.Length; c++)
                            {
                                dtClonedTemp[fi].Columns.Add(data[c], typeof(double));
                            }
                            dtClonedTemp[fi].Columns[0].DataType = typeof(DateTime);
                            LineNumber++;
                            continue;
                        }

                        if (data.Length < 7)// reach the file end
                            break;

                        DataRow row = dtClonedTemp[fi].NewRow();
                        for (int c = 0; c < data.Length; c++)
                        {
                            row[c] = data[c];
                        }
                        dtClonedTemp[fi].Rows.Add(row);
                        LineNumber++;
                    }

                    LineNumber -= 2;
                    //if (LineNumber <= 75)
                    //{
                    //    escape[fi] = 1;
                    //    ///continue;
                    //}
                    if (LineNumber <= 5)
                    {
                        escapeFive[fi] = 1;
                        continue;
                    }

                    TotalRows[fi] = LineNumber;
                    ///aValues[fi] = new double[TotalRows[fi]];//Store A values;
                    ///cValues[fi] = new double[TotalRows[fi]];//Store C values;
                    ///closeMinusB[fi] = new double[TotalRows[fi]];
                    ///K[fi] = new bool[TotalRows[fi]];
                    /*
                    int i, j, k;
                    double sumXY = 0, sumX = 0, sumY = 0, sumX2 = 0;
                    double[] slopes = new double[TotalRows[fi]];//Store slopes of every 21 days
                    double[] bValues = new double[TotalRows[fi]];//Store B values;
                    double[] yValues = new double[21];
                    double[] closeMinusA = new double[TotalRows[fi]];
                    for (i = 0; i < TotalRows[fi]; i++)//the first two rows of range should be escaped
                    {
                        k = i;
                        sumXY = 0; sumX = 0; sumY = 0; sumX2 = 0;

                        for (j = 0; j < 21; j++)
                        {
                            if (k >= TotalRows[fi])
                                break;//Already touch the end row

                            yValues[j] = (double)dtClonedTemp[fi].Rows[k][4];
                            k++;

                            sumXY += xValues[j] * yValues[j];
                            sumX += xValues[j];
                            sumY += yValues[j];
                            sumX2 += xValues[j] * xValues[j];
                        }

                        slopes[k - 1] = (sumXY * 21 - sumX * sumY) / (21 * sumX2 - sumX * sumX);
                        slopes[k - 1] = slopes[k - 1] * 20 + (double)dtClonedTemp[fi].Rows[k - 1][4];
                        if (k == TotalRows[fi])
                            break;
                    }

                    //Calculate the 55 days EMA
                    double sumTemp = 0;
                    for (i = 0; i < TotalRows[fi]; i++)//the first two rows of range should be escaped
                    {
                        k = i + 20;
                        if (k == 20)//Only calculate for once
                        {
                            sumTemp = 0;
                            for (j = 0; j < 55; j++)
                            {
                                sumTemp += slopes[k++];//slopes index start from 22
                            }

                            aValues[fi][74] = sumTemp / 55;
                        }

                        k = i + 75; //aValues index starts from 74
                        aValues[fi][k] = (slopes[k] + aValues[fi][k - 1] * 27) / 28;
                        aValues[fi][k - 1] = Math.Round(aValues[fi][k - 1], 2);//Round the value after it has been used
                        if (k + 1 == TotalRows[fi])
                        {
                            aValues[fi][k] = Math.Round(aValues[fi][k], 2);//Round the value of the end row
                            break;
                        }
                    }

                    //Calculate B values
                    double[] bTemp = new double[60];
                    for (i = 0; i < TotalRows[fi]; i++)
                    {
                        k = i;
                        for (j = 0; j < 60; j++)
                        {
                            bTemp[j] = aValues[fi][k++];
                        }
                        bValues[k - 1] = bTemp.Max();
                        if (k == TotalRows[fi])
                        {
                            break;
                        }
                    }
                    */
                    /*
                    dtClonedTemp[fi].Columns.Add("A", typeof(double));
                    dtClonedTemp[fi].Columns.Add("B", typeof(double));
                    dtClonedTemp[fi].Columns.Add("黄K", typeof(bool));
                    dtClonedTemp[fi].Columns.Add("C", typeof(double));
                    dtClonedTemp[fi].Columns.Add("收盘-A", typeof(double));
                    dtClonedTemp[fi].Columns.Add("收盘-B", typeof(double));
                    dtClonedTemp[fi].Columns.Add("A-最低", typeof(double));
                    for (i = 0; i < TotalRows[fi]; i++)
                    {
                        cValues[fi][i] = Math.Round(bValues[i] - aValues[fi][i], 2);//Calculate C values
                        dtClonedTemp[fi].Rows[i][7] = aValues[fi][i];
                        dtClonedTemp[fi].Rows[i][8] = bValues[i];
                        closeMinusB[fi][i] = Math.Round((double)dtClonedTemp[fi].Rows[i][4] - bValues[i], 2);
                        closeMinusA[i] = Math.Round((double)dtClonedTemp[fi].Rows[i][4] - aValues[fi][i], 2);
                        if (i == 0)
                        {
                            K[fi][i] = false;
                        }
                        else
                        {
                            double open = (double)dtClonedTemp[fi].Rows[i][1];
                            double closeOneDayAgo = (double)dtClonedTemp[fi].Rows[i - 1][4];
                            double close = (double)dtClonedTemp[fi].Rows[i][4];
                            if (close > 1.098 * closeOneDayAgo && close > open)// calculate K
                            {
                                K[fi][i] = true;
                            }
                        }


                        dtClonedTemp[fi].Rows[i][9] = K[fi][i];
                        if (bValues[i] == 0)
                        {
                            //dtClonedTemp[fi].Rows[i][10] = 0;
                            //dtClonedTemp[fi].Rows[i][12] = 0;
                        }
                        else
                        {
                            dtClonedTemp[fi].Rows[i][10] = (cValues[fi][i]) / bValues[i];
                            dtClonedTemp[fi].Rows[i][12] = closeMinusB[fi][i] / bValues[i];
                        }
                        if (aValues[fi][i] == 0)
                        {
                            //dtClonedTemp[fi].Rows[i][11] = 0;
                            //dtClonedTemp[fi].Rows[i][13] = 0;
                        }
                        else
                        {
                            dtClonedTemp[fi].Rows[i][11] = closeMinusA[i] / aValues[fi][i];
                            dtClonedTemp[fi].Rows[i][13] = (aValues[fi][i] - (double)dtClonedTemp[fi].Rows[i][3]) / aValues[fi][i];
                        }
                    }
                    */
                }
                catch
                {
                    MessageBox.Show("错误！");
                    return;
                }
            }

            if (!isProgressBarCanceled)
            {
                dtTablesAllFiles = new DataTable("All");
                dtTablesAllFiles.Columns.Add("日期", typeof(DateTime));
                //dtTablesAllFiles.Columns[0].DataType = typeof(DateTime);
                dtTablesAllFiles.Columns.Add("股票代码", typeof(string));
                dtTablesAllFiles.Columns.Add("股票名称", typeof(string));
                dtTablesAllFiles.Columns.Add("收盘价", typeof(string));
                //dtTablesAllFiles.Columns.Add("A", typeof(double));
                //dtTablesAllFiles.Columns.Add("B", typeof(double));

                ///newFileNumber = fileNumber - escape.Sum();
                newFileNumberFive = fileNumber - escapeFive.Sum();
                ///AMinusBTableIndex = new int[newFileNumber];
                ///CloseMinusBTableIndex = new int[newFileNumber];

                ///dtCloned = new DataTable[newFileNumber];
                dtClonedFive = new DataTable[newFileNumberFive];
                for (int i = 0; i < newFileNumberFive; i++)
                {
                    dtClonedFive[i] = dtClonedTemp[i].Copy();
                }


                ///StockInfo = new string[newFileNumber][];//Stock number and name of each file
                StockInfoFive = new string[newFileNumberFive][];
                //int idx = 0;
                //for (int i = 0; i < newFileNumber; i++)
                //{
                //    while (escape[idx] > 0)
                //    {
                //        idx++;// escaped files
                //    }

                //    dtCloned[i] = dtClonedTemp[idx].Copy();
                //    StockInfo[i] = StockInfoFive[idx];
                //    idx++;
                //}

                int idx = 0;
                for (int i = 0; i < newFileNumberFive; i++)
                {
                    while (escapeFive[idx] > 0)
                    {
                        idx++;// escaped files
                    }

                    dtClonedFive[i] = dtClonedTemp[idx].Copy();
                    StockInfoFive[i] = StockInfoTemp[idx];
                    idx++;
                }

                for (int i = 0; i < newFileNumberFive; i++)
                    //for (int i = 0; i < newFileNumber; i++)
                {
                    DataRow row = dtTablesAllFiles.NewRow();
                    //row[0] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][0];//date
                    row[0] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][0];//date
                    //row[1] = StockInfo[i][0];//stock number
                    //row[2] = StockInfo[i][1];//stock name
                    //row[3] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][4];//A
                    //row[4] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][7];//A
                    //row[5] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][8];//B

                    row[1] = StockInfoFive[i][0];//stock number
                    row[2] = StockInfoFive[i][1];//stock name
                    row[3] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][4];//A
                    //row[4] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][7];//A
                    //row[5] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][8];//B

                    dtTablesAllFiles.Rows.Add(row);
                }
                dtTableMean = dtTablesAllFiles.Copy();
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                MessageBox.Show("已取消！");
                workerPB.Close();
            }
            else if (e.Error != null)
            {
                MessageBox.Show("错误： " + e.Error.Message);
            }
            else
            {
                workerPB.Close();
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            workerPB.ProgressValue = e.ProgressPercentage;
        }

        void worker_DoWork_MeanLine(object sender, DoWorkEventArgs e)
        {
            //for (int fi = 0; fi < newFileNumber; fi++)
            for (int fi = 0; fi < newFileNumberFive; fi++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    isMeanLineCalculated = false;
                    isProgressBarCanceled = true;
                    isMeanLine = false;
                    if (!isWeekKCalculated)
                    {
                        isWeekK = false;
                        isStockDetail = true;
                    }
                    break;
                }

                currentFileNumber = fi;
                //timer_progressBar_Tick1();
                worker.ReportProgress(currentFileNumber);

                //dtTableMeanLine[fi] = dtCloned[fi].Copy();
                dtTableMeanLine[fi] = dtClonedFive[fi].Copy();
                //for (int j = 0; j < 7; j++)
                //{
                //    dtTableMeanLine[fi].Columns.RemoveAt(7);
                //}

                dtTableMeanLine[fi].Columns.Add("MMA5", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MMA10", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MMA20", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MMA30", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MMA60", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MMA120", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MMA250", typeof(double));
                dtTableMeanLine[fi].Columns.Add(" ");//分割线
                dtTableMeanLine[fi].Columns.Add("MA5", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MA10", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MA20", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MA30", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MA60", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MA120", typeof(double));
                dtTableMeanLine[fi].Columns.Add("MA250", typeof(double));

                int TotalRows = dtTableMeanLine[fi].Rows.Count;
                for (int i = 0; i < TotalRows; i++)
                {
                    if (i >= 5)
                    {
                        dtTableMeanLine[fi].Rows[i][7] = Math.Round((double)dtTableMeanLine[fi].Rows[i][4] - mma(5, i, dtTableMeanLine[fi]), 2);//ma5
                    }
                    if (i >= 10)
                    {
                        dtTableMeanLine[fi].Rows[i][8] = Math.Round((double)dtTableMeanLine[fi].Rows[i][4] - mma(10, i, dtTableMeanLine[fi]), 2);//ma10
                    }
                    if (i >= 20)
                    {
                        dtTableMeanLine[fi].Rows[i][9] = Math.Round((double)dtTableMeanLine[fi].Rows[i][4] - mma(20, i, dtTableMeanLine[fi]), 2);//ma20
                    }
                    if (i >= 30)
                    {
                        dtTableMeanLine[fi].Rows[i][10] = Math.Round((double)dtTableMeanLine[fi].Rows[i][4] - mma(30, i, dtTableMeanLine[fi]), 2);//ma30
                    }
                    if (i >= 60)
                    {
                        dtTableMeanLine[fi].Rows[i][11] = Math.Round((double)dtTableMeanLine[fi].Rows[i][4] - mma(60, i, dtTableMeanLine[fi]), 2);//ma60
                    }
                    if (i >= 120)
                    {
                        dtTableMeanLine[fi].Rows[i][12] = Math.Round((double)dtTableMeanLine[fi].Rows[i][4] - mma(120, i, dtTableMeanLine[fi]), 2);//ma120
                    }
                    if (i >= 250)
                    {
                        dtTableMeanLine[fi].Rows[i][13] = Math.Round((double)dtTableMeanLine[fi].Rows[i][4] - mma(250, i, dtTableMeanLine[fi]), 2);//ma250
                    }
                    if (i >= 4)
                    {
                        dtTableMeanLine[fi].Rows[i][15] = Math.Round(ma(5, i, dtTableMeanLine[fi]), 2);//mma5
                    }
                    if (i >= 9)
                    {
                        dtTableMeanLine[fi].Rows[i][16] = Math.Round(ma(10, i, dtTableMeanLine[fi]), 2);//mma10
                    }
                    if (i >= 19)
                    {
                        dtTableMeanLine[fi].Rows[i][17] = Math.Round(ma(20, i, dtTableMeanLine[fi]), 2);//mma20
                    }
                    if (i >= 29)
                    {
                        dtTableMeanLine[fi].Rows[i][18] = Math.Round(ma(30, i, dtTableMeanLine[fi]), 2);//mma30
                    }
                    if (i >= 59)
                    {
                        dtTableMeanLine[fi].Rows[i][19] = Math.Round(ma(60, i, dtTableMeanLine[fi]), 2);//mma60
                    }
                    if (i >= 119)
                    {
                        dtTableMeanLine[fi].Rows[i][20] = Math.Round(ma(120, i, dtTableMeanLine[fi]), 2);//mma120
                    }
                    if (i >= 249)
                    {
                        dtTableMeanLine[fi].Rows[i][21] = Math.Round(ma(250, i, dtTableMeanLine[fi]), 2);//mma250
                    }
                }
            }
        }

        void worker_DoWork_WeekK(object sender, DoWorkEventArgs e)
        {
            //for (int fi = 0; fi < newFileNumber; fi++)
            for (int fi = 0; fi < newFileNumberFive; fi++)
            {

                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    isWeekKCalculated = false;
                    isProgressBarCanceled = true;
                    isWeekK = false;
                    if (!isMeanLineCalculated)
                    {
                        isMeanLine = false;
                        isStockDetail = true;
                    }
                    break;
                }

                currentFileNumber = fi;
                //timer_progressBar_Tick1();

                worker.ReportProgress(currentFileNumber);

                dtTableMonth[fi] = new DataTable("dttablemonth");
                dtTableMonth[fi].Columns.Add("col1");
                dtTableMonth[fi].Columns.Add("col2");
                dtTableMonth[fi].Columns.Add("col3");
                dtTableMonth[fi].Columns.Add("col4");
                dtTableMonth[fi].Columns.Add("col5");
                dtTableMonth[fi].Columns[4].DataType = typeof(double);

                int month = 0;
                int year = 0;
                int dayCount = 0;
                double moneyCount = 0;
                //int TotalRows = dtCloned[fi].Rows.Count;
                int TotalRows = dtClonedFive[fi].Rows.Count;
                for (int i = 0; i < TotalRows; i++)
                {
                    //int y = ((DateTime)dtCloned[fi].Rows[i][0]).Year;
                    int y = ((DateTime)dtClonedFive[fi].Rows[i][0]).Year;
                    int m = ((DateTime)dtClonedFive[fi].Rows[i][0]).Month;
                    //int m = ((DateTime)dtCloned[fi].Rows[i][0]).Month;
                    if (i == 0)
                    {
                        year = y;
                        month = m;
                    }
                    if (year == y)// the same year
                    {
                        if (month == m)// the same month
                        {
                            //moneyCount += (double)dtCloned[fi].Rows[i][6];
                            moneyCount += (double)dtClonedFive[fi].Rows[i][6];
                            dayCount++;
                        }
                        else
                        {
                            DataRow row = dtTableMonth[fi].NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = dayCount;
                            row[3] = Math.Round(moneyCount / 100000000, 2);
                            row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                            dtTableMonth[fi].Rows.Add(row);
                            month = m;
                            dayCount = 1;
                            //moneyCount = (double)dtCloned[fi].Rows[i][6];
                            moneyCount = (double)dtClonedFive[fi].Rows[i][6];
                        }
                    }
                    else
                    {
                        DataRow row = dtTableMonth[fi].NewRow();
                        row[0] = year;
                        row[1] = month;
                        row[2] = dayCount;
                        row[3] = Math.Round(moneyCount / 100000000, 2);
                        row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                        dtTableMonth[fi].Rows.Add(row);

                        dayCount = 1;
                        //moneyCount = (double)dtCloned[fi].Rows[i][6];
                        moneyCount = (double)dtClonedFive[fi].Rows[i][6];
                        year = y;
                        month = m;
                    }

                    if (i == TotalRows - 1)
                    {
                        DataRow row = dtTableMonth[fi].NewRow();
                        row[0] = year;
                        row[1] = month;
                        row[2] = dayCount;
                        row[3] = Math.Round(moneyCount / 100000000, 2);
                        row[4] = Math.Round(moneyCount / dayCount / 100000000, 2);
                        dtTableMonth[fi].Rows.Add(row);
                    }
                }

                // calculate week K
                dtTableWeek[fi] = new DataTable("dttableweek");
                dtTableWeek[fi].Columns.Add("col0");
                dtTableWeek[fi].Columns.Add("col01");
                dtTableWeek[fi].Columns.Add("col1");
                dtTableWeek[fi].Columns.Add("col2");
                dtTableWeek[fi].Columns.Add("col3");
                dtTableWeek[fi].Columns.Add("col4");
                dtTableWeek[fi].Columns.Add("col5");
                dtTableWeek[fi].Columns.Add("col6");
                dtTableWeek[fi].Columns.Add("col7");
                dtTableWeek[fi].Columns.Add("col8");
                dtTableWeek[fi].Columns[9].DataType = typeof(double);

                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;
                int week = 0, day = 0;
                year = 0; month = 0;
                int RowIdxCurrent = 0;
                bool calStand = false;
                double standard = 0; //base value
                double moneyTotal = 0;
                double difTotal = 0;
                for (int i = 0; i < TotalRows; i++)
                {
                    //DateTime dt = (DateTime)dtCloned[fi].Rows[i][0];
                    DateTime dt = (DateTime)dtClonedFive[fi].Rows[i][0];
                    int y = dt.Year;
                    int m = dt.Month;
                    int w = cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                    day = (int)dt.DayOfWeek;

                    //if (i==0)
                    //{
                    //    year = y;
                    //    month = m;
                    //}

                    if (year == y)//the same year
                    {
                        if (month == m)// the same month
                        {
                            if (!calStand)// get the base value of last month
                            {
                                for (int ii = 0; ii < dtTableMonth[fi].Rows.Count; ii++)
                                {
                                    if (year.ToString() == dtTableMonth[fi].Rows[ii][0].ToString() && month.ToString() == dtTableMonth[fi].Rows[ii][1].ToString())
                                    {
                                        if (i == 0)
                                        {
                                            standard = 0;
                                            calStand = true;
                                            break;
                                        }
                                        else
                                        {
                                            standard = Convert.ToDouble(dtTableMonth[fi].Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                            calStand = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (week == w)
                            {
                                //double money = (double)dtCloned[fi].Rows[i][6];
                                double money = (double)dtClonedFive[fi].Rows[i][6];
                                string temp = ((int)money).ToString();//只取整数部分;
                                int len = temp.Length;
                                if (len < 7)
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 9 - len);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                                }
                                else
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                }
                            }
                            else
                            {
                                if (((int)moneyTotal).ToString().Length < 7)
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 9 - ((int)moneyTotal).ToString().Length);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 9 - ((int)moneyTotal).ToString().Length);
                                }
                                else
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                                }
                                moneyTotal = 0;
                                difTotal = 0;
                                RowIdxCurrent += 2;

                                week = w;

                                DataRow dwRow = dtTableWeek[fi].NewRow();
                                dwRow[0] = year;
                                dwRow[1] = month;
                                dwRow[2] = "第" + week.ToString() + "周";
                                dwRow[3] = "成交量";
                                dtTableWeek[fi].Rows.Add(dwRow);

                                DataRow row = dtTableWeek[fi].NewRow();
                                row[0] = year;
                                row[1] = month;
                                row[2] = "第" + week.ToString() + "周";
                                row[3] = "ewv-xpl";
                                dtTableWeek[fi].Rows.Add(row);

                                //double money = (double)dtCloned[fi].Rows[i][6];
                                double money = (double)dtClonedFive[fi].Rows[i][6];
                                string temp = ((int)money).ToString();//只取整数部分;
                                int len = temp.Length;
                                if (len < 7)
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 9 - len);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                                }
                                else
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                    moneyTotal += money;
                                    difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                }
                            }
                        }
                        else//month != m
                        {
                            if (dtTableWeek[fi].Rows.Count > 0)
                            {
                                if (((int)moneyTotal).ToString().Length < 7)
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 9 - ((int)moneyTotal).ToString().Length);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 9 - ((int)moneyTotal).ToString().Length);
                                }
                                else
                                {
                                    dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2);
                                    dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                                }
                                RowIdxCurrent += 2;
                            }

                            month = m;
                            week = w;
                            calStand = false;
                            moneyTotal = 0;
                            difTotal = 0;

                            DataRow dwRow = dtTableWeek[fi].NewRow();
                            dwRow[0] = year;
                            dwRow[1] = month;
                            dwRow[2] = "第" + week.ToString() + "周";
                            dwRow[3] = "成交量";
                            dtTableWeek[fi].Rows.Add(dwRow);

                            DataRow row = dtTableWeek[fi].NewRow();
                            row[0] = year;
                            row[1] = month;
                            row[2] = "第" + week.ToString() + "周";
                            row[3] = "ewv-xpl";
                            dtTableWeek[fi].Rows.Add(row);


                            if (!calStand)// get the base value of last month
                            {
                                for (int ii = 0; ii < dtTableMonth[fi].Rows.Count; ii++)
                                {
                                    if (year == Convert.ToDouble(dtTableMonth[fi].Rows[ii][0].ToString()) && month == Convert.ToDouble(dtTableMonth[fi].Rows[ii][1].ToString()))
                                    {
                                        if (i == 0)
                                        {
                                            standard = 0;
                                            calStand = true;
                                            break;
                                        }
                                        else
                                        {
                                            standard = Convert.ToDouble(dtTableMonth[fi].Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                            calStand = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            //double money = (double)dtCloned[fi].Rows[i][6];
                            double money = (double)dtClonedFive[fi].Rows[i][6];
                            string temp = ((int)money).ToString();//只取整数部分;
                            int len = temp.Length;
                            if (len < 7)
                            {
                                dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 9 - len);
                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                                moneyTotal += money;
                                difTotal += Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                            }
                            else
                            {
                                dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 2);
                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                                moneyTotal += money;
                                difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                            }
                        }
                    }
                    else//year != y
                    {
                        if (i != 0)
                        {
                            if (((int)moneyTotal).ToString().Length < 7)
                            {
                                dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 9 - ((int)moneyTotal).ToString().Length);
                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 9 - ((int)moneyTotal).ToString().Length);
                            }
                            else
                            {
                                dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2);
                                dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                            }
                            RowIdxCurrent += 2;
                        }

                        year = y;
                        month = m;
                        week = w;
                        moneyTotal = 0;
                        difTotal = 0;
                        DataRow dwRow = dtTableWeek[fi].NewRow();
                        dwRow[0] = year;
                        dwRow[1] = month;
                        dwRow[2] = "第" + week.ToString() + "周";
                        dwRow[3] = "成交量";
                        dtTableWeek[fi].Rows.Add(dwRow);

                        DataRow row = dtTableWeek[fi].NewRow();
                        row[0] = year;
                        row[1] = month;
                        row[2] = "第" + week.ToString() + "周";
                        row[3] = "ewv-xpl";
                        dtTableWeek[fi].Rows.Add(row);
                        calStand = false;

                        if (!calStand)// get the base value of last month
                        {
                            for (int ii = 0; ii < dtTableMonth[fi].Rows.Count; ii++)
                            {
                                if (year.ToString() == dtTableMonth[fi].Rows[ii][0].ToString() && month.ToString() == dtTableMonth[fi].Rows[ii][1].ToString())
                                {
                                    if (i == 0)
                                    {
                                        standard = 0;
                                        calStand = true;
                                        break;
                                    }
                                    else
                                    {
                                        standard = Convert.ToDouble(dtTableMonth[fi].Rows[ii - 1][4].ToString());// last month exist in last row in the DataTable
                                        calStand = true;
                                        break;
                                    }
                                }
                            }
                        }

                        //double money = (double)dtCloned[fi].Rows[i][6];
                        double money = (double)dtClonedFive[fi].Rows[i][6];
                        string temp = ((int)money).ToString();//只取整数部分;
                        int len = temp.Length;
                        if (len < 7)
                        {
                            dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 9 - len);
                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                            moneyTotal += money;
                            difTotal += Math.Round(Math.Round(money / 100000000, 9 - len) - standard, 9 - len);
                        }
                        else
                        {
                            dtTableWeek[fi].Rows[RowIdxCurrent][day + 3] = Math.Round(money / 100000000, 2);
                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][day + 3] = Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                            moneyTotal += money;
                            difTotal += Math.Round(Math.Round(money / 100000000, 2) - standard, 2);
                        }

                    }

                    if (i == TotalRows - 1)
                    {
                        string temp = ((int)moneyTotal).ToString();
                        int len = temp.Length;
                        if (len < 7)
                        {
                            dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 9 - len);
                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 9 - len);
                        }
                        else
                        {
                            dtTableWeek[fi].Rows[RowIdxCurrent][9] = Math.Round(moneyTotal / 100000000, 2);
                            dtTableWeek[fi].Rows[RowIdxCurrent + 1][9] = Math.Round(difTotal, 2);
                        }
                    }
                }
            }
        }

        private void Open_zhuanhuan_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open Txt files  转换宝六十分钟数据文本
            this.openFileDialog.Multiselect = true; //Enable select multiple files
            this.openFileDialog.Filter = "Txt Files|*.txt";

            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string FileName = this.openFileDialog.FileName;
                fileNumber = openFileDialog.FileNames.Length;


                dataGridView_all.Visible = false;
                AMinusBClicked = false;
                CloseMinusBClicked = false;

                fileOpened = false;
                isWeekKCalculated = false;
                isMeanLineCalculated = false;
                isMeanLine = false;
                isWeekK = false;

                isProgressBarCanceled = false;
                workerPB = new ProgressBar();
                workerPB.progressBar_MeanLine.Value = 0;
                workerPB.progressBar_MeanLine.Maximum = fileNumber;
                workerPB.progressBar_MeanLine.Minimum = 0;
                currentFileNumber = 0;

                worker = new BackgroundWorker();
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork_OpenFile_zhuanhuanbao);
                worker.RunWorkerAsync();
                workerPB.Text = "正在打开文件 ……";
                workerPB.StartPosition = FormStartPosition.CenterParent;
                workerPB.ShowDialog();


                // the open file operation is in worker_DoWork();

                if (!isProgressBarCanceled)
                {
                    dataGridView_all.Visible = true;
                    dataGridView_all.DataSource = dtTablesAllFiles;
                    fileOpened = true;
                    isStockDetail = true;
                    isMeanLine = false;
                    ShowRowNumber(dataGridView_all);
                }
            }
        }

        // For backgroundworker
        void worker_DoWork_OpenFile_zhuanhuanbao(object sender, DoWorkEventArgs e)
        {
            // open file and show progress bar

            DataTable[] dtClonedTemp = new DataTable[fileNumber];
            //StockInfo = new string[fileNumber][];//Stock number and name of each file
            string[][] StockInfoTemp = new string[fileNumber][];
            //dtCloned = new DataTable[fileNumber];
            TotalRows = new int[fileNumber];

            escapeFive = new int[fileNumber];
            Array.Clear(escapeFive, 0, fileNumber);

            isProgressBarCanceled = false;
            for (int fi = 0; fi < fileNumber; fi++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    isProgressBarCanceled = true;
                    break;
                }

                currentFileNumber = fi;
                //timer_progressBar_Tick1();
                worker.ReportProgress(currentFileNumber);

                try
                {
                    FileName = openFileDialog.FileNames[fi].ToString();

                    var filestream = new System.IO.FileStream(FileName,
                                      System.IO.FileMode.Open,
                                      System.IO.FileAccess.Read,
                                      System.IO.FileShare.ReadWrite);
                    var file = new System.IO.StreamReader(filestream, System.Text.Encoding.Default, true, 128);
                    string line;
                    int LineNumber = 0;
                    dtClonedTemp[fi] = new DataTable("temp");
                    while ((line = file.ReadLine()) != null)
                    {
                        //string[] data = line.Split(null);

                        if (LineNumber == 0)// the first line
                        {
                            StockInfoTemp[fi] = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                            LineNumber++;
                            continue;
                        }

                        string[] data = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                        if (LineNumber == 1)// the second line
                        {
                            for (int c = 0; c < data.Length; c++)
                            {
                                dtClonedTemp[fi].Columns.Add(data[c], typeof(double));
                            }
                            dtClonedTemp[fi].Columns[0].DataType = typeof(DateTime);
                            LineNumber++;
                            continue;
                        }

                        if (data.Length < 8)// reach the file end
                            break;

                        DataRow row = dtClonedTemp[fi].NewRow();
                        DateTime te = Convert.ToDateTime(data[0]);
                        //DateTime hours =  Convert.ToDateTime(data[1]);
                        //te.AddHours((double)hours.Hour);
                        //te.AddMinutes((double)hours.Minute);
                        char[] separatingChars = {':'};
                        string[] hours = data[1].Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                        DateTime new_te = te.AddHours(Convert.ToDouble(hours[0]));
                        DateTime new_new_te = new_te.AddMinutes(Convert.ToDouble(hours[1]));
                        
                        for (int c = 0; c < data.Length; c++)
                        {
                            if (c == 0)
                            {
                                row[c] = new_new_te;
                                c++;
                            }
                            else
                            {
                                row[c-1] = data[c];
                            }
                        }
                        dtClonedTemp[fi].Rows.Add(row);
                        LineNumber++;
                    }

                    LineNumber -= 2;
                    //if (LineNumber <= 75)
                    //{
                    //    escape[fi] = 1;
                    //    ///continue;
                    //}
                    if (LineNumber <= 5)
                    {
                        escapeFive[fi] = 1;
                        continue;
                    }

                    TotalRows[fi] = LineNumber;
                }
                catch
                {
                    MessageBox.Show("错误！");
                    return;
                }
            }

            if (!isProgressBarCanceled)
            {
                dtTablesAllFiles = new DataTable("All");
                dtTablesAllFiles.Columns.Add("日期", typeof(DateTime));
                //dtTablesAllFiles.Columns[0].DataType = typeof(DateTime);
                dtTablesAllFiles.Columns.Add("股票代码", typeof(string));
                dtTablesAllFiles.Columns.Add("股票名称", typeof(string));
                dtTablesAllFiles.Columns.Add("收盘价", typeof(string));
                //dtTablesAllFiles.Columns.Add("A", typeof(double));
                //dtTablesAllFiles.Columns.Add("B", typeof(double));

                ///newFileNumber = fileNumber - escape.Sum();
                newFileNumberFive = fileNumber - escapeFive.Sum();
                ///AMinusBTableIndex = new int[newFileNumber];
                ///CloseMinusBTableIndex = new int[newFileNumber];

                ///dtCloned = new DataTable[newFileNumber];
                dtClonedFive = new DataTable[newFileNumberFive];
                //for (int i = 0; i < newFileNumberFive; i++)
                //{
                //    dtClonedFive[i] = dtClonedTemp[i].Copy();
                //}

                StockInfoFive = new string[newFileNumberFive][];

                int idx = 0;
                for (int i = 0; i < newFileNumberFive; i++)
                {
                    while (escapeFive[idx] > 0)
                    {
                        idx++;// escaped files
                    }

                    dtClonedFive[i] = dtClonedTemp[idx].Copy();
                    StockInfoFive[i] = StockInfoTemp[idx];
                    idx++;
                }

                // 清除变量
                dtClonedTemp = null;
                StockInfoTemp = null;

                for (int i = 0; i < newFileNumberFive; i++)
                //for (int i = 0; i < newFileNumber; i++)
                {
                    DataRow row = dtTablesAllFiles.NewRow();
                    //row[0] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][0];//date
                    row[0] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][0];//date
                    //row[1] = StockInfo[i][0];//stock number
                    //row[2] = StockInfo[i][1];//stock name
                    //row[3] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][4];//A
                    //row[4] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][7];//A
                    //row[5] = dtCloned[i].Rows[dtCloned[i].Rows.Count - 1][8];//B

                    row[1] = StockInfoFive[i][0];//stock number
                    row[2] = StockInfoFive[i][1];//stock name
                    row[3] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][4];//A
                    //row[4] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][7];//A
                    //row[5] = dtClonedFive[i].Rows[dtClonedFive[i].Rows.Count - 1][8];//B

                    dtTablesAllFiles.Rows.Add(row);
                }
                dtTableMean = dtTablesAllFiles.Copy();
            }
        }
    }

    public class AbortableBackgroundWorker : BackgroundWorker
    {

        private Thread workerThread;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            workerThread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true; //We must set Cancel property to true!
                Thread.ResetAbort(); //Prevents ThreadAbortException propagation
            }
        }


        public void Abort()
        {
            if (workerThread != null)
            {
                workerThread.Abort();
                workerThread = null;
            }
        }
    }
}
