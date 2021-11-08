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
    /// Interaction logic for CustomMessageBoxWindow.xaml
    /// </summary>
    partial class CustomMessageBoxWindow : Window
    {
        internal CustomMessageBoxWindow(string message)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
        }

        internal CustomMessageBoxWindow(string message, MessageBoxImage image)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
            DisplayImageIcon(image);
        }

        internal CustomMessageBoxWindow(string message, MessageBoxButton button)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
            DisplayButton(button);
        }

        internal CustomMessageBoxWindow(string message, MessageBoxButton button, MessageBoxImage image)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
            DisplayButton(button);
            DisplayImageIcon(image);
        }

        internal string Message { get => textblock.Text; set => textblock.Text = value; }

        public MessageBoxResult Result { get; set; }

        private void DisplayButton(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    this.btn_OK.Visibility = Visibility.Visible;
                    this.btn_Cancel.Visibility = Visibility.Visible;
                    this.btn_OK.Focus();
                    break;
                case MessageBoxButton.OK:
                    this.btn_OK.Visibility = Visibility.Visible;
                    this.btn_OK.Focus();
                    break;
                default:
                    break;
            }
        }

        private void DisplayImageIcon(MessageBoxImage image)
        {
            BitmapImage bitmapImage = new BitmapImage();

            switch (image)
            {
                default:
                    break;
            }

            this.image_icon.Visibility = Visibility.Visible;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            this.Close();
        }

    }
}
