using FoodOrderApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls.Admin;

namespace FoodOrderApp.ViewModels
{
    public class OrderManagementViewModel : BaseViewModel
    {
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

        private RECEIPT receipt;

        public RECEIPT Receipt
        {
            get => receipt;
            set
            {
                receipt = value;
                OnPropertyChanged("Receipt");
            }
        }

        public int Status { get; set; }

        public int numberOfDistinctProduct { get; set; }

        public ICommand LoadedCommand { get; set; }
        public ICommand OpenOrderDetailWindowCommand { get; set; }
        public ICommand ConfirmReceiptCommand { get; set; }
        public ICommand CancelReceiptCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand DoneReceiptCommand { get; set; }

        public ICommand PrintCommand { get; set; }

        // status = 0 là trạng thái chờ xác nhận
        // status = 1 là trạng thái đang tiến hành
        // status = 2 là trạng thái đã hoàn thành
        // status = 3 là trạng thái đã huỷ
        public OrderManagementViewModel()
        {
            LoadedCommand = new RelayCommand<OrderManagementUC>(p => p == null ? false : true, p => Load(p));
            OpenOrderDetailWindowCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => OpenOrderDetailWindow(p));
            ConfirmReceiptCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => ConfirmReceipt(p));
            CancelReceiptCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => CancelReceipt(parameter));
            SelectionChangedCommand = new RelayCommand<OrderManagementUC>((parameter) => { return true; }, (parameter) => SelectionChanged(parameter));
            DoneReceiptCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => DoneReceipt(p));
            PrintCommand = new RelayCommand<InvoiceWindow>(paramater => true, paramater => print(paramater));
        }

        private void ConfirmReceipt(ListViewItem parameter)
        {
            receipt = parameter.DataContext as RECEIPT;
            ListReceiptDetail = Data.Ins.DB.RECEIPT_DETAIL.Where(x => x.RECEIPT_ID == receipt.ID_).ToList();
            if (CustomMessageBox.Show("Bạn có muốn in hóa đơn?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                InvoiceWindow invoiceWindow = new InvoiceWindow();
                invoiceWindow.listView.ItemsSource = ListReceiptDetail;
                invoiceWindow.receiptValue.Text = receipt.VALUE_.ToString("N0");
                invoiceWindow.ShowDialog();
                List<RECEIPT> listConfirmReceipt = Data.Ins.DB.RECEIPTs.Where(receiptDB => receiptDB.ID_ == receipt.ID_).ToList();
                foreach (var item in listConfirmReceipt)
                {
                    int tmp = Int32.Parse(item.STATUS_);
                    if (tmp <= 1)
                        tmp++;
                    item.STATUS_ = tmp.ToString();
                }
                ListReceipt.Clear();
                Data.Ins.DB.SaveChanges();
                SelectionChanged(GetAncestorOfType<OrderManagementUC>(parameter));
            }
            else
            {
                List<RECEIPT> listConfirmReceipt = Data.Ins.DB.RECEIPTs.Where(receiptDB => receiptDB.ID_ == receipt.ID_).ToList();
                foreach (var item in listConfirmReceipt)
                {
                    int tmp = Int32.Parse(item.STATUS_);
                    if (tmp <= 1)
                        tmp++;
                    item.STATUS_ = tmp.ToString();
                }
                ListReceipt.Clear();
                Data.Ins.DB.SaveChanges();
                SelectionChanged(GetAncestorOfType<OrderManagementUC>(parameter));
            }
        }

        private void CancelReceipt(ListViewItem parameter)
        {
            RECEIPT receipt = parameter.DataContext as RECEIPT;
            List<RECEIPT> listConfirmReceipt = Data.Ins.DB.RECEIPTs.Where(receiptDB => receiptDB.ID_ == receipt.ID_).ToList();
            foreach (var item in listConfirmReceipt)
            {
                int tmp = Int32.Parse(item.STATUS_);
                if (tmp != 3)
                    tmp = 3;
                else
                    tmp = 0;
                item.STATUS_ = tmp.ToString();
            }
            ListReceipt.Clear();
            Data.Ins.DB.SaveChanges();
            SelectionChanged(GetAncestorOfType<OrderManagementUC>(parameter));
        }

        private void OpenOrderDetailWindow(ListViewItem p)
        {
            RECEIPT receipt = p.DataContext as RECEIPT;
            OrderDetailAdminWindow orderDetailAdminWindow = new OrderDetailAdminWindow();
            ListReceiptDetail = Data.Ins.DB.RECEIPT_DETAIL.Where(receiptDetail => receiptDetail.RECEIPT_ID == receipt.ID_).ToList();
            //USER uSER = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == receipt.USERNAME_).SingleOrDefault();
            if (!(receipt.USERNAME_ == "admin"))
            {
                Fullname = receipt.USER.FULLNAME_;
                Address = receipt.USER.ADDRESS_;
                Phone = receipt.USER.PHONE_;
            }
            else
            {
                orderDetailAdminWindow.userInfomation.Visibility = Visibility.Hidden;
                orderDetailAdminWindow.adminInformation.Visibility = Visibility.Visible;
            }
            Value = receipt.VALUE_;
            orderDetailAdminWindow.listReceiptDetail.ItemsSource = listReceiptDetail;
            orderDetailAdminWindow.ShowDialog();
        }

        private void Load(OrderManagementUC p)
        {
            ListReceipt = Data.Ins.DB.RECEIPTs.Where(receipt => receipt.STATUS_ == "0").ToList();
            Status = p.statusListView.SelectedIndex;
        }

        private void SelectionChanged(OrderManagementUC parameter)
        {
            ListReceipt.Clear();
            Status = parameter.statusListView.SelectedIndex;
            ListReceipt = Data.Ins.DB.RECEIPTs.Where(receipt => receipt.STATUS_ == Status.ToString()).ToList();
        }

        private void DoneReceipt(ListViewItem parameter)
        {
            receipt = parameter.DataContext as RECEIPT;
            List<RECEIPT> listConfirmReceipt = Data.Ins.DB.RECEIPTs.Where(receiptDB => receiptDB.ID_ == receipt.ID_).ToList();
            foreach (var item in listConfirmReceipt)
            {
                int tmp = Int32.Parse(item.STATUS_);
                if (tmp < 2)
                    tmp++;
                item.STATUS_ = tmp.ToString();
            }
            ListReceipt.Clear();
            Data.Ins.DB.SaveChanges();
            SelectionChanged(GetAncestorOfType<OrderManagementUC>(parameter));
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
    }
}