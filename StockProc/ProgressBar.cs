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
    public partial class ProgressBar : Form
    {
        public ProgressBar()
        {
            InitializeComponent();
        }

        public int ProgressValue
        {
            get { return this.progressBar_MeanLine.Value; }
            set { this.progressBar_MeanLine.Value = value; }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            MainForm.worker.CancelAsync();
        }
    }
}
