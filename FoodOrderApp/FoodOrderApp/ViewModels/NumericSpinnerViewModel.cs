using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FoodOrderApp.ViewModels
{
    class NumericSpinnerViewModel
    {
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public NumericSpinnerViewModel()
        {
            DownCommand = new RelayCommand<NumericSpinner>(p => true, p => Down(p));
            UpCommand = new RelayCommand<NumericSpinner>(p => true, p => Up(p));
        }
        private void Down(NumericSpinner parameter)
        {
            int amount = Int32.Parse(parameter.tb_main.Text.ToString());
            if (amount == 1)
            {
                if (CustomMessageBox.Show("Xóa món ăn khỏi giỏ hàng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    try
                    {
                        var lvi = GetAncestorOfType<ListViewItem>(parameter);
                        PRODUCT product = lvi.DataContext as PRODUCT;
                        CART cartToDelete = Data.Ins.DB.CARTs.Where(x => x.PRODUCT_ == product.ID_ && x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
                        Data.Ins.DB.CARTs.Remove(cartToDelete);
                        Data.Ins.DB.SaveChanges();
                        CustomMessageBox.Show("Xóa thành công!", MessageBoxButton.OK, MessageBoxImage.None);
                    }
                    catch
                    {
                        CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                amount--;
                parameter.tb_main.Text = amount.ToString();
            }
        }
        private void Up(NumericSpinner parameter)
        {
            int amount = Int32.Parse(parameter.tb_main.Text.ToString());
            if (amount == int.MaxValue)
            {
                return;
            }
            else
            {
                amount++;
                parameter.tb_main.Text = amount.ToString();
            }
        }
        public T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }
    }
}
