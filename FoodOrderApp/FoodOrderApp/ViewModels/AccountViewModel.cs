﻿using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Azure.Storage.Blobs;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace FoodOrderApp.ViewModels
{
    internal class AccountViewModel : BaseViewModel
    {
        public ICommand UploadImageCommand { get; set; }
        public ICommand ChangeInfoCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        private USER user;
        private string FULLNAME;

        public string FULLNAME_
        { get => FULLNAME; set { FULLNAME = value; OnPropertyChanged("FULLNAME_"); } }

        private string AVATAR;

        public string AVATAR_
        { get => AVATAR; set { AVATAR = value; OnPropertyChanged("AVATAR_"); } }

        private string mail;

        public string Mail
        { get => mail; set { mail = value; OnPropertyChanged("Mail"); } }

        private string phone;

        public string Phone
        { get => phone; set { phone = value; OnPropertyChanged("Phone"); } }

        private string userName;

        public string UserName
        { get => userName; set { userName = value; OnPropertyChanged("UserName"); } }

        private string address;

        public string Address
        { get => address; set { address = value; OnPropertyChanged("Address"); } }

        public AccountViewModel()
        {
            UploadImageCommand = new RelayCommand<AccountUC>((parameter) => true, (parameter) => UploadImage(parameter));
            ChangeInfoCommand = new RelayCommand<AccountUC>((parameter) => true, (paramater) => ChangeInfo(paramater));
            LoadedCommand = new RelayCommand<AccountUC>((parameter) => true, (paramater) => loaded(paramater));
        }

        private void loaded(AccountUC paramater)
        {
            user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            AVATAR_ = user.AVATAR_;
            FULLNAME_ = user.FULLNAME_;
            Phone = user.PHONE_;
            UserName = user.USERNAME_;
            Mail = user.EMAIL_;
            Address = user.ADDRESS_;
        }

        public void UploadImage(AccountUC accountUC)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image files | *.jpg; *.png | All files | *.*";

            //Create connection to Storage

            string containerName = "avatar";
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=phong;AccountKey=/4FmL2uepULrqhajPWW1odbS70e5L/SEYVyO7ej3Zyzgh5cw61MysAf+f73I3euXcATYPUi8nJHQ0la8XB7Ccg==;EndpointSuffix=core.windows.net";
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            //Update Image

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get name of Image

                string[] filename = Path.GetFileName(openFileDialog.FileName).Split('.');

                //Delete old Image

                BlobClient blobClient = new BlobClient(connectionString, containerName, CurrentAccount.Username + "." + AVATAR_.Split('.')[5]);
                blobClient.Delete();

                //Start upload

                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName)))
                {
                    //Upload new Image

                    containerClient.UploadBlob(CurrentAccount.Username + "." + filename[1], stream);
                    CustomMessageBox.Show("Thay đổi ảnh thành công", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }

                //Update new Image link

                USER user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
                user.AVATAR_ = "https://phong.blob.core.windows.net/avatar/" + CurrentAccount.Username + "." + filename[1];

                //Save database change

                Data.Ins.DB.SaveChanges();

                //Load new image

                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bitmap.EndInit();
                accountUC.ImgBrush.ImageSource = bitmap;
            }
        }

        public void ChangeInfo(AccountUC accountUC)
        {
            ChangeInformationWindow changeInformationWindow = new ChangeInformationWindow();
            changeInformationWindow.ShowDialog();

            USER user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            AVATAR_ = user.AVATAR_;
            FULLNAME_ = user.FULLNAME_;
            Phone = user.PHONE_;
            UserName = user.USERNAME_;
            Mail = user.EMAIL_;
            Address = user.ADDRESS_;
        }
    }
}
