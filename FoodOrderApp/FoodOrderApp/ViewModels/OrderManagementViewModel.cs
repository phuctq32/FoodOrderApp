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
using System.Globalization;
using System.Data.Entity.Validation;
using FoodOrderApp.Views.UserControls.Admin;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        public int countItemInReceipt
        {
            get;
            set;
        }

        public int Status { get; set; }

        public int numberOfDistinctProduct { get; set; }

        public ICommand LoadedCommand { get; set; }
        public ICommand OpenOrderDetailWindowCommand { get; set; }
        public ICommand ConfirmReceiptCommand { get; set; }
        public ICommand CancelReceiptCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }

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
        }

        private void ConfirmReceipt(ListViewItem parameter)
        {
            RECEIPT receipt = parameter.DataContext as RECEIPT;
            List<RECEIPT_DETAIL> listReceipt = Data.Ins.DB.RECEIPT_DETAIL.Where(x => x.RECEIPT_ID == receipt.ID_).ToList();
            if (CustomMessageBox.Show("In hóa đơn?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                InvoiceWindow invoiceWindow = new InvoiceWindow();
                invoiceWindow.listView.ItemsSource = receipt;
                invoiceWindow.Show();

            }
            // đoạn này t với th phúc định kiểu như: bấm xác nhận đơn hàng thì sẽ hiện ra hỏi có in hoá đơn không
            // có thì hiện ra khung in hoá đơn rồi chuyển trạng thái
            // không thì chỉ chuyển trạng thái thôi
            // cái invoice t tạo bị sai nên chưa làm được khúc in hoá đơn nên chỗ in để t tự thêm sau
            // m cứ chuyển trạng thái trước đi
            //if (CustomMessageBox.Show("Bạn có muốn in hoá đơn?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    InvoiceWindow invoiceWindow = new InvoiceWindow();
            //    //invoiceWindow.listView.ItemsSource = receipt;
            //}
            // chỗ này t định code chuyển trạng thái kiểu vầy, đã thử và được nhá
            //Data.Ins.DB.RECEIPTs.Where(receiptDB => receiptDB.ID_ == receipt.ID_).Single().STATUS_ = "1";
            //Data.Ins.DB.SaveChanges();
            else
            {
                //List<RECEIPT> listConfirmReceipt = Data.Ins.DB.RECEIPTs.Where(receiptDB => receiptDB.ID_ == receipt.ID_).ToList();
                //foreach (var item in listConfirmReceipt)
                //{
                //    int tmp = Int32.Parse(item.STATUS_);
                //    if (tmp <= 1)
                //        tmp++;
                //    item.STATUS_ = tmp.ToString();
                //}
                //ListReceipt.Clear();
                //Data.Ins.DB.SaveChanges();
                //SelectionChanged(GetAncestorOfType<OrderManagementUC>(parameter));
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
            Fullname = receipt.USER.FULLNAME_;
            Address = receipt.USER.ADDRESS_;
            Phone = receipt.USER.PHONE_;
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

        private void print(InvoiceWindow paramater)
        {
            // code để t in invoice thoai
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