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
    public partial class MonthK : Form
    {
        public MonthK()
        {
            InitializeComponent();
        }

        private void rowMergeView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in this.rowMergeView1.Rows)
            {
                this.rowMergeView1.Rows[row.Index].HeaderCell.Value = (row.Index + 1).ToString();
            }
        }
    }
}
