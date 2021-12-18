using FoodOrderApp.Models;
using FoodOrderApp.Views;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class ForgotPasswordViewModel : BaseViewModel
    {
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }

        public ICommand SendCodeCommand { get; set; }

        private string mail;

        public string Mail
        { get => mail; set { mail = value; OnPropertyChanged(); } }

        private string code;
        private int systemCode = 0;

        public string Code
        { get => code; set { code = value; OnPropertyChanged(); } }

        private string password;

        public string Password
        { get => password; set { password = value; OnPropertyChanged(); } }

        private string rePassword;

        public string RePassword
        { get => rePassword; set { rePassword = value; OnPropertyChanged(); } }

        public ForgotPasswordViewModel()
        {
            Code = "";
            ChangePasswordCommand = new RelayCommand<ForgotPasswordWindow>((parameter) => true, (parameter) => ChangePassword(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { RePassword = parameter.Password; });
            SendCodeCommand = new RelayCommand<ForgotPasswordWindow>((parameter) => true, (parameter) => SendCode(parameter));
        }

        public void SendCode(ForgotPasswordWindow parameter)
        {
            Random random = new Random();
            systemCode = random.Next(100000, 999999);
            sendGmail("sadam01664@gmail.com", Mail, "FOOD ORDER APP", "Your code is : " + systemCode.ToString());
        }

        public void ChangePassword(ForgotPasswordWindow parameter)
        {
            //Check Mail
            if (string.IsNullOrEmpty(parameter.txtMail.Text))
            {
                CustomMessageBox.Show("Email đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtMail.Focus();
                parameter.txtMail.Text = "";
                return;
            }
            if (!Regex.IsMatch(parameter.txtMail.Text.Trim(), @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                parameter.txtMail.Focus();
                CustomMessageBox.Show("Email không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (parameter.txtMail.Text.Contains(" "))
            {
                CustomMessageBox.Show("Email không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtMail.Focus();
                return;
            }

            //Check Password
            if (string.IsNullOrEmpty(parameter.PasswordBox.Password))
            {
                parameter.PasswordBox.Focus();
                parameter.PasswordBox.Password = "";
                CustomMessageBox.Show("Mật khẩu trống", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (parameter.PasswordBox.Password.Contains(" "))
            {
                parameter.RePasswordBox.Focus();
                CustomMessageBox.Show("Mật khẩu không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(parameter.RePasswordBox.Password))
            {
                parameter.RePasswordBox.Focus();
                parameter.RePasswordBox.Password = "";
                CustomMessageBox.Show("Chưa xác nhận mật khẩu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (parameter.RePasswordBox.Password.Contains(" "))
            {
                parameter.RePasswordBox.Focus();
                CustomMessageBox.Show("Mật khẩu nhập lại không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password != RePassword)
            {
                parameter.RePasswordBox.Focus();
                CustomMessageBox.Show("Nhập lại mật khẩu không đúng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //Nếu chưa nhấn
            if (systemCode == 0)
            {
                CustomMessageBox.Show("Bạn chưa nhận mã xác thực!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (systemCode.ToString().Contains(" "))
            {
                CustomMessageBox.Show("Mã xác nhận không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string tmpCode = systemCode.ToString().Trim();
            string tmp = Code.Trim();
            if (tmp != tmpCode)
            {
                CustomMessageBox.Show("Mã xác nhận không đúng!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                USER user = Data.Ins.DB.USERS.Where(x => x.EMAIL_ == Mail.Trim()).SingleOrDefault();
                user.PASSWORD_ = Password.Trim();
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Đổi mật khẩu thành công!", MessageBoxButton.OK);
                systemCode = 0;
                parameter.Close();
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu");
            }
        }
    }
}