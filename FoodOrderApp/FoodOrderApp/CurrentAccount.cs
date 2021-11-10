using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderApp
{
    public class CurrentAccount
    {
        public CurrentAccount()
        {
            IsAdmin = false;
            IsUser = true;
        }

        public static bool IsAdmin { get; set; }
        public static bool IsUser { get; set; }
        public static bool UserName { get; set; }
        public  static bool Password { get; set; }
    }
}