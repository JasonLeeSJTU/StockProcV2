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
    public partial class DateSelectedWeekK : Form
    {
        public DateSelectedWeekK()
        {
            InitializeComponent();
        }

        private void rowMergeView_dateSelected_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!isColor)
            {
                for (int ii = 0; ii < rowMergeView_dateSelected.Rows.Count; ii += 1)
                {
                    for (int jj = 3; jj < 9; jj++)
                    {
                        if (rowMergeView_dateSelected.Rows[ii].Cells[jj].Value.ToString() != "" && Convert.ToDouble(rowMergeView_dateSelected.Rows[ii].Cells[jj].Value) > 0)
                        {
                            rowMergeView_dateSelected.Rows[ii].Cells[jj].Style.BackColor = Color.Red;
                        }
                        else if (rowMergeView_dateSelected.Rows[ii].Cells[jj].Value.ToString() != "" && Convert.ToDouble(rowMergeView_dateSelected.Rows[ii].Cells[jj].Value) < 0)
                        {
                            rowMergeView_dateSelected.Rows[ii].Cells[jj].Style.BackColor = Color.Green;
                        }
                    }

                    // 百分数;
                    string[] str = rowMergeView_dateSelected.Rows[ii].Cells[9].Value.ToString().Split(new char[] { '%' });
                    if (str[0] != "" && Convert.ToDouble(str[0]) >= 0)
                    {
                        rowMergeView_dateSelected.Rows[ii].Cells[9].Style.BackColor = Color.Red;
                    }
                    else if (str[0] != "" && Convert.ToDouble(str[0]) < 0)
                    {
                        rowMergeView_dateSelected.Rows[ii].Cells[9].Style.BackColor = Color.Green;
                    }
                }
                isColor = true;
            }
        }

        private void rowMergeView_dateSelected_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            isColor = false;
        }
    }
}
