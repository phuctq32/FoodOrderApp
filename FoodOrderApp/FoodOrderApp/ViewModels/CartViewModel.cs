using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        private List<CART> currentCart;
        public List<CART> CurrentCart
        {
            get => currentCart;
            set
            {
                currentCart = value;
                OnPropertyChanged("CurrentCart");
            }
        }
        public CartViewModel()
        {

            LoadedCommand = new RelayCommand<CartUC>(p => p == null ? false : true, p => Loaded(p));
            DeleteCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => DeleteCart(parameter));
            DownCommand = new RelayCommand<TextBlock>(p => true, p => Down(p));
            UpCommand = new RelayCommand<TextBlock>(p => true, p => Up(p));
        }
        private void Loaded(CartUC cartUC)
        {
            CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
        }
        protected void DeleteCart(ListViewItem parameter)
        {
            try
            {
                if (CustomMessageBox.Show("Xóa món ăn khỏi giỏ hàng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    CART cartToDelete = parameter.DataContext as CART;
                    Data.Ins.DB.CARTs.Remove(cartToDelete);
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Xóa thành công", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
                }
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Down(TextBlock parameter)
        {
            short amount = short.Parse(parameter.Text.ToString());
            var lvi = GetAncestorOfType<ListViewItem>(parameter);
            if (amount == 1)
            {
                if (CustomMessageBox.Show("Xóa món ăn khỏi giỏ hàng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    CART cartToDelete = lvi.DataContext as CART;
                    Data.Ins.DB.CARTs.Remove(cartToDelete);
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Xóa thành công", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
                }
            }
            else
            {
                CART cart = lvi.DataContext as CART;
                amount--;
                cart.AMOUNT_ = amount;
                parameter.Text = amount.ToString();
            }
        }
        private void Up(TextBlock parameter)
        {
            short amount = short.Parse(parameter.Text.ToString());
            if (amount == short.MaxValue)
            {
                return;
            }
            else
            {
                var lvi = GetAncestorOfType<ListViewItem>(parameter);
                CART cart = lvi.DataContext as CART;
                amount++;
                cart.AMOUNT_ = amount;
                parameter.Text = amount.ToString();
            }
        }
    }
}
