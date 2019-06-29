using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace StockProc
{
    public partial class DateSelectionForm : Form
    {
        public DateTime m_dtDateStart;
        public DateTime m_dtDateEnd;


        public DateSelectionForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        
        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_dtDateStart = monthCalendarStart.SelectionStart;
            m_dtDateEnd = monthCalendarEnd.SelectionStart;

            //MessageBox.Show(m_dtDateStart.ToString(), "start time", MessageBoxButtons.YesNo);
            //MessageBox.Show(m_dtDateEnd.ToString(), "end time", MessageBoxButtons.YesNo);
            //DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            //Calendar cal = dfi.Calendar;
            //int weekNum = cal.GetWeekOfYear(m_dtDateStart, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            //MessageBox.Show(weekNum.ToString(), "end time", MessageBoxButtons.YesNo);
            this.Close();
        }
    }
}
