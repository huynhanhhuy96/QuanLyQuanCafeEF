namespace QuanLyQuanCafeEF
{
    using Core.Models;
    using System.Windows.Forms;

    public partial class frmTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get => loginAccount;
            set
            {
                loginAccount = value;
                ChangeAccount(loginAccount.Type);
            }
        }
        public frmTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;
        }
        void ChangeAccount(int type)
        {
            toolStripMenuItem1.Enabled = type == 1;
            toolStripMenuItem2.Text += $" ({LoginAccount.DisplayName})";
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Text == "Admin")
            {
                frmAdmin f = new frmAdmin();
                f.ShowDialog();
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Text == "Thông tin cá nhân")
            {
                frmAccountProfile f = new frmAccountProfile(loginAccount);
                f.ShowDialog();
            }
        }
    }
}
