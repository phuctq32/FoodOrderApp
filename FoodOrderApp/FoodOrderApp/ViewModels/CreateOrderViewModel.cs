using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class CreateOrderViewModel : BaseViewModel
    {
        private string search = "";
        private List<PRODUCT> products;
        private long totalPrice;
        private List<CART> currentCart;
        public string Search
        { get => search; set { search = value; OnPropertyChanged("Search"); } }
        public List<PRODUCT> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged("Products");
            }
        }
        public long TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public List<CART> CurrentCart
        {
            get => currentCart;
            set
            {
                currentCart = value;
                OnPropertyChanged("CurrentCart");
            }
        }
        public string FullName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public ICommand OpenSetUserInformationWindowCommand { get; set; }
        public ICommand SetInformationCommand { get; set; }
        public ICommand CreateOrderCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand HoverItemCommand { get; set; }
        public ICommand CancelHoverItemCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand DeleteCartCommand { get; set; }
        public ICommand OrderCommand { get; set; }

        public CreateOrderViewModel()
        {
            Products = Data.Ins.DB.PRODUCTs.ToList();
            OpenSetUserInformationWindowCommand = new RelayCommand<OrderManagementUC>((parameter) => parameter == null ? false : true, (parameter) => OpenSetUserInformationWindow(parameter));
            SetInformationCommand = new RelayCommand<SetUserInformationWindow>((parameter) => parameter == null ? false : true, (parameter) => SetInformation(parameter));
            CreateOrderCommand = new RelayCommand<OrderManagementUC>(p => p == null ? false : true, (p) => CreateOrder(p));
            LoadedCommand = new RelayCommand<CreateOrderWindow>((parameter) => true, (parameter) => Load(parameter));
            SearchCommand = new RelayCommand<CreateOrderWindow>((parameter) => parameter == null ? false : true, (parameter) => SearchFunction(parameter));
            FilterCommand = new RelayCommand<ComboBox>((parameter) => true, (parameter) => Filter(parameter));
            HoverItemCommand = new RelayCommand<Button>((paramter) => paramter == null ? false : true, (parameter) => HoverItem(parameter));
            CancelHoverItemCommand = new RelayCommand<Button>((paramter) => paramter == null ? false : true, (parameter) => CancelHoverItem(parameter));
            AddToCartCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => AddToCart(p));
            DownCommand = new RelayCommand<TextBlock>(p => true, p => Down(p));
            UpCommand = new RelayCommand<TextBlock>(p => true, p => Up(p));
            DeleteCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => DeleteCart(parameter));
            OrderCommand = new RelayCommand<CreateOrderWindow>((parameter) => parameter == null ? false : true, (parameter) => Order(parameter));
        }
        private void Load(CreateOrderWindow parameter)
        {
            Products = Data.Ins.DB.PRODUCTs.ToList();
            CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
            TotalPrice = 0;
        }
        private void OpenSetUserInformationWindow(OrderManagementUC parameter)
        {
            SetUserInformationWindow setUserInformationWindow = new SetUserInformationWindow();
            setUserInformationWindow.ShowDialog();
        }
        private void SetInformation(SetUserInformationWindow parameter)
        {
            if (string.IsNullOrEmpty(parameter.txtFullname.Text))
            {
                parameter.txtFullname.Focus();
                CustomMessageBox.Show("Tài khoản đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtFullname.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(parameter.txtPhone.Text))
            {
                parameter.txtPhone.Focus();
                CustomMessageBox.Show("Số điện thoại đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPhone.Text = "";
                return;
            }
            if(!parameter.txtPhone.Text.StartsWith("0"))
            {
                parameter.txtPhone.Focus();
                CustomMessageBox.Show("Số điện thoại phải bắt đầu bằng 0!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPhone.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(parameter.txtAddress.Text))
            {
                parameter.txtAddress.Focus();
                CustomMessageBox.Show("Địa chỉ đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtAddress.Text = "";
                return;
            }

            try
            {
                Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().FULLNAME_ = parameter.txtFullname.Text.ToString();
                Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().PHONE_ = parameter.txtPhone.Text.ToString();
                Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().ADDRESS_ = parameter.txtAddress.Text.ToString();
                Data.Ins.DB.SaveChanges();
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            FullName = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().FULLNAME_.ToString();
            PhoneNumber = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().PHONE_.ToString();
            Address = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().ADDRESS_.ToString();

            parameter.Close();
            CreateOrderWindow createOrderWindow = new CreateOrderWindow();
            createOrderWindow.ShowDialog();
            List<CART> tempCarts = Data.Ins.DB.CARTs.Where(x => x.USERNAME_ == CurrentAccount.Username).ToList();
            foreach (var cartToDelete in tempCarts)
            {
                Data.Ins.DB.CARTs.Remove(cartToDelete);
            }
            Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().FULLNAME_ = "Administrator";
            Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().PHONE_ = "0";
            Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().ADDRESS_ = "";
            Data.Ins.DB.SaveChanges();
        }
        private void CreateOrder(OrderManagementUC parameter)
        {
            
        }
        private void SearchFunction(CreateOrderWindow parameter)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource);
            view.Filter = CompareString;
            CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource).Refresh();
        }
        private bool CompareString(object item)
        {
            string a = (item as PRODUCT).NAME_;
            Search = Search.Trim();
            string b = Search;
            a = MenuViewModel.RemoveSign4VietnameseString(a);
            if (b != null)
            {
                b = MenuViewModel.RemoveSign4VietnameseString(b);
            }
            if (string.IsNullOrEmpty(b))
                return true;
            else
                return (a.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void Filter(ComboBox item)
        {
            var createOrderWD = GetAncestorOfType<CreateOrderWindow>(item);
            if (item.SelectedIndex == 0)
            {
                Products = Data.Ins.DB.PRODUCTs.OrderBy(p => p.PRICE_ * (1 - p.DISCOUNT_)).ToList();
            }
            else if (item.SelectedIndex == 1)
            {
                Products = Data.Ins.DB.PRODUCTs.OrderByDescending(p => p.PRICE_ * (1 - p.DISCOUNT_)).ToList();
            }
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(createOrderWD.ViewListProducts.ItemsSource);
            view.Filter = CompareString;
            CollectionViewSource.GetDefaultView(createOrderWD.ViewListProducts.ItemsSource).Refresh();
        }
        private void HoverItem(Button deleteBtn)
        {
            deleteBtn.Visibility = Visibility.Visible;
        }
        private void CancelHoverItem(Button deleteBtn)
        {
            deleteBtn.Visibility = Visibility.Collapsed;
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
        private long GetTotalPrice(ListView listView)
        {
            long res = 0;
            foreach (var lvi in FindVisualChildren<ListViewItem>(listView))
            {
                CART cart = lvi.DataContext as CART;
                res += (long)((Int32)cart.AMOUNT_ * (Int32)cart.PRODUCT.PRICE_ * (1 - (Double)cart.PRODUCT.DISCOUNT_));
            }
            return res;
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
                    string tmpID = CurrentAccount.Username + "_" + item.ID_;
                    Data.Ins.DB.CARTs.Add(new CART() { ID_ = tmpID, PRODUCT_ = item.ID_, USERNAME_ = CurrentAccount.Username, AMOUNT_ = 1 });
                    Data.Ins.DB.SaveChanges();
                    CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
                    long res = 0;
                    foreach (var cart in CurrentCart)
                    {
                        res += (long)((Int32)cart.AMOUNT_ * (Int32)cart.PRODUCT.PRICE_ * (1 - (Double)cart.PRODUCT.DISCOUNT_));
                    }
                    TotalPrice = res;
                    CustomMessageBox.Show("Đã thêm " + item.NAME_.ToString() + " vào giỏ hàng thành công", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                    CustomMessageBox.Show("Món ăn " + item.NAME_.ToString() + " đã có sẵn trong giỏ hàng", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    long res = 0;
                    foreach(var cart in CurrentCart)
                    {
                        res += (long)((Int32)cart.AMOUNT_ * (Int32)cart.PRODUCT.PRICE_ * (1 - (Double)cart.PRODUCT.DISCOUNT_));
                    }
                    TotalPrice = res;
                }
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private CultureInfo viVn = new CultureInfo("vi-VN");

        public void Order(CreateOrderWindow parameter)
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

                foreach (CART cart in CurrentCart)
                {
                    receipt_detail.Add(new RECEIPT_DETAIL()
                    {
                        DETAIL_ID = countReceiptDetail.ToString(),
                        AMOUNT_ = (short)cart.AMOUNT_,
                        RECEIPT_ID = receipt.ID_,
                        PRODUCT_ = cart.PRODUCT_,
                        PRODUCT = cart.PRODUCT,
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
                foreach (var lvi in FindVisualChildren<ListViewItem>(parameter.carts))
                {
                    CART cart = lvi.DataContext as CART;
                    Data.Ins.DB.CARTs.Remove(cart);
                }
                Data.Ins.DB.SaveChanges();
                CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();

                //reset giá trị về mặc định
                TotalPrice = 0;

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
    }
}
