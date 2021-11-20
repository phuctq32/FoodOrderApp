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
    internal class ChangeInformationViewModel : BaseViewModel
    {
        public ICommand SaveInfoCommand { get; set; }

        //public ICommand PasswordChangedCommand { get; set; }
        //public ICommand RePasswordChangedCommand { get; set; }
        //public ICommand ActivationCommand { get; set; }

        private USER user;

        private string mail;

        public string Mail
        { get => mail; set { mail = value; OnPropertyChanged("Mail"); } }

        private string fullname;

        public string FULLNAME_
        { get => fullname; set { fullname = value; OnPropertyChanged("Fullname"); } }

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

        public void SaveChange(ChangeInformationWindow changeInformationWD)
        {
            user.PHONE_ = changeInformationWD.txtPhone.Text;
            user.FULLNAME_ = changeInformationWD.txtFullname.Text;
            user.ADDRESS_ = changeInformationWD.txtAddress.Text;
            try
            {
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
            changeInformationWD.Close();
        }
    }
}

