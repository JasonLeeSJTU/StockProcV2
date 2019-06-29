using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StockProc
{
    public partial class LastDay : Form
    {
        public LastDay()
        {
            InitializeComponent();
        }

        private void LastDay_Load(object sender, EventArgs e)
        {
            //dataGridView_LastDay.Columns[0].Width = 80; //date
            //dataGridView_LastDay.Columns[1].Width = 80; //number
            //dataGridView_LastDay.Columns[2].Width = 80; //name
            //dataGridView_LastDay.Columns[3].Width = 100; //成-基
            //dataGridView_LastDay.Columns[9].Width = 90; //mma评分
            //dataGridView_LastDay.Columns[10].Width = 90; //mma个数
            //dataGridView_LastDay.Columns[11].Width = 90; //mma增减
            //dataGridView_LastDay.Columns[12].Width = 90;
            //dataGridView_LastDay.Columns[13].Width = 90;
            //dataGridView_LastDay.Columns[14].Width = 90;
            ShowRowNumber(this.dataGridView_LastDay);
        }

        private void ShowRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                dgv.Rows[row.Index].HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void dataGridView_LastDay_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            isColor = false;
            ShowRowNumber(this.dataGridView_LastDay);
        }

        private void dataGridView_LastDay_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!isColor)
            {
                for (int ii = 0; ii < dataGridView_LastDay.Rows.Count; ii += 1)
                {
                    // 百分数
                    string str = dataGridView_LastDay.Rows[ii].Cells[8].Value.ToString();
                    if (str != "" && Convert.ToDouble(dataGridView_LastDay.Rows[ii].Cells[8].Value) >= 0)
                    {
                        dataGridView_LastDay.Rows[ii].Cells[8].Style.BackColor = Color.Red;
                    }
                    else if (str != "" && Convert.ToDouble(dataGridView_LastDay.Rows[ii].Cells[8].Value) < 0)
                    {
                        dataGridView_LastDay.Rows[ii].Cells[8].Style.BackColor = Color.Green;
                    }

                    // mma评分，判断mma6的值
                    //dataGridView_LastDay.Rows[ii].Selected = true;
                    //int RowIndex = MainForm.m_dtLastDay.Rows.IndexOf(((DataRowView)this.dataGridView_LastDay.CurrentRow.DataBoundItem).Row);
                    //dataGridView_LastDay.Rows[ii].Selected = false;
                    int RowIndex = MainForm.m_dtLastDay.Rows.IndexOf(((DataRowView)this.dataGridView_LastDay.Rows[ii].DataBoundItem).Row);
                    if (MainForm.mma60forPingFen[RowIndex] > 0)
                    {
                        dataGridView_LastDay.Rows[ii].Cells[9].Style.BackColor = Color.Blue;
                    }
                    //else
                    //{
                    //    dataGridView_LastDay.Rows[ii].Cells[9].Style.BackColor = Color.Red;
                    //}

                    str = dataGridView_LastDay.Rows[ii].Cells[30].Value.ToString();
                    if (str != "" && Convert.ToDouble(dataGridView_LastDay.Rows[ii].Cells[30].Value) >= 0.95)
                    {
                        dataGridView_LastDay.Rows[ii].Cells[30].Style.BackColor = Color.Red;
                    }

                    for (int i = 0; i < MainForm.color_for_ma[ii].Length; i++)
                    {
                        str = dataGridView_LastDay.Rows[ii].Cells[35+i].Value.ToString();
                        if (str != "" && MainForm.color_for_ma[ii][i] == true)
                        {
                            dataGridView_LastDay.Rows[ii].Cells[35+i].Style.BackColor = Color.Red;
                        }
                    }
                }
                isColor = true;
            }
        }

        private void dataGridView_LastDay_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = MainForm.m_dtLastDay.Rows.IndexOf(((DataRowView)this.dataGridView_LastDay.CurrentRow.DataBoundItem).Row);

            MonthK mk = new MonthK();
            //mk.Text = MainForm.StockInfo[RowIndex][0] + "  " + MainForm.StockInfo[RowIndex][1];
            mk.Text = MainForm.StockInfoFive[RowIndex][0] + "  " + MainForm.StockInfoFive[RowIndex][1];
            mk.rowMergeView1.DataSource = MainForm.dtTableMonth[RowIndex];
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
            //wk.Text = MainForm.StockInfo[RowIndex][0] + "  " + MainForm.StockInfo[RowIndex][1];
            wk.Text = MainForm.StockInfoFive[RowIndex][0] + "  " + MainForm.StockInfoFive[RowIndex][1];
            wk.rowMergeView_weekK.DataSource = MainForm.dtTableWeek[RowIndex];
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
    }
}
