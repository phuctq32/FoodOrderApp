using FoodOrderApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderApp
{
    public class CurrentAccount
    {
        private static List<PRODUCT> productsInCart;
        public CurrentAccount()
        {
            IsAdmin = false;
            IsUser = true;
            ProductsInCart = new List<PRODUCT>();
        }

        public static bool IsAdmin { get; set; }
        public static bool IsUser { get; set; }
        public static bool UserName { get; set; }
        public  static bool Password { get; set; }
        public static List<PRODUCT> ProductsInCart { get => productsInCart; set {
                productsInCart = value;
            }          
        }

        //protected void OnPropertyChanged(List<PRODUCT> propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
    }
}