using FoodOrderApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class SignUpViewModel : BaseViewModel
    {
        public ICommand SignUpCommand { get; set; }
        public ICommand SwichTabCommand { get; set; }

        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }

        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand<SignUpWindow>((parameter) => true, (parameter) => SignUp(parameter));
            SwichTabCommand = new RelayCommand<SignUpWindow>((parameter) => true, (parameter) => swichTab(parameter));
        }

        public void swichTab(SignUpWindow parameter)
        {
            parameter.grdActivation.Visibility = System.Windows.Visibility.Collapsed;
            parameter.transitionContentSlideInside.Visibility = System.Windows.Visibility.Visible;
        }

        public void SignUp(SignUpWindow parameter)
        {
            //isSignUp = false;

            // Check username
            if (string.IsNullOrEmpty(parameter.txtUsername.Text))
            {
                parameter.txtUsername.Focus();
                parameter.txtUsername.Text = "";
                return;
            }

            if (!Regex.IsMatch(parameter.txtUsername.Text, @"^[a-zA-Z0-9_]+$"))
            {
                parameter.txtUsername.Focus();
                return;
            }
        }
    }
}