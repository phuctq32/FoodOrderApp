﻿using FoodOrderApp.Models;
using FoodOrderApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoodOrderApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MenuUC.xaml
    /// </summary>
    public partial class MenuUC : UserControl
    {
        public MenuUC()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }

        //private void addToCartBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    var item = ((sender as Button)?.Tag as ListViewItem).DataContext;
        //    PRODUCT p = (item as PRODUCT);
        //    CurrentAccount.productsInCart.Add(p);
        //}
    }
}