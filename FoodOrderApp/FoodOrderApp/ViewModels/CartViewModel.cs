using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public CartViewModel() 
        {
            LoadedCommand = new RelayCommand<CartUC>(p => true, p => DisplayCart(p));
        }

        private void DisplayCart(CartUC cartUC)
        {
            cartUC.cartList.ItemsSource = CurrentAccount.ProductsInCart;
        }
    }
}
