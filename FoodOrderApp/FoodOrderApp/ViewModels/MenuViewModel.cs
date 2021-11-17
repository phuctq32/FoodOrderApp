using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
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
    internal class MenuViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand IndexCommand { get; set; }
        public ICommand SortD { get; set; }
        private string search;
        private int index;
        public int Index { get => index; set { index = value; OnPropertyChanged(); } }
        public string Search { get => search; set { search = value; OnPropertyChanged(); } }
        public List<PRODUCT> products; 
        public PRODUCT pRODUCT = new PRODUCT();
        
        public MenuViewModel()
        {
            Index = -1;
            AddToCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => AddToCart(parameter));
            LoadedCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => Load(parameter));
            SearchCommand = new RelayCommand<MenuUC>((parameter) => true, (parameter) => BtnSearch(parameter));
            IndexCommand = new RelayCommand<ComboBox>((parameter) => true, (parameter) => { Index = parameter.SelectedIndex;});
            SortD = new RelayCommand<MenuUC>((parameter) => true, (parameter) => GiaT(parameter));
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
        private void GiaT(MenuUC parameter)
        {
            object item = new object();
            int v = (item as PRODUCT).PRICE_;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource);

            if (Index == 1)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription(v.ToString(), ListSortDirection.Descending));
            }
            else
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription(v.ToString(), ListSortDirection.Descending));
            }
        }
        public void BtnSearch(MenuUC parameter)
        {
            MessageBox.Show(Index.ToString());
            CollectionViewSource.GetDefaultView(parameter.ViewListProducts.ItemsSource).Refresh();
        }
        private bool UserFilter(object item)
        {
            // string a = RemoveSign4VietnameseString(pRODUCT.NAME_);
            string a = (item as PRODUCT).NAME_;
            a = RemoveSign4VietnameseString(a);
            if (String.IsNullOrEmpty(Search))
                return true;
            else
                return (a.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0);
                //return ((item as PRODUCT).NAME_.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private static readonly string[] VietnameseSigns = new string[]
                {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
                };
        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        private void AddToCart(ListViewItem parameter)
        {
           
        }
    }
}