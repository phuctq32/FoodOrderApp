using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MenuViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public List<PRODUCT> products;

        public MenuViewModel()
        {
            AddToCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => AddToCart(parameter));
            LoadedCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => Load(parameter));
            
        }

        private void Load(MenuUC parameter)
        {
            products = Data.Ins.DB.PRODUCTs.ToList();

            parameter.ViewListProducts.ItemsSource = products;
        }

        private void AddToCart(ListViewItem parameter)
        {
            try
            {
                var item = parameter.DataContext as PRODUCT;
                int cartsCount = Data.Ins.DB.CARTs.Where(x => x.USERNAME_ == CurrentAccount.Username && x.PRODUCT_ == item.ID_).Count();
                int idCarts = Data.Ins.DB.CARTs.Count();
                if (cartsCount == 0)
                {
                    Data.Ins.DB.CARTs.Add(new CART() { ID_ = item.ID_, PRODUCT_ = item.ID_, USERNAME_ = CurrentAccount.Username, AMOUNT_ = 1 });
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Đã thêm " + item.NAME_ + " vào giỏ hàng thành công", MessageBoxButton.OK);
                }
                else
                    CustomMessageBox.Show("Món ăn " + item.NAME_ + " đã có sẵn trong giỏ hàng", MessageBoxButton.OK);
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu");
            }
        }
    }
}