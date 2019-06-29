using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StockProc
{
    public partial class WeekK : Form
    {
        public int m_iTableIdx; // Selected table index.
        
        public WeekK()
        {
            InitializeComponent();
        }


        private void rowMergeView_weekK_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!isColor)
            {
                for (int ii = 1; ii < rowMergeView_weekK.Rows.Count; ii += 2)
                {
                    for (int jj = 4; jj < 10; jj++)
                    {
                        if (rowMergeView_weekK.Rows[ii].Cells[jj].Value.ToString() != "" && Convert.ToDouble(rowMergeView_weekK.Rows[ii].Cells[jj].Value) > 0)
                        {
                            rowMergeView_weekK.Rows[ii].Cells[jj].Style.BackColor = Color.Red;
                        }
                        else if (rowMergeView_weekK.Rows[ii].Cells[jj].Value.ToString() != "" && Convert.ToDouble(rowMergeView_weekK.Rows[ii].Cells[jj].Value) < 0)
                        {
                            rowMergeView_weekK.Rows[ii].Cells[jj].Style.BackColor = Color.Green;
                        }
                    }
                }
                isColor = true;
            }
        }

        private void rowMergeView_weekK_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            isColor = false;

            foreach (DataGridViewRow row in this.rowMergeView_weekK.Rows)
            {
                this.rowMergeView_weekK.Rows[row.Index].HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void dataSelection_Click(object sender, EventArgs e)
        {
            DateSelectionForm ds = new DateSelectionForm();

            //ds.Show();
            if (ds.ShowDialog() == DialogResult.OK)
            {
                DateTime dateStart = ds.m_dtDateStart;
                DateTime dateEnd = ds.m_dtDateEnd;

                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;

                int yearStart = dateStart.Year;
                int monthStart = dateStart.Month;
                int weekStart = cal.GetWeekOfYear(dateStart, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                int yearEnd = dateEnd.Year;
                int monthEnd = dateEnd.Month;
                int weekEnd = cal.GetWeekOfYear(dateEnd, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                //MessageBox.Show(m_iTableIdx.ToString(), "table index", MessageBoxButtons.YesNo);

                DataTable dtTableWeek = MainForm.dtTableWeek[m_iTableIdx].Copy();

                DataTable dtTableWeekSelectedDate = new DataTable("dttableweekselecteddate");
                dtTableWeekSelectedDate.Columns.Add("col_year");
                dtTableWeekSelectedDate.Columns.Add("col_month");
                dtTableWeekSelectedDate.Columns.Add("col_week");
                dtTableWeekSelectedDate.Columns.Add("col_mon");
                dtTableWeekSelectedDate.Columns.Add("col_tue");
                dtTableWeekSelectedDate.Columns.Add("col_wed");
                dtTableWeekSelectedDate.Columns.Add("col_thur");
                dtTableWeekSelectedDate.Columns.Add("col_fri");
                dtTableWeekSelectedDate.Columns.Add("col_sum");
                dtTableWeekSelectedDate.Columns.Add("col_increasePercent");
                dtTableWeekSelectedDate.Columns[8].DataType = typeof(double);

                //bool dialogShow = false; // Whether show the result dialog.

                int totalRows = dtTableWeek.Rows.Count;
                for (int rowid = 0; rowid < totalRows; rowid++)
                {
                    if (dateStart == dateEnd)   // Only one day
                    {
                        int year = Convert.ToInt32(dtTableWeek.Rows[rowid][0].ToString());
                        int month = Convert.ToInt32(dtTableWeek.Rows[rowid][1].ToString());
                        string result = System.Text.RegularExpressions.Regex.Replace(dtTableWeek.Rows[rowid][2].ToString(), @"[^0-9]+", "");
                        int week = Convert.ToInt32(result);
                        int day = (int)dateStart.DayOfWeek;
                        if (year == yearStart && month == monthStart && week == weekStart)
                        {
                            if (dtTableWeek.Rows[rowid][3].ToString() == "成-基")
                            {
                                string str = dtTableWeek.Rows[rowid][day + 3].ToString();
                                if (str == "" | str == string.Empty)
                                {
                                    MessageBox.Show("当年选择的日期范围无记录！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //dialogShow = false;
                                }
                                else
                                {
                                    DataRow row = dtTableWeekSelectedDate.NewRow();
                                    row[0] = year;
                                    row[1] = month;
                                    row[2] = dtTableWeek.Rows[rowid][2].ToString();
                                    row[day + 2] = str;
                                    //row[8] = Math.Round(Convert.ToDouble(str), 2);
                                    row[8] = Convert.ToDouble(str);
                                    dtTableWeekSelectedDate.Rows.Add(row);

                                    //dialogShow = true;
                                }

                                break; // Only one data.
                            }
                        }
                        
                    }
                    else
                    {
                        int year = Convert.ToInt32(dtTableWeek.Rows[rowid][0].ToString());
                        if (year >= yearStart && year <= yearEnd)
                        {
                            int month = Convert.ToInt32(dtTableWeek.Rows[rowid][1].ToString());

                            if (yearStart == yearEnd)
                            {
                                if (month >= monthStart && month <= monthEnd)
                                {
                                    string result = System.Text.RegularExpressions.Regex.Replace(dtTableWeek.Rows[rowid][2].ToString(), @"[^0-9]+", "");
                                    int week = Convert.ToInt32(result);
                                    if (week >= weekStart && week <= weekEnd)
                                    {
                                        if (dtTableWeek.Rows[rowid][3].ToString() == "成-基")
                                        {
                                            DataRow row = dtTableWeekSelectedDate.NewRow();
                                            row[0] = year;
                                            row[1] = month;
                                            row[2] = dtTableWeek.Rows[rowid][2].ToString();
                                            row[3] = dtTableWeek.Rows[rowid][4].ToString();
                                            row[4] = dtTableWeek.Rows[rowid][5].ToString();
                                            row[5] = dtTableWeek.Rows[rowid][6].ToString();
                                            row[6] = dtTableWeek.Rows[rowid][7].ToString();
                                            row[7] = dtTableWeek.Rows[rowid][8].ToString();
                                            //row[8] = Math.Round(Convert.ToDouble(dtTableWeek.Rows[rowid][9].ToString()), 2);
                                            row[8] = Convert.ToDouble(dtTableWeek.Rows[rowid][9].ToString());
                                            dtTableWeekSelectedDate.Rows.Add(row);
                                            //dialogShow = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (year == yearStart)
                                {
                                    if (month >= monthStart)
                                    {
                                        if (dtTableWeek.Rows[rowid][3].ToString() == "成-基")
                                        {
                                            DataRow row = dtTableWeekSelectedDate.NewRow();
                                            row[0] = year;
                                            row[1] = month;
                                            row[2] = dtTableWeek.Rows[rowid][2].ToString();
                                            row[3] = dtTableWeek.Rows[rowid][4].ToString();
                                            row[4] = dtTableWeek.Rows[rowid][5].ToString();
                                            row[5] = dtTableWeek.Rows[rowid][6].ToString();
                                            row[6] = dtTableWeek.Rows[rowid][7].ToString();
                                            row[7] = dtTableWeek.Rows[rowid][8].ToString();
                                            //row[8] = Math.Round(Convert.ToDouble(dtTableWeek.Rows[rowid][9].ToString()), 2);
                                            row[8] = Convert.ToDouble(dtTableWeek.Rows[rowid][9].ToString());
                                            dtTableWeekSelectedDate.Rows.Add(row);
                                            //dialogShow = true;
                                        }
                                    }
                                }
                                else if (year == yearEnd)
                                {
                                    if (month <= monthEnd)
                                    {
                                        if (dtTableWeek.Rows[rowid][3].ToString() == "成-基")
                                        {
                                            DataRow row = dtTableWeekSelectedDate.NewRow();
                                            row[0] = year;
                                            row[1] = month;
                                            row[2] = dtTableWeek.Rows[rowid][2].ToString();
                                            row[3] = dtTableWeek.Rows[rowid][4].ToString();
                                            row[4] = dtTableWeek.Rows[rowid][5].ToString();
                                            row[5] = dtTableWeek.Rows[rowid][6].ToString();
                                            row[6] = dtTableWeek.Rows[rowid][7].ToString();
                                            row[7] = dtTableWeek.Rows[rowid][8].ToString();
                                            //row[8] = Math.Round(Convert.ToDouble(dtTableWeek.Rows[rowid][9].ToString()), 2);
                                            row[8] = Convert.ToDouble(dtTableWeek.Rows[rowid][9].ToString());
                                            dtTableWeekSelectedDate.Rows.Add(row);
                                            //dialogShow = true;
                                        }
                                    }
                                }
                                //if (month >= monthStart && month <= monthEnd)
                                else
                                {
                                    if (dtTableWeek.Rows[rowid][3].ToString() == "成-基")
                                    {
                                        DataRow row = dtTableWeekSelectedDate.NewRow();
                                        row[0] = year;
                                        row[1] = month;
                                        row[2] = dtTableWeek.Rows[rowid][2].ToString();
                                        row[3] = dtTableWeek.Rows[rowid][4].ToString();
                                        row[4] = dtTableWeek.Rows[rowid][5].ToString();
                                        row[5] = dtTableWeek.Rows[rowid][6].ToString();
                                        row[6] = dtTableWeek.Rows[rowid][7].ToString();
                                        row[7] = dtTableWeek.Rows[rowid][8].ToString();
                                        row[8] = Math.Round(Convert.ToDouble(dtTableWeek.Rows[rowid][9].ToString()), 2);
                                        dtTableWeekSelectedDate.Rows.Add(row);
                                        //dialogShow = true;
                                    }
                                }
                            }
                        }
                    }
                }

                
                if(dtTableWeekSelectedDate.Rows.Count != 0)
                {
                    // Calculate 增长比;
                    for (int rowid = 0; rowid<dtTableWeekSelectedDate.Rows.Count; rowid++)
                    {
                        double lastweek=1.0, thisweek;
                        thisweek = Convert.ToDouble(dtTableWeekSelectedDate.Rows[rowid][8].ToString());
                        
                        // Check if the selected start date is the first line
                        // or the selected start date < the first line
                        if (rowid == 0)
                        {
                            //string firstline = MainForm.dtCloned[m_iTableIdx].Rows[0][0].ToString();
                            string firstline = MainForm.dtClonedFive[m_iTableIdx].Rows[0][0].ToString();
                            DateTime date = DateTime.Parse(firstline);

                            if (date >= dateStart)
                            {
                                lastweek = thisweek;
                            }
                        }
                        else
                        {
                            lastweek = Convert.ToDouble(dtTableWeekSelectedDate.Rows[rowid-1][8].ToString());
                        }

                        if (thisweek >= lastweek)
                        {
                            dtTableWeekSelectedDate.Rows[rowid][9] = Math.Abs((thisweek - lastweek) / lastweek).ToString("0.00%");
                        }
                        else
                        {
                            dtTableWeekSelectedDate.Rows[rowid][9] = ((thisweek - lastweek) / lastweek).ToString("0.00%");
                        }
                    }

                    // Show data
                    DateSelectedWeekK dsWeekK = new DateSelectedWeekK();
                    dsWeekK.Text = "从 " + dateStart.ToString("yyyy/MM/dd") + " 到 " + dateEnd.ToString("yyyy/MM/dd") + " 的周K基准量";
                    dsWeekK.rowMergeView_dateSelected.DataSource = dtTableWeekSelectedDate;
                    dsWeekK.rowMergeView_dateSelected.ColumnHeadersHeight = 40;
                    dsWeekK.rowMergeView_dateSelected.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    dsWeekK.rowMergeView_dateSelected.MergeColumnNames.Add("year");
                    dsWeekK.rowMergeView_dateSelected.MergeColumnNames.Add("month");
                    dsWeekK.rowMergeView_dateSelected.MergeColumnNames.Add("week");
                    //dsWeekK.StartPosition = FormStartPosition.Manual;
                    //dsWeekK.Location = new Point(500, 10);
                    dsWeekK.rowMergeView_dateSelected.Sort(dsWeekK.rowMergeView_dateSelected.Columns[8], ListSortDirection.Ascending);
                    dsWeekK.Show();
                    MainForm.ShowRowNumber(dsWeekK.rowMergeView_dateSelected);
                }
                else
                {
                    MessageBox.Show("当年选择的日期范围无记录！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        //Not used
        //增长比;
        private void menuIncreasePercent_Click(object sender, EventArgs e)
        {
            DateSelectionForm ds = new DateSelectionForm();

            //ds.Show();
            if (ds.ShowDialog() == DialogResult.OK)
            {
                DateTime dateStart = ds.m_dtDateStart;
                DateTime dateEnd = ds.m_dtDateEnd;

                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;

                int yearStart = dateStart.Year;
                int monthStart = dateStart.Month;
                int weekStart = cal.GetWeekOfYear(dateStart, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                int yearEnd = dateEnd.Year;
                int monthEnd = dateEnd.Month;
                int weekEnd = cal.GetWeekOfYear(dateEnd, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }
        }
    }
}
