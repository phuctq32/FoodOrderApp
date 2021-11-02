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
    class LoginViewModel : BaseViewModel
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
            DoubleAnimation fadeIn = new DoubleAnimation(0.5, 1.0, new Duration(TimeSpan.FromSeconds(0.3)));

            //ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            //forgotPasswordWindow.BeginAnimation(Window.OpacityProperty, fadeIn);
            //forgotPasswordWindow.ShowDialog();
        }

        public void OpenSignUpWindow(LoginWindow parameter)
        {
            DoubleAnimation fadeIn = new DoubleAnimation(0.5, 1.0, new Duration(TimeSpan.FromSeconds(0.3)));

            //SignUpWindow signUpWindow = new SignUpWindow();
            //signUpWindow.BeginAnimation(Window.OpacityProperty, fadeIn);
            //signUpWindow.ShowDialog();
        }
    }
}
