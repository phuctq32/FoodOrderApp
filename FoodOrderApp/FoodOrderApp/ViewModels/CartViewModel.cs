using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand DeleteCartCommand { get; set; }

        public CartViewModel()
        {
            LoadedCommand = new RelayCommand<CartUC>(p => true, p => DisplayCart(p));
            DeleteCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => DeleteCart(parameter));
        }

        private void DisplayCart(CartUC parameter)
        {
            List<PRODUCT> loadedListCart = new List<PRODUCT>() ;
            List<CART> currentUserCarts = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
            foreach (var item in currentUserCarts)
            {
                PRODUCT product = Data.Ins.DB.PRODUCTs.Where(p => p.ID_ == item.PRODUCT_).SingleOrDefault();
                loadedListCart.Add(product);
            }
            parameter.cartList.ItemsSource = loadedListCart;
        }
        private void DeleteCart(ListViewItem parameter)
        {
            try
            {
                PRODUCT item = parameter.DataContext as PRODUCT;
                CART del = Data.Ins.DB.CARTs.Where(x => x.PRODUCT_ == item.ID_ && x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
                Data.Ins.DB.CARTs.Remove(del);
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Xóa thành công");

            }
            catch
            {
                CustomMessageBox.Show("Lỗi xóa cart");
            }
        }
    }
}
