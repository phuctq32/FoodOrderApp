﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;

namespace FoodOrderApp.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        public ICommand OpenForgotPasswordWDCommand { get; set; }
        public ICommand OpenSignUpWDCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand OpenLogInWDCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }


        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }
        private bool isLogin;
        public bool IsLogin { get => isLogin; set => isLogin = value; }



        public LoginViewModel()
        {
            Password = "";
            OpenLogInWDCommand = new RelayCommand<LoginWindow>((parameter) => { return true; }, (parameter) => Login(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            OpenForgotPasswordWDCommand = new RelayCommand<LoginWindow>((parameter) => true, (parameter) => OpenForgotPasswordWindow(parameter));
            OpenSignUpWDCommand = new RelayCommand<LoginWindow>((parameter) => true, (parameter) => OpenSignUpWindow(parameter));
            LoadedCommand = new RelayCommand<ControlBarUC>(p => true, (p) => Loaded(p));
            CloseWindowCommand = CloseWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p => {
                if(CustomMessageBox.Show("Thoát ứng dụng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    FrameworkElement window = ControlBarViewModel.GetParentWindow(p);
                    var w = window as Window;

                    if (w != null)
                    {
                        w.Close();
                    }
                }
            });
        }

        public void Login(LoginWindow parameter)
        {
            
            try
            {

                isLogin = false;
                if (parameter == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(parameter.txtUsername.Text))
                {
                    CustomMessageBox.Show("Vui lòng nhập tên đăng nhập!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.txtUsername.Focus();
                    return;
                }
                //check password
                if (string.IsNullOrEmpty(parameter.txtPassword.Password))
                {
                    CustomMessageBox.Show("Vui lòng nhập mật khẩu!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.txtPassword.Focus();
                    return;
                }

                //string passEncode = MD5Hash(Password)   ;
                int accCount = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == UserName && x.PASSWORD_ == Password).Count();
                Data.Ins.DB.USERS.ToList();
                /*List<USER> acc = Data.Ins.DB.USERS.ToList(); 
                foreach (var a in acc)
                {
                    MessageBox.Show(a.USERNAME_ + a.PASSWORD_);
                } */   
                if (accCount > 0)
                {
                    isLogin = true;
                    if(UserName == "admin")
                    {
                        CurrentAccount.IsAdmin = true;
                        CurrentAccount.IsUser = false;
                    }
                    else
                    {
                        CurrentAccount.IsAdmin = false;
                        CurrentAccount.IsUser = true;
                    }
                    CurrentAccount.Username = UserName;
                    
                    MainWindow app = new MainWindow();
                    app.ShowDialog();
                    parameter.Close();
                    parameter.txtPassword.Clear();
                }
                else
                {
                    isLogin = false;
                    CustomMessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", MessageBoxButton.OK, MessageBoxImage.Error);
                    parameter.txtPassword.Focus();
                }

            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu");
            }
        }


        public void Loaded(ControlBarUC cb)
        {
            cb.closeBtn.Command = CloseWindowCommand;
            cb.closeBtn.CommandParameter = cb;
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