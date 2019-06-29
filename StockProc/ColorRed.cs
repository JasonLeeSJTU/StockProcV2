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
    public partial class ColorRed : Form
    {
        public ColorRed()
        {
            InitializeComponent();
        }

        private void colorRedrowMergeView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!isColor)
            {
                for (int ii = 1; ii < colorRedrowMergeView.Rows.Count; ii += 2)
                {
                    for (int jj = 4; jj < 9; jj++)
                    {
                        if (colorRedrowMergeView.Rows[ii].Cells[jj].Value.ToString() != "" && Convert.ToDouble(colorRedrowMergeView.Rows[ii].Cells[jj].Value) > 0)
                        {
                            colorRedrowMergeView.Rows[ii].Cells[jj].Style.BackColor = Color.Red;
                        }
                        else if (colorRedrowMergeView.Rows[ii].Cells[jj].Value.ToString() != "" && Convert.ToDouble(colorRedrowMergeView.Rows[ii].Cells[jj].Value) < 0)
                        {
                            colorRedrowMergeView.Rows[ii].Cells[jj].Style.BackColor = Color.Green;
                        }
                    }
                }
                isColor = true;
            }
        }

        private void colorRedrowMergeView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            isColor = false;

            int idx = 0;
            for (int i = 0; i < colorRedrowMergeView.Rows.Count; i += 2)
            {
                idx++;
                colorRedrowMergeView.Rows[i].HeaderCell.Value = (idx).ToString();
            }
        }

        private void ColorRed_Load(object sender, EventArgs e)
        {
            int idx = 0;
            for (int i = 0; i < colorRedrowMergeView.Rows.Count; i += 2)
            {
                idx++;
                colorRedrowMergeView.Rows[i].HeaderCell.Value = (idx).ToString();
            }
        }
    }
}
