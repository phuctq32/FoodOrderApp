using FoodOrderApp.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MyOrderViewModel : BaseViewModel
    {
        public ICommand RatingCommand { get; set; }
        public MyOrderViewModel()
        {
            RatingCommand = new RelayCommand<RatingBar>(p => true, p => RatingChanged(p));
        }
        private void RatingChanged(RatingBar p)
        {
            //CustomMessageBox.Show(p.Value.ToString());
            //p.IsEnabled = false;
        }
    }
}