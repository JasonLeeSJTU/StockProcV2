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
    public partial class StockDetail : Form
    {
        public StockDetail()
        {
            InitializeComponent();
        }

        private void StockDetail_Load(object sender, EventArgs e)
        {
            //dataGridView_StockDetail.Columns[0].Width = 80;
            ShowRowNumber(this.dataGridView_StockDetail);
        }

        private void ShowRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                dgv.Rows[row.Index].HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void dataGridView_StockDetail_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView_StockDetail.Rows)
            {
                this.dataGridView_StockDetail.Rows[row.Index].HeaderCell.Value = (row.Index + 1).ToString();
            }
        }
    }
}
