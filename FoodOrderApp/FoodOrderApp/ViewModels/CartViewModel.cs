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
using FoodOrderApp.Views;

namespace FoodOrderApp.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private long totalPrice;
        private string name;
        private string mail;
        private string phone;
        private string address;
        public ICommand LoadedCommand { get; set; }
        public ICommand DeleteCartCommand { get; set; }
        public ICommand DeleteAllCartCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand OrderCommand { get; set; }
        public ICommand OpenSetAddressWDCommand { get; set; }

        //private bool allChecked;
        //public bool AllChecked { get => allChecked; set { allChecked = value; OnPropertyChanged(); } }

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

        // Biến này để gán cho tổng hóa đơn, mỗi lần nhấn checkbox thì sẽ gán lại cho nó = Hàm GetTotalPrice() ở dưới
        public long TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public string Mail
        {
            get => mail;
            set
            {
                mail = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public CartViewModel()
        {
            OpenSetAddressWDCommand = new RelayCommand<CartUC>((parameter) => { return true; }, (parameter) => OpenSetAddress(parameter));
            OrderCommand = new RelayCommand<CartUC>((parameter) => { return true; }, (parameter) => Order(parameter));
            LoadedCommand = new RelayCommand<CartUC>(p => p == null ? false : true, p => Loaded(p));
            DeleteCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => DeleteCart(parameter));
            DeleteAllCartCommand = new RelayCommand<ListView>((parameter) => { return true; }, (parameter) => DeleteAllCart(parameter));
            DownCommand = new RelayCommand<TextBlock>(p => true, p => Down(p));
            UpCommand = new RelayCommand<TextBlock>(p => true, p => Up(p));
            AllCheckedCommand = new RelayCommand<CartUC>((parameter) => { return true; }, (parameter) => AllChecked(parameter));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => { return true; }, (parameter) => Checked(parameter));
            var user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            Name = user.FULLNAME_;
            Phone = user.PHONE_;
            Mail = user.EMAIL_;
            Address = user.ADDRESS_;
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
            var lv = GetAncestorOfType<ListView>(parameter);
            TotalPrice = GetTotalPrice(lv);
        }

        protected void DeleteAllCart(ListView parameter)
        {
            if (parameter.Items.Count == 0)
            {
                return;
            }

            try
            {
                if (CustomMessageBox.Show("Xóa tất cả món ăn khỏi giỏ hàng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    foreach (var cartToDelete in CurrentCart)
                    {
                        Data.Ins.DB.CARTs.Remove(cartToDelete);
                    }
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Xóa thành công", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
                }
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TotalPrice = GetTotalPrice(parameter);
        }

        private void Down(TextBlock parameter)
        {
            short amount = short.Parse(parameter.Text.ToString());
            var lv = GetAncestorOfType<ListView>(parameter);
            var lvi = GetAncestorOfType<ListViewItem>(parameter);
            if (amount == 1)
            {
                try
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
                catch
                {
                    CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                CART cart = lvi.DataContext as CART;
                amount--;
                cart.AMOUNT_ = amount;
                parameter.Text = amount.ToString();
            }

            TotalPrice = GetTotalPrice(lv);
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
                var lv = GetAncestorOfType<ListView>(parameter);
                TotalPrice = GetTotalPrice(lv);
            }
        }

        private void AllChecked(CartUC parameter)
        {
            bool newVal = (parameter.selectAllCheckBox.IsChecked == true);
            //for (int i = 0; i < currentCart.Count; i++)
            //{
            //    //// các checkbox.isCheck = newVal;
            //}
            foreach (var item in FindVisualChildren<CheckBox>(parameter.cartList))
            {
                item.IsChecked = newVal;
            }
            TotalPrice = GetTotalPrice(parameter.cartList);
        }

        private void Checked(CheckBox parameter)
        {
            //// cái thằng allChecked.IsChecked = null
            //if (parameter.IsChecked == true)
            //    CustomMessageBox.Show("true", MessageBoxButton.OK);
            //else
            //    CustomMessageBox.Show("false", MessageBoxButton.OK);
            var lv = GetAncestorOfType<ListView>(parameter);
            TotalPrice = GetTotalPrice(lv);

            // Check xem nếu checked hết thì check cái ô trên cùng
            bool isAllChecked = true;
            var cartUC = GetAncestorOfType<CartUC>(lv);
            foreach (var item in FindVisualChildren<CheckBox>(lv))
            {
                if (item.IsChecked == false)
                {
                    cartUC.selectAllCheckBox.IsChecked = false;
                    isAllChecked = false;
                    break;
                }
            }
            if (isAllChecked)
            {
                cartUC.selectAllCheckBox.IsChecked = true;
            }
        }

        // Tính tổng giá của thằng item có checked = true
        private long GetTotalPrice(ListView listView)
        {
            long res = 0;
            foreach (var lvi in FindVisualChildren<ListViewItem>(listView))
            {
                CART cart = lvi.DataContext as CART;
                var checkBox = GetVisualChild<CheckBox>(lvi);
                if (checkBox.IsChecked == true)
                {
                    res += (long)((Int32)cart.AMOUNT_ * (Int32)cart.PRODUCT.PRICE_ * (1 - (Double)cart.PRODUCT.DISCOUNT_));
                }
            }
            return res;
        }

        public void Order(CartUC parameter)
        {
            CustomMessageBox.Show("Đặt hàng thành công!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public void OpenSetAddress(CartUC parameter)
        {
            ChangeInformationWindow changeInformationWindow = new ChangeInformationWindow();
            changeInformationWindow.nameStack.Visibility = Visibility.Collapsed;
            changeInformationWindow.emailStack.Visibility = Visibility.Collapsed;
            changeInformationWindow.phoneStack.Visibility = Visibility.Collapsed;
            changeInformationWindow.lblChangeinfo.Content = "Cập nhật địa chỉ";
            changeInformationWindow.ShowDialog();
        }
    }
}