using FoodOrderApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderApp
{
    public class CurrentAccount : ViewModels.BaseViewModel
    {
        private static string username;
        private static string password;
        
        public CurrentAccount()
        {
            
        }

        public static bool IsAdmin { get; set; }
        public static bool IsUser { get; set; }
        

        public static string Username { get => username; set => username = value; }
        public static string Password { get => password; set => password = value; }
    }
}