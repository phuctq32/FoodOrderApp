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
                OnPropertyChanged("ListReceipt");
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

        private int totalProductEachReceipt;

        public ICommand LoadedCommand { get; set; }
        public ICommand OpenOrderDetailWindowCommand { get; set; }
        public ICommand ConfirmReceiptCommand { get; set; }

        // status = 1 là trạng thái chờ xác nhận
        // status = 2 là trạng thái đang tiến hành
        // status = 3 là trạng thái đã hoàn thành
        // status = 4 là trạng thái đã huỷ
        public OrderManagementViewModel()
        {
            LoadedCommand = new RelayCommand<OrderManagementUC>(p => p == null ? false : true, p => Load(p));
            OpenOrderDetailWindowCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => OpenOrderDetailWindow(p));
            ConfirmReceiptCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => ConfirmReceipt(p));
        }

        private void ConfirmReceipt(ListViewItem p)
        {
            RECEIPT receipt = p.DataContext as RECEIPT;
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
            //Data.Ins.DB.RECEIPTs.Where(receiptDB => receiptDB.ID_ == receipt.ID_).Single().STATUS_ = "2";
            //Data.Ins.DB.SaveChanges();
        }

        private void OpenOrderDetailWindow(ListViewItem p)
        {
            RECEIPT receipt = p.DataContext as RECEIPT;
            OrderDetailAdminWindow orderDetailAdminWindow = new OrderDetailAdminWindow();
            ListReceiptDetail = Data.Ins.DB.RECEIPT_DETAIL.Where(receiptDetail => receiptDetail.RECEIPT_ID == receipt.ID_).ToList();
            orderDetailAdminWindow.listReceiptDetail.ItemsSource = listReceiptDetail;
            orderDetailAdminWindow.ShowDialog();
        }

        private void Load(OrderManagementUC p)
        {
            ListReceipt = Data.Ins.DB.RECEIPTs.Where(receipt => receipt.STATUS_ == "1").ToList();

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

        //int calculateTotalItem()
        //{
        //    totalProductEachReceipt = 0;
        //    foreach (var item in listReceiptDetail)
        //    {
        //        totalProductEachReceipt += item.AMOUNT_
        //    }
        //}
    }
}