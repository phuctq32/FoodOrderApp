using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NumbericSpinner.xaml
    /// </summary>
    public partial class NumericSpinner : UserControl
    {
        private decimal value;
        public NumericSpinner()
        {
            InitializeComponent();
            Value = 1;
        }

        public decimal Value { get => value; set {
                if(value > decimal.MaxValue)
                {
                    this.value = decimal.MaxValue;
                }
                else if(value < decimal.MinValue)
                {
                    this.value = decimal.MinValue;
                }
                else
                {
                    this.value = value;
                }
            }
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (Value == 1)
            {
                CustomMessageBox.Show("Bạn có muốn xóa sản phảm khỏi giỏ hàng không?", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            }
            Value -= 1;
            tb_main.Text = Value.ToString();
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            Value += 1;
            tb_main.Text = Value.ToString();
        }
    }
}
