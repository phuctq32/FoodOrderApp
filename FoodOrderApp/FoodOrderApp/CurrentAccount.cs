using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderApp
{
    internal class CurrentAccount
    {
        private bool isAdmin;
        private bool isUser;
        private bool userName;
        private bool password;

        public CurrentAccount()
        {
            IsAdmin = true;
            isUser = false;
        }

        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public bool IsUser { get => isUser; set => isUser = value; }
        public bool UserName { get => userName; set => userName = value; }
        public bool Password { get => password; set => password = value; }
    }
}