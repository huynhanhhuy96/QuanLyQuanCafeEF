namespace QuanLyQuanCafeEF
{
    using System;
    using System.Data;
    using Core.Models;
    using System.Windows.Forms;
    using System.Linq;

    public partial class frmAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get => loginAccount;
            set
            {
                loginAccount = value;
                ChangeAccount(loginAccount);
            }
        }

        public frmAccountProfile(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            txtUserName.Text = loginAccount.UserName;
            txtDisplayName.Text = loginAccount.DisplayName;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateAccount()
        {
            string displayName = txtDisplayName.Text;
            string password = txtPassword.Text;
            string newpass = txtNewPass.Text;
            string reenterpass = txtReEnterPass.Text;
            string userName = txtUserName.Text;

            var dbcontext = new QuanLyQuanCafeContext();

            if (!newpass.Equals(reenterpass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới");
            }
            else
            {
                Account acc = dbcontext.Accounts.Where(x => userName.Equals(x.UserName) && password.Equals(x.PassWord)).SingleOrDefault();

                if (acc != null)
                {
                    if (newpass == "")
                    {
                        acc.DisplayName = displayName;
                    }
                    else
                    {
                        acc.DisplayName = displayName;
                        acc.PassWord = newpass;
                    }

                    dbcontext.SaveChanges();

                    MessageBox.Show("Cập nhập thành công");

                    if (updateAccountEvent != null)
                    {
                        updateAccountEvent(this, new AccountEvent(acc));
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đúng mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccountEvent;
        public event EventHandler<AccountEvent> UpdateAccountEvent
        {
            add { updateAccountEvent += value; }
            remove { updateAccountEvent -= value; }
        }

        public class AccountEvent : EventArgs
        {
            private Account acc;

            public Account Acc { get => acc; set => acc = value; }

            public AccountEvent(Account acc)
            {
                this.Acc = acc;
            }
        }
    }
}
