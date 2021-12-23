using FoodOrderApp.Models;
using FoodOrderApp.Views;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class ChangeInformationViewModel : BaseViewModel
    {
        public ICommand SaveInfoCommand { get; set; }

        private USER user;

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
            SaveInfoCommand = new RelayCommand<ChangeInformationWindow>((parameter) => true, (parameter) => SaveChange(parameter));
            user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            this.Phone = user.PHONE_;
            this.FULLNAME_ = user.FULLNAME_;
            this.Address = user.ADDRESS_;
            this.Mail = user.EMAIL_;
        }

        public void SaveChange(ChangeInformationWindow parameter)
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
            int mailCount = Data.Ins.DB.USERS.Where(x => x.EMAIL_ == Mail.Trim()).Count();
            if (mailCount > 0 && (Mail.Trim() != parameter.txtMail.Text.ToString().Trim()))
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
            //setInfo after check
            user.EMAIL_ = parameter.txtMail.Text.Trim();
            user.PHONE_ = parameter.txtPhone.Text.Trim();
            user.FULLNAME_ = parameter.txtFullname.Text.Trim();
            user.ADDRESS_ = parameter.txtAddress.Text.Trim();
            try
            {
                //try to update database
                Data.Ins.DB.SaveChanges();
                FULLNAME_ = user.FULLNAME_;
                Phone = user.PHONE_;
                Mail = user.EMAIL_;
                Address = user.ADDRESS_;
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