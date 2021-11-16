﻿using System;
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
    /// Interaction logic for CartUC.xaml
    /// </summary>
    public partial class CartUC : UserControl
    {
        public CartUC()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentAccount.ProductsInCart.Clear();
        }
    }
}