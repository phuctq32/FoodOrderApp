using FoodOrderApp.Models;
using FoodOrderApp.Views;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class ChangeInformationViewModel : BaseViewModel
    {
        public ICommand SaveInfoCommand { get; set; }
        public ICommand SaveAddressCommand { get; set; }

        private string mail;

        public string Mail
        { get => mail; set { mail = value; OnPropertyChanged("Mail"); } }

        private string fullname;

        public string FULLNAME_
        { get => fullname; set { fullname = value; OnPropertyChanged("FULLNAME_"); } }

        private string phone;

        public string Phone
        { get => phone; set { phone = value; OnPropertyChanged("Phone"); } }

        private string address;

        public string Address
        { get => address; set { address = value; OnPropertyChanged("Address"); } }

        public ChangeInformationViewModel()
        {
            SaveInfoCommand = new RelayCommand<ChangeInformationWindow>((parameter) => true, (parameter) => SaveChangesAccount(parameter));
            CurrentAccount.User = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            SaveAddressCommand = new RelayCommand<ChangeInformationWindow>((parameter) => true, (parameter) => SaveChangesAddress(parameter));
        }

        private void SaveChangesAddress(ChangeInformationWindow parameter)
        {
            if (string.IsNullOrEmpty(parameter.txtAddress.Text))
            {
                CustomMessageBox.Show("Địa chỉ đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtMail.Focus();
                parameter.txtMail.Text = "";
                return;
            }
            CurrentAccount.User.ADDRESS_ = parameter.txtAddress.Text;
            try
            {
                //try to update database
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Thay đổi thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                CustomMessageBox.Show("Thay đổi không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            parameter.Close();
        }

        public void SaveChangesAccount(ChangeInformationWindow parameter)
        {
            /// Check Mail
            if (string.IsNullOrEmpty(parameter.txtMail.Text))
            {
                CustomMessageBox.Show("Email đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtMail.Focus();
                parameter.txtMail.Text = "";
                return;
            }
            if (parameter.txtMail.Text.Contains(" "))
            {
                CustomMessageBox.Show("Email không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtMail.Focus();
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
            int mailCount = Data.Ins.DB.USERS.Where(x => x.EMAIL_ == parameter.txtMail.Text.ToString().Trim()).Count();
            if (mailCount > 0 && (CurrentAccount.User.EMAIL_ != parameter.txtMail.Text.ToString().Trim()))
            {
                parameter.txtMail.Focus();
                CustomMessageBox.Show("Mail đã tồn tại!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Check PhoneNumber
            if (string.IsNullOrEmpty(parameter.txtPhone.Text))
            {
                parameter.txtPhone.Focus();
                CustomMessageBox.Show("Số điện thoại đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPhone.Text = "";
                return;
            }
            if (!parameter.txtPhone.Text.StartsWith("0"))
            {
                parameter.txtPhone.Focus();
                CustomMessageBox.Show("Số điện thoại phải bắt đầu bằng 0!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPhone.Text = "";
                return;
            }
            if (!Regex.IsMatch(parameter.txtPhone.Text, @"^[0-9_]+$"))
            {
                parameter.txtPhone.Focus();
                CustomMessageBox.Show("Số điện thoại không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPhone.Text = "";
                return;
            }
            //setInfo after check
            CurrentAccount.User.EMAIL_ = parameter.txtMail.Text.Trim();
            CurrentAccount.User.PHONE_ = parameter.txtPhone.Text.Trim();
            CurrentAccount.User.FULLNAME_ = parameter.txtFullname.Text.Trim();
            CurrentAccount.User.ADDRESS_ = parameter.txtAddress.Text.Trim();
            try
            {
                //try to update database
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Thay đổi thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                CustomMessageBox.Show("Thay đổi không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            parameter.Close();
        }
    }
}