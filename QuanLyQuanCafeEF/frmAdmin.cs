﻿namespace QuanLyQuanCafeEF
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
        BindingSource foodList = new BindingSource();

        public frmAdmin()
        {
            InitializeComponent();

            LoadAll();
        }

        #region Method
        void LoadAll()
        {
            dgvFood.DataSource = foodList;

            LoadDateTimePickerBill();
            LoadListViewByDate(dtpFromDate.Value, dtpToDate.Value, txtPage.Text, billByPage);

            LoadListFood();
            AddFoodBinding();
            LoadCategoryIntoCombobox(cbFoodCategory);
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

        void LoadListFood()
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            foodList.DataSource = dbcontext.Foods.ToList();
        }

        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtFoodId.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Id", true, DataSourceUpdateMode.Never));
            nudFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            cb.DataSource = dbcontext.FoodCategories.ToList();
            cb.DisplayMember = "Name";
        }

        List<Food> SearchFoodByName(string name)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            List<Food> listFood = dbcontext.Foods.Where(x => x.Name.Contains(name)).ToList();

            return listFood;
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

        private void txtFoodId_TextChanged(object sender, EventArgs e)
        {
            if (dgvFood.SelectedCells.Count > 0)
            {
                using var dbcontext = new QuanLyQuanCafeContext();

                int? id = (int?)dgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;

                if (id == null)
                {
                    return;
                }

                FoodCategory category = dbcontext.FoodCategories.Where(x => id.Equals(x.Id)).SingleOrDefault();

                cbFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;

                foreach (FoodCategory item in cbFoodCategory.Items)
                {
                    if (item.Id == category.Id)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                cbFoodCategory.SelectedIndex = index;
            }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nudFoodPrice.Value;

            Food newfood = new Food();
            newfood.Name = name;
            newfood.IdCategory = categoryID;
            newfood.Price = price;

            dbcontext.Add(newfood);

            int numberRows = dbcontext.SaveChanges();

            if (numberRows > 0)
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nudFoodPrice.Value;
            int id = Convert.ToInt32(txtFoodId.Text);

            Food food = dbcontext.Foods.Where(x => id.Equals(x.Id)).SingleOrDefault();
            food.Name = name;
            food.IdCategory = categoryID;
            food.Price = price;

            int numberRows = dbcontext.SaveChanges();

            if (numberRows > 0)
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            using var dbcontext = new QuanLyQuanCafeContext();

            int id = Convert.ToInt32(txtFoodId.Text);

            Food food = dbcontext.Foods.Where(x => id.Equals(x.Id)).SingleOrDefault();

            dbcontext.BillInfos.Where(x => id.Equals(x.IdFood)).ToList().ForEach(x => { dbcontext.Remove(x); dbcontext.SaveChanges(); });

            dbcontext.Remove(food);

            int numberRows = dbcontext.SaveChanges();

            if (numberRows > 0)
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txtSearchFoodName.Text);
        }
    }
}
