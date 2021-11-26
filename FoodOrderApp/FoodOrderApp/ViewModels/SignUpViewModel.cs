using FoodOrderApp.Models;
using FoodOrderApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public ICommand ActivationCommand { get; set; }
    



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

        private string code;
        public string Code { get => code; set { code = value; OnPropertyChanged(); } }

        private int systemCode=0;

        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand<SignUpWindow>((parameter) => true, (parameter) => SignUp(parameter));
            ActivationCommand = new RelayCommand<SignUpWindow>((parameter) => true, (parameter) => Activation(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { RePassword = parameter.Password; });
        }

        public void SignUp(SignUpWindow parameter)
        {

            /// Check Mail
            if (string.IsNullOrEmpty(parameter.txtMail.Text))
            {
                CustomMessageBox.Show("Email đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtMail.Focus();
                parameter.txtMail.Text = "";
                return;
            }
            if (!Regex.IsMatch(parameter.txtMail.Text, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                parameter.txtMail.Focus();
                CustomMessageBox.Show("Email không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            /// Check Mail exist
            int mailCount = Data.Ins.DB.USERS.Where(x => x.EMAIL_ == Mail).Count();
            if (mailCount > 0)
            {
                parameter.txtMail.Focus();
                CustomMessageBox.Show("Mail đã tồn tại!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            //Check Phone
            if (string.IsNullOrEmpty(parameter.txtPhone.Text))
            {
                parameter.txtPhone.Focus();
                CustomMessageBox.Show("Số điện thoại đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPhone.Text = ""; 
                return;
            }

            // if (!Regex.IsMatch(parameter.txtUsername.Text, @"^[a-zA-Z0-9_]+$"))
            // Check username
            if (string.IsNullOrEmpty(parameter.txtUsername.Text))
            {
                parameter.txtUsername.Focus();
                CustomMessageBox.Show("Tài khoản đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                CustomMessageBox.Show("Tài khoản đã tồn tại!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (string.IsNullOrEmpty(parameter.RePasswordBox.Password))
            {
                parameter.RePasswordBox.Focus();
                parameter.RePasswordBox.Password = "";
                CustomMessageBox.Show("Chưa xác nhận mật khẩu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password != RePassword)
            {
                parameter.RePasswordBox.Focus();
                CustomMessageBox.Show("Nhập lại mật khẩu không đúng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ///Tạo code
            Random random = new Random();
            systemCode = random.Next(100000, 999999);
            sendGmail("teambaylttq@gmail.com", Mail, "FOOD ORDER APP", "Your code is : " + systemCode.ToString());
            ///Hiện xác nhận mã 
            parameter.grdInformation.Visibility = Visibility.Collapsed;
            parameter.transitionContentSlideInside.Visibility = Visibility.Visible;
            //CustomMessageBox.Show(systemCode.ToString(), MessageBoxButton.OK);


        }
        public void Activation(SignUpWindow parameter)
        {
            if (systemCode == 0)
            {
                CustomMessageBox.Show("Bạn chưa nhận mã xác thực!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string tmpCode = systemCode.ToString();
            string tmp = Code;
            if (tmp != tmpCode )
            {
                CustomMessageBox.Show("Mã xác nhận không đúng!!", MessageBoxButton.OK);
                return;
            }
             try
             {

                  Data.Ins.DB.USERS.Add(new USER() { EMAIL_ = Mail, PHONE_ = Phone, USERNAME_ = UserName, PASSWORD_ = Password, TYPE_ = "user" });
                  Data.Ins.DB.SaveChanges();
                 CustomMessageBox.Show("Đăng ký thành công",MessageBoxButton.OK);
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