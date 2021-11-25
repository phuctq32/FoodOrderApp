using FoodOrderApp.Models;
using FoodOrderApp.Views;
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
        public ICommand RatingCommand { get; set; }
        public ICommand OpenInvoiceCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        private List<RECEIPT> receipts;

        // status = 1 là trạng thái chờ xác nhận
        // status = 2 là trạng thái đang tiến hành
        // status = 3 là trạng thái đã hoàn thành
        // status = 4 là trạng thái đã huỷ
        public MyOrderViewModel()
        {
            RatingCommand = new RelayCommand<RatingBar>(p => true, p => RatingChanged(p));
            OpenInvoiceCommand = new RelayCommand<ListViewItem>(p => true, p => openInvoice(p));
            PrintCommand = new RelayCommand<InvoiceWindow>(paramater => true, paramater => print(paramater));
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

        private void openInvoice(ListViewItem paramater)
        {
            RECEIPT receipt = paramater.DataContext as RECEIPT;
            if (CustomMessageBox.Show("Bạn có muốn in hoá đơn?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                InvoiceWindow invoiceWindow = new InvoiceWindow();
                invoiceWindow.listView.ItemsSource = receipts;
                invoiceWindow.Show();
            }

            //receipt.STATUS_
        }

        private void RatingChanged(RatingBar p)
        {
            //CustomMessageBox.Show(p.Value.ToString());
            //p.IsEnabled = false;
        }
    }
}