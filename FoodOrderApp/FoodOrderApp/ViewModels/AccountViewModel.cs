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
using System.Windows.Forms;
using Microsoft.Win32;
using System.Media;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace FoodOrderApp.ViewModels
{
    class AccountViewModel : BaseViewModel
    {
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
            UploadImageCommand = new RelayCommand<AccountUC>((parameter) => true, (parameter) => UploadImage(parameter));

            //Load current account information

            USER user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
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

                //Load new image

                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bitmap.EndInit();
                accountUC.ImgBrush.ImageSource = bitmap;

                //Save database change

                Data.Ins.DB.SaveChanges();
                
            }
        }
    }
}
