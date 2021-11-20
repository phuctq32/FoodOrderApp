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
        private static string username;
        private static string password;
        public CurrentAccount()
        {
            IsUser = true;
            ProductsInCart = new List<PRODUCT>();
        }

        public static bool IsAdmin { get; set; }
        public static bool IsUser { get; set; }
        
        public static List<PRODUCT> ProductsInCart { get => productsInCart; set {
                productsInCart = value;
            }          
        }

        public static string Username { get => username; set => username = value; }
        public static string Password { get => password; set => password = value; }
    }
}