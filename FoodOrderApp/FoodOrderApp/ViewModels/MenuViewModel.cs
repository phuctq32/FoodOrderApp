using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MenuViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        private List<PRODUCT> products;

        public MenuViewModel()
        {
            LoadedCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => Load(parameter));
            AddToCartCommand = new RelayCommand<ExecutedRoutedEventArgs>(p => p == null ? false : true, p => AddToCart(p));
        }

        private void Load(MenuUC parameter)
        {
            products = Data.Ins.DB.PRODUCTs.ToList();

            parameter.ViewListProducts.ItemsSource = products;
        }

        private void AddToCart(ExecutedRoutedEventArgs e)
        {
            var p = (e.Parameter as ListViewItem).DataContext;
            CurrentAccount.productsInCart.Add(p as PRODUCT);
        }
    }
}