namespace QuanLyQuanCafeEF
{
    using Core.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class frmAdmin : Form
    {
        const int billByPage = 5;
        public frmAdmin()
        {
            InitializeComponent();

            LoadAll();
        }

        #region Method
        void LoadAll()
        {
            LoadDateTimePickerBill();
            LoadListViewByDate(dtpFromDate.Value, dtpToDate.Value, txtPage.Text, billByPage);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListViewByDate(DateTime fromDate, DateTime toDate, string strSkip, int take)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            int skip = Convert.ToInt32(strSkip) * billByPage - billByPage;

            dgvBill.DataSource = dbcontext.Bills.Join(dbcontext.TableFoods, b => b.IdTable, tb => tb.Id, (b, tb) => new
            {
                TableName = tb.Name,
                TotalPrice = b.TotalPrice,
                CheckIn = b.DateCheckIn,
                CheckOut = b.DateCheckOut,
                Discount = b.Discount
            }).Where(x => fromDate <= x.CheckIn && toDate >= x.CheckOut).Skip(skip).Take(take).ToList();
        }
        #endregion

        #region Events

        #endregion

        private void btnFirstViewPage_Click(object sender, EventArgs e)
        {
            txtPage.Text = "1";
        }

        private void btnLastVewPage_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            int count = dbcontext.Bills.Where(x => dtpFromDate.Value <= x.DateCheckIn && dtpToDate.Value >= x.DateCheckOut).Count();

            int lastPage = count / billByPage;

            if (count % billByPage > 0)
            {
                lastPage++;
            }

            txtPage.Text = lastPage.ToString();
        }

        private void btnPrevioursVewPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPage.Text);

            if (page > 1)
                page--;

            txtPage.Text = page.ToString();
        }

        private void btnNextViewPage_Click(object sender, EventArgs e)
        {

            using var dbcontext = new QuanLyQuanCafeContext();

            int page = Convert.ToInt32(txtPage.Text);

            int count = dbcontext.Bills.Where(x => dtpFromDate.Value <= x.DateCheckIn && dtpToDate.Value >= x.DateCheckOut).Count();

            int lastPage = count / billByPage;

            if (count % billByPage > 0)
            {
                lastPage++;
            }

            if (page < lastPage)
            {
                page++;
            }

            txtPage.Text = page.ToString();
        }

        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            LoadListViewByDate(dtpFromDate.Value, dtpToDate.Value, txtPage.Text, billByPage);
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            txtPage.Text = "1";
            LoadListViewByDate(dtpFromDate.Value, dtpToDate.Value, txtPage.Text, billByPage);
        }
    }
}
