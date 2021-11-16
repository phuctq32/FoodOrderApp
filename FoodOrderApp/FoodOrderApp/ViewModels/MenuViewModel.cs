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
using System.Windows.Data;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MenuViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        private string search;
        public string Search { get => search; set { search = value; OnPropertyChanged(); } }
        public List<PRODUCT> products;

        public MenuViewModel()
        {
            AddToCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => AddToCart(parameter));
            LoadedCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => Load(parameter));
            SearchCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => BtnSearch(parameter));
            //AddToCartCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, p => AddToCart(p));

        }

        //private bool UserFilter(object item)
        //{
        //    if (String.IsNullOrEmpty(Search.Text))
        //        return true;
        //    else
        //        return (item as PRODUCT).NAME_.IndexOf(parameter.ViewListProducts..Search.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        //}
        private void Load(MenuUC parameter)
        {
            products = Data.Ins.DB.PRODUCTs.ToList();
            parameter.ViewListProducts.ItemsSource = products;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource);
            view.Filter = UserFilter;
        }
        public void BtnSearch(MenuUC parameter)
        {
            CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource).Refresh();
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Search))
                return true;
            else
                return ((item as PRODUCT).NAME_.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void AddToCart(ListViewItem parameter)
        {
           
        }
    }
}