namespace QuanLyQuanCafeEF
{
    using Core.Models;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Globalization;

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

            LoadTable();
            LoadCategory();

            LoadComboboxTable(cbSwitchTable);
        }

        #region Method
        void ChangeAccount(int type)
        {
            tsmiAdmin.Enabled = type == 1;
            toolStripMenuItem2.Text += $" ({LoginAccount.DisplayName})";
        }

        void LoadListFoodByCategooryID(int id)
        {
            var dbcontext = new QuanLyQuanCafeContext();
            List<Food> listFood = dbcontext.Foods.Where(x => id.Equals(x.IdCategory)).ToList(); ;
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            var dbcontext = new QuanLyQuanCafeContext();
            fpnTable.Controls.Clear();
            List<TableFood> tableList = dbcontext.TableFoods.Select(x => x).ToList();
            foreach (TableFood item in tableList)
            {
                Button btn = new Button() { Width = 65, Height = 65 };

                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += Btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                fpnTable.Controls.Add(btn);
            }
        }

        void LoadCategory()
        {
            var dbcontext = new QuanLyQuanCafeContext();
            List<FoodCategory> listCategory = dbcontext.FoodCategories.Select(x => x).ToList();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        #endregion

        #region Events
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as TableFood).Id;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();

            var dbcontext = new QuanLyQuanCafeContext();

            var lst = dbcontext.Bills
                .Where(x => id.Equals(x.IdTable) && x.Status == 0)
                .Join(dbcontext.BillInfos, b => b.Id, bi => bi.IdBill, (b, bi) => new { b, bi })
                .Join(dbcontext.Foods, bbi => bbi.bi.IdFood, f => f.Id, (bbi, f) => new
                {
                    FoodName = f.Name,
                    Count = bbi.bi.Count,
                    Price = f.Price,
                    TotalPrice = f.Price * bbi.bi.Count
                }).ToList();

            float totalPrice = 0;

            foreach (var item in lst)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += (float)item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");

            //Thread.CurrentThread.CurrentCulture = culture;

            txtTotalPrice.Text = totalPrice.ToString("c", culture);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tsmiAdmin")
            {
                frmAdmin f = new frmAdmin();
                f.ShowDialog();
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tsmiAccountProfile")
            {
                frmAccountProfile f = new frmAccountProfile(loginAccount);
                f.UpdateAccountEvent += f_UpdateAccountEvent;
                f.ShowDialog();
            }

            if (e.ClickedItem.Name == "tsmiExit")
            {
                this.Close();
            }
        }

        private void f_UpdateAccountEvent(object sender, frmAccountProfile.AccountEvent e)
        {
            toolStripMenuItem2.Text = $"Thông tin tài khoản ({e.Acc.DisplayName})";
        }
        #endregion

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            FoodCategory selected = cb.SelectedItem as FoodCategory;
            id = selected.Id;

            LoadListFoodByCategooryID(id);
        }

        private void AddFood_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            TableFood table = lsvBill.Tag as TableFood;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            Bill bill = dbcontext.Bills.Where(x => table.Id.Equals(x.IdTable) && x.Status == 0).Select(x => x).SingleOrDefault();

            int idFood = (cbFood.SelectedItem as Food).Id;
            int count = (int)nudFoodCount.Value;

            if (bill == null)
            {
                var newBill = new Bill();

                newBill.DateCheckIn = DateTime.Now;
                newBill.DateCheckOut = null;
                newBill.IdTable = table.Id;
                newBill.Status = 0;
                newBill.Discount = 0;

                dbcontext.Add(newBill);

                dbcontext.SaveChanges();

                InsertBillInfo(newBill.Id, idFood, count);
            }
            else
            {
                InsertBillInfo(bill.Id, idFood, count);
            }

            ShowBill(table.Id);

            LoadTable();
        }

        void InsertBillInfo(int idBill, int idFood, int count)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            BillInfo newBillinfo = dbcontext.BillInfos
                    .Where(x => idBill.Equals(x.IdBill) && idFood.Equals(x.IdFood))
                    .Select(x => x).SingleOrDefault();

            if (newBillinfo != null)
            {
                if (newBillinfo.Count + count > 0)
                {
                    newBillinfo.Count = newBillinfo.Count + count;
                }
                else
                {
                    dbcontext.Remove(newBillinfo);
                }
            }
            else
            {
                if (count < 0)
                {
                    return;
                }

                BillInfo bi = new BillInfo();
                bi.IdBill = idBill;
                bi.IdFood = idFood;
                bi.Count = count;

                dbcontext.Add(bi);
            }

            dbcontext.SaveChanges();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            TableFood table = lsvBill.Tag as TableFood;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            Bill bill = dbcontext.Bills.Where(x => table.Id.Equals(x.IdTable) && x.Status == 0).Select(x => x).SingleOrDefault();
            int discount = (int)nudDiscount.Value;

            CultureInfo culture = new CultureInfo("vi");

            double totalPrice = double.Parse(txtTotalPrice.Text.Split(new char[] { ' ', ',' })[0], culture);
            double fnTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (bill != null)
            {
                if (MessageBox.Show($"Bạn có chắc thanh toán hóa đơn cho {table.Name} \n Tổng tiền - (Tổng tiền / 100) x Giảm giá \n => {totalPrice} - ({totalPrice} / 100) x {discount} = {fnTotalPrice}", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    bill.DateCheckOut = DateTime.Now;
                    bill.Status = 1;
                    bill.Discount = discount;
                    bill.TotalPrice = fnTotalPrice;

                    dbcontext.SaveChanges();

                    ShowBill(table.Id);

                    LoadTable();
                }
            }
        }

        void LoadComboboxTable(ComboBox cb)
        {
            using var dbcontext = new QuanLyQuanCafeContext();
            cb.DataSource = dbcontext.TableFoods.Select(x => x).ToList();
            cb.DisplayMember = "Name";
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            int id1 = (lsvBill.Tag as TableFood).Id;
            int id2 = (cbSwitchTable.SelectedItem as TableFood).Id;

            TableFood table1 = dbcontext.TableFoods.Where(x => id1.Equals(x.Id)).SingleOrDefault();
            TableFood table2 = dbcontext.TableFoods.Where(x => id2.Equals(x.Id)).SingleOrDefault();

            Bill bill1 = dbcontext.Bills.Where(x => id1.Equals(x.IdTable) && x.Status == 0).SingleOrDefault();
            Bill bill2 = dbcontext.Bills.Where(x => id2.Equals(x.IdTable) && x.Status == 0).SingleOrDefault();

            if (bill1 == null && bill2 == null)
            {
                return;
            }

            if (MessageBox.Show($"Bạn có thật sự muốn chuyển bàn {(lsvBill.Tag as TableFood).Name} quan bàn {(cbSwitchTable.SelectedItem as TableFood).Name} ?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (bill1 == null )
                {
                    bill2.IdTable = id1;
                    dbcontext.TableFoods.Where(x => id1.Equals(x.Id)).SingleOrDefault().Status = "Có người";
                    dbcontext.TableFoods.Where(x => id2.Equals(x.Id)).SingleOrDefault().Status = "Trống";
                }
                else if (bill2 == null)
                {
                    bill1.IdTable = id2;
                    dbcontext.TableFoods.Where(x => id2.Equals(x.Id)).SingleOrDefault().Status = "Có người";
                    dbcontext.TableFoods.Where(x => id1.Equals(x.Id)).SingleOrDefault().Status = "Trống";
                }
                else
                {
                    bill1.IdTable = id2;
                    bill2.IdTable = id1;
                }

                dbcontext.SaveChanges();

                LoadTable();
                ShowBill(id1);
            }
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tsmiAddFood")
            {
                AddFood_Click(this, new EventArgs());
            }
            if (e.ClickedItem.Name == "tsmiCheckOut")
            {
                btnCheckOut_Click(this, new EventArgs());
            }
        }
    }
}
