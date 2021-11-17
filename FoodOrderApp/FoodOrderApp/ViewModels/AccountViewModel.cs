using FoodOrderApp;
using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FoodOrderApp.ViewModels
{
    class AccountViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }

        private string FULLNAME;

        public string FULLNAME_
        { get => FULLNAME; set { FULLNAME = value; OnPropertyChanged(); } }

        private string AVATAR;

        public string AVATAR_
        { get => AVATAR; set { AVATAR = value; OnPropertyChanged(); } }

        private string mail;

        public string Mail
        { get => mail; set { mail = value; OnPropertyChanged(); } }

        private string phone;

        public string Phone
        { get => phone; set { phone = value; OnPropertyChanged(); } }

        private string userName;

        public string UserName
        { get => userName; set { userName = value; OnPropertyChanged(); } }

        private string address;

        public string Address
        { get => address; set { address = value; OnPropertyChanged(); } }

        public AccountViewModel()
        {
            LoadedCommand = new RelayCommand<AccountUC>((parameter) => true, (parameter) => Loaded(parameter));
            UploadImageCommand = new RelayCommand<AccountUC>((parameter) => true, (parameter) => UploadImage(parameter));
        }
        public void Loaded(AccountUC accountUC)
        {
            USER user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            AVATAR_ = user.AVATAR_;
            FULLNAME_ = user.FULLNAME_;
            Phone = user.PHONE_;
            UserName = user.USERNAME_;
            Mail = user.EMAIL_;
            Address = user.ADDRESS_;
            accountUC.btnSelectImage.Command = UploadImageCommand;
            accountUC.btnSelectImage.CommandParameter = accountUC; 
        }
        public void UploadImage(AccountUC accountUC)
        {
            OpenFileDialog a = new OpenFileDialog();
            a.ShowDialog();
            string containerName = "ordercloud";
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=orderappcloud;AccountKey=JNOryWgw9qYsh7LwMAzeqrl7YdZdko9BvwlcANzs49ghHvn1WYw1vZAUYtue5L/wyOyG237Ya5QDuv4E9tMqeA==;EndpointSuffix=core.windows.net";
            //// Create a BlobServiceClient object which will be used to create a container client
            //    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //    // Create the container and return a container client object
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(a.FileName)))
            {
                containerClient.UploadBlob("20520442", stream);
                MessageBox.Show("uploaded");
            }
        }
    }
}
