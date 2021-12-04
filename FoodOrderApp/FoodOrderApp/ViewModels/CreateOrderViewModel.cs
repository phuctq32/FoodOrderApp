using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using FoodOrderApp.Views.UserControls.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    internal class CreateOrderViewModel : BaseViewModel
    {
        private string search = "";
        private List<PRODUCT> products;
        public string Search
        { get => search; set { search = value; OnPropertyChanged("Search"); } }
        public List<PRODUCT> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged("Products");
            }
        }
        public ICommand LoadedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand FilterCommand { get; set; }


        public CreateOrderViewModel()
        {
            Products = Data.Ins.DB.PRODUCTs.ToList();
            LoadedCommand = new RelayCommand<CreateOrderWindow>((parameter) => true, (parameter) => Load(parameter));
            SearchCommand = new RelayCommand<CreateOrderWindow>((parameter) => parameter == null ? false : true, (parameter) => SearchFunction(parameter));
            FilterCommand = new RelayCommand<ComboBox>((parameter) => true, (parameter) => Filter(parameter));
        }
        private void Load(CreateOrderWindow parameter)
        {
            Products = Data.Ins.DB.PRODUCTs.ToList();
        }

        private void SearchFunction(CreateOrderWindow parameter)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource);
            view.Filter = CompareString;
            CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource).Refresh();
        }
        private bool CompareString(object item)
        {
            string a = (item as PRODUCT).NAME_;
            Search = Search.Trim();
            string b = Search;
            a = MenuViewModel.RemoveSign4VietnameseString(a);
            if (b != null)
            {
                b = MenuViewModel.RemoveSign4VietnameseString(b);
            }
            if (string.IsNullOrEmpty(b))
                return true;
            else
                return (a.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void Filter(ComboBox item)
        {
            var createOrderWD = GetAncestorOfType<CreateOrderWindow>(item);
            if (item.SelectedIndex == 0)
            {
                Products = Data.Ins.DB.PRODUCTs.OrderBy(p => p.PRICE_ * (1 - p.DISCOUNT_)).ToList();
            }
            else if (item.SelectedIndex == 1)
            {
                Products = Data.Ins.DB.PRODUCTs.OrderByDescending(p => p.PRICE_ * (1 - p.DISCOUNT_)).ToList();
            }
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(createOrderWD.ViewListProducts.ItemsSource);
            view.Filter = CompareString;
            CollectionViewSource.GetDefaultView(createOrderWD.ViewListProducts.ItemsSource).Refresh();
        }
    }
}
