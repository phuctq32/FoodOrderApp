using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FoodOrderApp.Views;
using System.Globalization;

namespace FoodOrderApp.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private long totalPrice;
        private int foodCount;
        private string name;
        private string mail;
        private string phone;
        private string address;
        public ICommand LoadedCommand { get; set; }
        public ICommand DeleteCartCommand { get; set; }
        public ICommand DeleteIsCheckedCartCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand OrderCommand { get; set; }
        public ICommand OpenSetAddressWDCommand { get; set; }

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

        public int FoodCount
        {
            get => foodCount;
            set
            {
                foodCount = value;
                OnPropertyChanged("FoodCount");
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Mail
        {
            get => mail;
            set
            {
                mail = value;
                OnPropertyChanged("Mail");
            }
        }

        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        public CartViewModel()
        {
            FoodCount = 0;
            OpenSetAddressWDCommand = new RelayCommand<CartUC>((parameter) => { return true; }, (parameter) => OpenSetAddress(parameter));
            OrderCommand = new RelayCommand<CartUC>((parameter) => { return true; }, (parameter) => Order(parameter));
            LoadedCommand = new RelayCommand<CartUC>(p => p == null ? false : true, p => Loaded(p));
            DeleteCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => DeleteCart(parameter));
            DeleteIsCheckedCartCommand = new RelayCommand<ListView>((parameter) => { return true; }, (parameter) => DeleteIsCheckedCart(parameter));
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
                    var lv = GetAncestorOfType<ListView>(parameter);
                    var cartUC = GetAncestorOfType<CartUC>(parameter);
                    if (cartUC.selectAllCheckBox.IsChecked == true)
                    {
                        cartUC.selectAllCheckBox.IsChecked = false;
                    }
                    CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
                    TotalPrice = GetTotalPrice(lv);
                    FoodCount = GetFoodCount(lv);
                }
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void DeleteIsCheckedCart(ListView parameter)
        {
            if (parameter.Items.Count == 0)
            {
                return;
            }

            try
            {
                if (CustomMessageBox.Show("Xóa các món ăn đang được chọn khỏi giỏ hàng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    foreach (var lvi in FindVisualChildren<ListViewItem>(parameter))
                    {
                        CART cart = lvi.DataContext as CART;
                        var checkBox = GetVisualChild<CheckBox>(lvi);
                        if (checkBox.IsChecked == true)
                        {
                            Data.Ins.DB.CARTs.Remove(cart);
                        }
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
            FoodCount = GetFoodCount(parameter);
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
            foreach (var item in FindVisualChildren<CheckBox>(parameter.cartList))
            {
                item.IsChecked = newVal;
            }
            TotalPrice = GetTotalPrice(parameter.cartList);
            FoodCount = GetFoodCount(parameter.cartList);
        }

        private void Checked(CheckBox parameter)
        {
            var lv = GetAncestorOfType<ListView>(parameter);

            TotalPrice = GetTotalPrice(lv);
            FoodCount = GetFoodCount(lv);

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

        private int GetFoodCount(ListView listView)
        {
            int res = 0;
            foreach (var lvi in FindVisualChildren<ListViewItem>(listView))
            {
                CART cart = lvi.DataContext as CART;
                var checkBox = GetVisualChild<CheckBox>(lvi);
                if (checkBox.IsChecked == true)
                {
                    res++;
                }
            }
            return res;
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
                    res += (long)((Int32)cart.AMOUNT_ * (Int32)cart.PRODUCT.PRICE_ * (1 - (decimal)cart.PRODUCT.DISCOUNT_));
                }
            }
            return res;
        }

        private CultureInfo viVn = new CultureInfo("vi-VN");

        public void Order(CartUC parameter)
        {
            try
            {
                DateTime Now = DateTime.Now;
                string now = Now.GetDateTimeFormats(viVn)[0];

                HashSet<RECEIPT_DETAIL> receipt_detail = new HashSet<RECEIPT_DETAIL>();

                int countReceipt = Data.Ins.DB.RECEIPTs.Count() + 1;
                RECEIPT receipt = new RECEIPT();
                receipt.ID_ = countReceipt.ToString();
                receipt.STATUS_ = "0";
                receipt.DATE_ = Now;
                receipt.USERNAME_ = CurrentAccount.Username;
                receipt.VALUE_ = (int)TotalPrice;

                USER user = Data.Ins.DB.USERS.Where(user1 => user1.USERNAME_ == CurrentAccount.Username).Single();
                receipt.USER = user;

                int countReceiptDetail = Data.Ins.DB.RECEIPT_DETAIL.Count() + 1;

                foreach (CART cart in currentCart)
                {
                    receipt_detail.Add(new RECEIPT_DETAIL()
                    {
                        DETAIL_ID = countReceiptDetail.ToString(),
                        AMOUNT_ = (short)cart.AMOUNT_,
                        RECEIPT_ID = receipt.ID_,
                        PRODUCT_ = cart.PRODUCT_,
                        PRODUCT = cart.PRODUCT,
                        RATED_ = true,
                        RATING_ = 0,
                        RECEIPT = receipt
                    });
                    countReceiptDetail++;
                }

                foreach (var receipt_de in receipt_detail)
                {
                    Data.Ins.DB.RECEIPT_DETAIL.Add(receipt_de);
                }
                receipt.RECEIPT_DETAIL = receipt_detail;

                //New này chỉ xóa những cái đang được check thôi
                foreach (var lvi in FindVisualChildren<ListViewItem>(parameter.cartList))
                {
                    CART cart = lvi.DataContext as CART;
                    var checkBox = GetVisualChild<CheckBox>(lvi);
                    if (checkBox.IsChecked == true)
                    {
                        Data.Ins.DB.CARTs.Remove(cart);
                    }
                }
                Data.Ins.DB.SaveChanges();
                CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();

                //reset giá trị về mặc định
                parameter.selectAllCheckBox.IsChecked = false;
                TotalPrice = 0;
                FoodCount = 0;

                CustomMessageBox.Show("Đơn hàng đã được tạo thành công đang chờ xử lí...", MessageBoxButton.OK);
            }
            catch //(DbEntityValidationException e)
            {
                // code để xem lỗi lệnh entity lỗi chỗ nào
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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