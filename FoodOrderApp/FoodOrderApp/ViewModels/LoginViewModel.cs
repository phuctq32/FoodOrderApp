using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FoodOrderApp.Views;

namespace FoodOrderApp.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        public ICommand OpenForgotPasswordWDCommand { get; set; }
        public ICommand OpenSignUpWDCommand { get; set; }

        public LoginViewModel()
        {
            OpenForgotPasswordWDCommand = new RelayCommand<LoginWindow>((parameter) => true, (parameter) => OpenForgotPasswordWindow(parameter));
            OpenSignUpWDCommand = new RelayCommand<LoginWindow>((parameter) => true, (parameter) => OpenSignUpWindow(parameter));
        }

        public void OpenForgotPasswordWindow(LoginWindow parameter)
        {

            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.ShowDialog();
        }

        public void OpenSignUpWindow(LoginWindow parameter)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.ShowDialog();
        }
    }
}