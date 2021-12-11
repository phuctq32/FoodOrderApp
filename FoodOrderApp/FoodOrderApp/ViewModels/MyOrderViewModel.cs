using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using FoodOrderApp.Views.UserControls.Admin;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MyOrderViewModel : BaseViewModel
    {
        private List<RECEIPT_DETAIL> listReceiptDetail;

        public List<RECEIPT_DETAIL> ListReceiptDetail
        {
            get => listReceiptDetail;
            set
            {
                listReceiptDetail = value;
                OnPropertyChanged("ListReceiptDetail");
            }
        }
        private string fullname;
        public string Fullname
        {
            get => fullname;
            set
            {
                fullname = value;
                OnPropertyChanged("Fullname");
            }
        }
        private string address;
        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }
        private int VALUE;
        public int Value
        {
            get => VALUE;
            set
            {
                VALUE = value;
                OnPropertyChanged("Value");
            }
        }
        private List<RECEIPT> listReceipt;

        public List<RECEIPT> ListReceipt
        {
            get => listReceipt;
            set
            {
                listReceipt = value;
                OnPropertyChanged("Status");
                OnPropertyChanged("ListReceipt");
            }
        }
        public int countItemInReceipt
        {
            get;
            set;
        }
        public int Status;
        public ICommand RatingCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand OpenOrderDetailWindowCommand { get; set; }
        public ICommand OpenInvoiceCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        //private List<RECEIPT> receipts;

        // status = 0 là trạng thái chờ xác nhận
        // status = 1 là trạng thái đang tiến hành
        // status = 2 là trạng thái đã hoàn thành
        // status = 3 là trạng thái đã huỷ
        public MyOrderViewModel()
        {
            RatingCommand = new RelayCommand<RatingBar>(p => true, p => RatingChanged(p));
            LoadedCommand = new RelayCommand<MyOrderUC>(p => p == null ? false : true, p => Load(p));
            SelectionChangedCommand = new RelayCommand<MyOrderUC>((parameter) => { return true; }, (parameter) => SelectionChanged(parameter));
            OpenOrderDetailWindowCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => OpenOrderDetailWindow(p));
            //OpenInvoiceCommand = new RelayCommand<ListViewItem>(p => true, p => openInvoice(p));
            PrintCommand = new RelayCommand<InvoiceWindow>(paramater => true, paramater => print(paramater));
        }
        private void OpenOrderDetailWindow(ListViewItem p)
        {
            //MyOrderUC pa = new MyOrderUC();
            RECEIPT receipt = p.DataContext as RECEIPT;
            OrderDetailWindow orderDetailWindow = new OrderDetailWindow();
            OrderDetailAdminWindow orderDetailAdminWindow = new OrderDetailAdminWindow();
            ListReceiptDetail = Data.Ins.DB.RECEIPT_DETAIL.Where(receiptDetail => receiptDetail.RECEIPT_ID == receipt.ID_).ToList();
            //USER uSER = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == receipt.USERNAME_).SingleOrDefault();
            Fullname = receipt.USER.FULLNAME_;
            Address = receipt.USER.ADDRESS_;
            Phone = receipt.USER.PHONE_;
            Value = receipt.VALUE_;
            string Status = receipt.STATUS_;
            if (Status == "0"|| Status == "1" || Status == "3")
            {
                orderDetailAdminWindow.listReceiptDetail.ItemsSource = listReceiptDetail;
                orderDetailAdminWindow.Address.Visibility = Visibility.Collapsed;
                orderDetailAdminWindow.PhoneNumber.Visibility = Visibility.Collapsed;
                orderDetailAdminWindow.Customer.Visibility = Visibility.Collapsed;
                orderDetailAdminWindow.value.Text = Value.ToString();
                orderDetailAdminWindow.ShowDialog();
            }
            else 
            {
                orderDetailWindow.ListOtherUser.ItemsSource = listReceiptDetail;
                orderDetailWindow.ShowDialog();
            }
            
        }
        private void print(InvoiceWindow paramater)
        {
            PrintDialog printDialog = new PrintDialog();
            try
            {
                if (printDialog.ShowDialog() == true)
                {
                    paramater.printBtn.Visibility = Visibility.Collapsed;
                    paramater.controlBar.Visibility = Visibility.Collapsed;
                    printDialog.PrintVisual(paramater.print, "Invoice");
                }
                paramater.printBtn.Visibility = Visibility.Visible;
                paramater.controlBar.Visibility = Visibility.Visible;
            }
            catch
            {
                paramater.printBtn.Visibility = Visibility.Visible;
                paramater.controlBar.Visibility = Visibility.Visible;
            }
        }
        private void Load(MyOrderUC p)
        {
            ListReceipt = Data.Ins.DB.RECEIPTs.Where(receipt => receipt.STATUS_ == "0" && receipt.USER.USERNAME_ == CurrentAccount.Username).ToList();
            Status = p.statusListViewUser.SelectedIndex;
        }
        private void SelectionChanged(MyOrderUC parameter)
        {
            ListReceipt.Clear();
            Status = parameter.statusListViewUser.SelectedIndex;
            ListReceipt = Data.Ins.DB.RECEIPTs.Where(receipt => receipt.STATUS_ == Status.ToString() && receipt.USER.USERNAME_ == CurrentAccount.Username).ToList();
        }

        private void RatingChanged(RatingBar p)
        {
            //CustomMessageBox.Show(p.Value.ToString());
            //p.IsEnabled = false;
        }
    }
}