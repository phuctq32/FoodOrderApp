using FoodOrderApp.Models;
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
using System.Net.Cache;

namespace FoodOrderApp.ViewModels
{
    internal class AccountViewModel : MainViewModel
    {
        public ICommand UploadImageCommand { get; set; }
        public ICommand ChangeInfoCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        

        public AccountViewModel()
        {
            UploadImageCommand = new RelayCommand<AccountUC>((parameter) => true, (parameter) => UploadImage(parameter));
            ChangeInfoCommand = new RelayCommand<AccountUC>((parameter) => true, (paramater) => ChangeInfo(paramater));
            LoadCommand = new RelayCommand<AccountUC>((parameter) => true, (paramater) => loaded(paramater));
            ChangePasswordCommand = new RelayCommand<AccountUC>((parameter) => true, (parameter) => ChangePassword(parameter));
        }

        private void loaded(AccountUC paramater)
        {
            Phone = CurrentAccount.User.PHONE_;
            UserName = CurrentAccount.User.USERNAME_;
            Mail = CurrentAccount.User.EMAIL_;
            Address = CurrentAccount.User.ADDRESS_;
        }

        public void UploadImage(AccountUC accountUC)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image files | *.jpg; *.png | All files | *.*";

            //Create connection to Storage

            string containerName = "container";
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodorderapp1;AccountKey=i1GnOJc+VJJpoRe3l44AeH3uBiq3n+ZbFELlNJMyiZuyovi7RlmYA5bTNoUWGFvS6tUTURPGRfgRvkXlsiDm/Q==;EndpointSuffix=core.windows.net";
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            //Update Image

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get name of Image

                string[] filename = Path.GetFileName(openFileDialog.FileName).Split('.');

                //Delete old Image
                if(!string.IsNullOrEmpty(Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault().AVATAR_) && Data.Ins.DB.USERS.Where(x =>x.USERNAME_ == CurrentAccount.Username).SingleOrDefault().AVATAR_ != "https://foodorderapp1.blob.core.windows.net/container/default.png")
                { 
                    BlobClient blobClient = new BlobClient(connectionString, containerName, CurrentAccount.Username + "." + CurrentAccount.User.AVATAR_.Split('.')[5]);
                    blobClient.Delete();
                }

                Data.Ins.DB.SaveChanges();
                //Start upload
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName)))
                {
                    //Upload new Image
                    containerClient.UploadBlob(CurrentAccount.Username + "." + filename[1], stream);
                    CustomMessageBox.Show("Thay đổi ảnh thành công", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }

                //Update new Image link

                //USER user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
                //user.AVATAR_ = "https://foodorderapp1.blob.core.windows.net/container/" + CurrentAccount.Username + "." + filename[1];
                //AVATAR_ = "https://foodorderapp1.blob.core.windows.net/container/" + CurrentAccount.Username + "." + filename[1];
                //User = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
                //User.AVATAR_ = "";
                //User.AVATAR_ = AVATAR_;
                //Save database change

                //Load new image

                Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault().AVATAR_ = "https://foodorderapp1.blob.core.windows.net/container/"  + CurrentAccount.Username + "." + filename[1];
                CurrentAccount.User = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
                Data.Ins.DB.SaveChanges();

                Avatar = CurrentAccount.User.AVATAR_;


                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bitmap.EndInit();
                accountUC.ImgBrush.ImageSource = bitmap;
                //User = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
                //AVATAR_ = "https://foodorderapp1.blob.core.windows.net/container/" + CurrentAccount.Username + "." + filename[1];

            }
        }

        public void ChangeInfo(AccountUC accountUC)
        {
            ChangeInformationWindow changeInformationWindow = new ChangeInformationWindow();
            changeInformationWindow.txtFullname.Text = CurrentAccount.User.FULLNAME_;
            changeInformationWindow.txtMail.Text = CurrentAccount.User.EMAIL_;
            changeInformationWindow.txtAddress.Text = CurrentAccount.User.ADDRESS_;
            changeInformationWindow.txtPhone.Text = CurrentAccount.User.PHONE_;
            changeInformationWindow.ShowDialog();
            Fullname = CurrentAccount.User.FULLNAME_;
            Phone = CurrentAccount.User.PHONE_;
            UserName = CurrentAccount.User.USERNAME_;
            Mail = CurrentAccount.User.EMAIL_;
            Address = CurrentAccount.User.ADDRESS_;
        }

        private void ChangePassword(AccountUC paramter)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.lblSignUp.Content = "Đổi mật khẩu";
            forgotPasswordWindow.txtMail.Text = CurrentAccount.User.EMAIL_;
            forgotPasswordWindow.txtMail.IsEnabled = false;
            forgotPasswordWindow.ShowDialog();
        }
    }
}
