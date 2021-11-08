using FoodOrderApp.Models;
using FoodOrderApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class SignUpViewModel : BaseViewModel
    {
        public ICommand SignUpCommand { get; set; }

        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }



        private string mail;
        public string Mail { get => mail; set { mail = value; OnPropertyChanged(); } }

        private string phone;
        public string Phone { get => phone; set { phone = value; OnPropertyChanged(); } }

        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }

        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }

        private string rePassword;
        public string RePassword { get => rePassword; set { rePassword = value; OnPropertyChanged(); } }

        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand<SignUpWindow>((parameter) => true, (parameter) => SignUp(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { RePassword = parameter.Password; });
        }

        /*public void SignUp(SignUpWindow parameter)
        {
            //isSignUp = false;

            // Check username
            //if (string.IsNullOrEmpty(parameter.txtUsername.Text))
            //{
            //    parameter.txtUsername.Focus();
            //    parameter.txtUsername.Text = "";
            //    return;
            //}

            //if (!Regex.IsMatch(parameter.txtUsername.Text, @"^[a-zA-Z0-9_]+$"))
            //{
            //    parameter.txtUsername.Focus();
            //    return;
            //}

            parameter.grdInformation.Visibility = System.Windows.Visibility.Collapsed;
            parameter.transitionContentSlideInside.Visibility = System.Windows.Visibility.Visible;
        }*/

        public void SignUp(SignUpWindow parameter)
        {

            /// Check Mail
            if (string.IsNullOrEmpty(parameter.txtMail.Text))
            {
                parameter.txtMail.Focus();
                parameter.txtMail.Text = "";
                return;
            }
            /// Check Mail exist
            int mailCount = Data.Ins.DB.USERS.Where(x => x.EMAIL_ == Mail).Count();
            if (mailCount > 0)
            {
                parameter.txtMail.Focus();
                MessageBox.Show("Mail đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            //Check Phone
            if (string.IsNullOrEmpty(parameter.txtPhone.Text))
            {
                parameter.txtPhone.Focus();
                parameter.txtPhone.Text = "";
                return;
            }


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
            // Check User exist
            int accCount = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == UserName).Count();
            if (accCount > 0)
            {
                parameter.txtUsername.Focus();
                MessageBox.Show("Tài khoản đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            //Check Password
            if (string.IsNullOrEmpty(parameter.PasswordBox.Password))
            {
                parameter.PasswordBox.Focus();
                parameter.PasswordBox.Password = "";
                return;
            }
            if (string.IsNullOrEmpty(parameter.RePasswordBox.Password))
            {
                parameter.RePasswordBox.Focus();
                parameter.RePasswordBox.Password = "";
                return;
            }

            if (Password != RePassword)
            {
                parameter.RePasswordBox.Focus();
                MessageBox.Show("Nhập lại mật khẩu không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Data.Ins.DB.USERS.Add(new USER() { EMAIL_ = Mail, PHONE_ = Phone, USERNAME_ = UserName, PASSWORD_ = Password, TYPE_ = "user" });
                Data.Ins.DB.SaveChanges();
                MessageBox.Show("Đăng ký tài khoản thành công");
                parameter.Close();
            }
            catch
            {
                MessageBox.Show("Lỗi cơ sở dữ liệu");
            }
        }
    }
}