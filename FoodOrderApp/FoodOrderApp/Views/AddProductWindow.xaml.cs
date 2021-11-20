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
using System.Windows.Shapes;

namespace FoodOrderApp.Views
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            this.updatebtn.Visibility = Visibility.Collapsed;
        }
        public AddProductWindow(PRODUCT pRODUCT)
        {
            InitializeComponent();
            this.addbtn.Visibility = Visibility.Collapsed;
            this.txtName.Text = pRODUCT.NAME_;
            this.txtDiscount.Text = pRODUCT.DISCOUNT_.ToString();
            this.txtPrice.Text = pRODUCT.PRICE_.ToString();
            this.txtDescription.Text = pRODUCT.DESCRIPTION_;
        }
    }
}
