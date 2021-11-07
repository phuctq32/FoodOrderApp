using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FoodOrderApp.Views;

namespace FoodOrderApp
{
    public static class CustomMessageBox
    {
        public static MessageBoxResult Show(string message)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message);
            msg.ShowDialog();

            return msg.Result;
        }

        public static MessageBoxResult Show(string message, MessageBoxButton button)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message, button);
            msg.ShowDialog();

            return msg.Result;
        }

        public static MessageBoxResult Show(string message, MessageBoxButton button, MessageBoxImage icon)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message, button, icon);
            msg.ShowDialog();

            return msg.Result;
        }
    }
}
