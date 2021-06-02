namespace QuanLyQuanCafeEF
{
    using Core.Models;
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        #region Method
        bool Login(string userName, string password)
        {
            using var dbcontext = new QuanLyQuanCafeContext();
            if (dbcontext.Accounts.Where(x => $"{userName}".Equals(x.UserName) && $"{password}".Equals(x.PassWord)).Select(x => x).SingleOrDefault() != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Events
        private void btnLogin_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            if (Login(userName, password))
            {
                Account loginAccount = dbcontext.Accounts.Where(x => $"{userName}".Equals(x.UserName)).Select(x => x).FirstOrDefault();
                frmTableManager f = new frmTableManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
