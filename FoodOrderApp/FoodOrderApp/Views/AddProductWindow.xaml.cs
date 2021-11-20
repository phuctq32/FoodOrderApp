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
        public AddProductWindow(bool Add)
        {
            InitializeComponent();
            if(Add = true)
            {
                this.updatebtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.addbtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}
