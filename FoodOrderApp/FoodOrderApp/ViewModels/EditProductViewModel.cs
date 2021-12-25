using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls.Admin;
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
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.IO;

namespace FoodOrderApp.ViewModels
{
    internal class EditProductViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand UpdateProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand OpenImageCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand UpdateButtonCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        private string Image;

        public string IMAGE_
        { get => Image; set { Image = value; OnPropertyChanged("Image"); } }

        //private string Name;

        //public string NAME_
        //{ get => Name; set { Name = value; OnPropertyChanged("Name"); } }

        //private string Price;

        //public string PRICE_
        //{ get => Price; set { Price = value; OnPropertyChanged("Price"); } }

        //private string Description;

        //public string DESCRIPTION_
        //{ get => Description; set { Description = value; OnPropertyChanged("Description"); } }

        //private string Discount;

        //public string DISCOUNT_
        //{ get => Discount; set { Discount = value; OnPropertyChanged("Discount"); } }

        private PRODUCT Current_Product;

        private List<PRODUCT> products;

        public List<PRODUCT> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged("Products");
            }
        }

        private string containerName = "container";
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=foodorderapp1;AccountKey=i1GnOJc+VJJpoRe3l44AeH3uBiq3n+ZbFELlNJMyiZuyovi7RlmYA5bTNoUWGFvS6tUTURPGRfgRvkXlsiDm/Q==;EndpointSuffix=core.windows.net";

        public EditProductViewModel()
        {
            LoadedCommand = new RelayCommand<EditProductUC>((parameter) => true, (parameter) => Loaded(parameter));
            AddProductCommand = new RelayCommand<EditProductUC>((parameter) => true, (parameter) => Add(parameter));
            UpdateProductCommand = new RelayCommand<System.Windows.Controls.ListViewItem>((parameter) => true, (parameter) => Update(parameter));
            DeleteProductCommand = new RelayCommand<System.Windows.Controls.ListViewItem>((parameter) => true, (parameter) => Delete(parameter));
            SelectImageCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => SelectImage(parameter));
            UpdateButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => UpdateProduct(parameter));
            AddButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => AddProduct(parameter));
            CloseButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => CloseButton(parameter));
        }

        public void Loaded(EditProductUC editProductUC)
        {
            Products = Data.Ins.DB.PRODUCTs.ToList();
        }

        public void Add(EditProductUC editProductUC)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.updatebtn.Visibility = Visibility.Collapsed;
            Current_Product = new PRODUCT();
            List<PRODUCT> a = Data.Ins.DB.PRODUCTs.ToList();
            int i = 1;
            foreach (PRODUCT pRODUCT in a)
            {
                if (i < Convert.ToInt32(pRODUCT.ID_))
                {
                    i = Convert.ToInt32(pRODUCT.ID_);
                }
            }
            Current_Product.ID_ = (i + 1).ToString();
            addProductWindow.ShowDialog();
        }

        public void Update(System.Windows.Controls.ListViewItem listViewItem)
        {
            //Get current product

            PRODUCT pRODUCT = listViewItem.DataContext as PRODUCT;
            Current_Product = pRODUCT;

            //Open AddProductWindow with current product data

            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.addbtn.Visibility = Visibility.Collapsed;
            addProductWindow.txtName.Text = pRODUCT.NAME_;
            addProductWindow.txtPrice.Text = pRODUCT.PRICE_.ToString();
            addProductWindow.txtDiscount.Text = (pRODUCT.DISCOUNT_ * 100).ToString();
            addProductWindow.txtDescription.Text = pRODUCT.DESCRIPTION_;

            //Load current product image
            if (!string.IsNullOrEmpty(pRODUCT.IMAGE_))
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(pRODUCT.IMAGE_, UriKind.Absolute);
                bitmap.EndInit();
                addProductWindow.Image.ImageSource = bitmap;
            }
            addProductWindow.ShowDialog();
            Products = Data.Ins.DB.PRODUCTs.ToList();
        }

        public void Delete(System.Windows.Controls.ListViewItem listViewItem)
        {
            if (CustomMessageBox.Show("Tạm ngưng bán món ăn?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                PRODUCT pRODUCT = listViewItem.DataContext as PRODUCT;
                Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == pRODUCT.ID_).Single().ACTIVE_ = 0;
                Data.Ins.DB.SaveChanges();
                Products = Data.Ins.DB.PRODUCTs.ToList();
            }
        }

        private void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files | *.jpg; *.png | All files | *.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Create connection to Storage

                BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

                //Update Image

                //Get name of Image

                string[] filename = Path.GetFileName(openFileDialog.FileName).Split('.');

                //Delete old Image
                if (Current_Product != null)
                {
                    if (!string.IsNullOrEmpty(Current_Product.IMAGE_))
                    {
                        BlobClient blobClient = new BlobClient(connectionString, containerName, Current_Product.ID_ + "." + Current_Product.IMAGE_.Split('.')[5]);
                        blobClient.Delete();
                    }
                }
                //Start upload

                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName)))
                {
                    //Upload new Image

                    containerClient.UploadBlob(Current_Product.ID_ + "." + filename[1], stream);
                    CustomMessageBox.Show("Thay đổi ảnh thành công", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }

                //Update new Image link

                //PRODUCT product = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
                Current_Product.IMAGE_ = "https://foodorderapp1.blob.core.windows.net/container/" + Current_Product.ID_ + "." + filename[1];

                //Save database change

                Data.Ins.DB.SaveChanges();
                Products = Data.Ins.DB.PRODUCTs.ToList();
                IMAGE_ = openFileDialog.FileName;
            }
        }

        public void SelectImage(AddProductWindow addProductWindow)
        {
            UploadImage();
            if (!string.IsNullOrEmpty(IMAGE_))
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(IMAGE_, UriKind.Absolute);
                bitmap.EndInit();
                addProductWindow.Image.ImageSource = bitmap;
            }
        }

        public void UpdateProduct(AddProductWindow addProductWindow)
        {
            PRODUCT pRODUCT = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
            pRODUCT.NAME_ = addProductWindow.txtName.Text;
            pRODUCT.PRICE_ = Convert.ToInt32(addProductWindow.txtPrice.Text);
            pRODUCT.DESCRIPTION_ = addProductWindow.txtDescription.Text;
            if (!Regex.IsMatch(addProductWindow.txtDiscount.Text, @"^[0-9_]+$"))
            {
                addProductWindow.txtDiscount.Focus();
                CustomMessageBox.Show("Giá khuyến mãi không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                addProductWindow.txtDiscount.Text = "";
                return;
            }
            if (Convert.ToDecimal(addProductWindow.txtDiscount.Text) < 100 )
            {
                pRODUCT.DISCOUNT_ = Convert.ToDecimal(addProductWindow.txtDiscount.Text) / 100;
            }
            else
            {
                CustomMessageBox.Show("Khuyển mãi phải nhỏ hơn 100%!", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            if (!Regex.IsMatch(addProductWindow.txtPrice.Text, @"^[0-9_]+$"))
            {
                addProductWindow.txtPrice.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                addProductWindow.txtPrice.Text = "";
                return;
            }
            Data.Ins.DB.SaveChanges();
            addProductWindow.Close();
            IMAGE_ = "";
            Products = Data.Ins.DB.PRODUCTs.ToList();
        }

        public void AddProduct(AddProductWindow addProductWindow)
        {
            PRODUCT newProduct = new PRODUCT();
            newProduct = Current_Product;
            newProduct.DESCRIPTION_ = addProductWindow.txtDescription.Text;
            if (newProduct.DESCRIPTION_ != "")
            {
                newProduct.DISCOUNT_ = Convert.ToDecimal(addProductWindow.txtDiscount.Text) / 100;
            }
              if (!Regex.IsMatch(addProductWindow.txtDiscount.Text, @"^[0-9_]+$"))
            {
                addProductWindow.txtDiscount.Focus();
                CustomMessageBox.Show("Giá khuyến mãi không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                addProductWindow.txtDiscount.Text = "";
                return;
            }
            newProduct.NAME_ = addProductWindow.txtName.Text;
            newProduct.RATE_TIMES_ = 0;
            newProduct.RATING_ = 0;
            newProduct.ACTIVE_ = 1;
            newProduct.PRICE_ = Convert.ToInt32(addProductWindow.txtPrice.Text);
            if (!Regex.IsMatch(addProductWindow.txtPrice.Text, @"^[0-9_]+$"))
            {
                addProductWindow.txtPrice.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                addProductWindow.txtPrice.Text = "";
                return;
            }
            Data.Ins.DB.PRODUCTs.Add(newProduct);
            IMAGE_ = "";
            Data.Ins.DB.SaveChanges();
            Products = Data.Ins.DB.PRODUCTs.ToList();
            addProductWindow.Close();
        }

        public void CloseButton(AddProductWindow addProductWindow)
        {
            if (!string.IsNullOrEmpty(IMAGE_))
            {
                BlobClient blobClient = new BlobClient(connectionString, containerName, Current_Product.ID_ + "." + Current_Product.IMAGE_.Split('.')[5]);
                blobClient.Delete();
                IMAGE_ = "";
            }
            addProductWindow.Close();
        }
    }
}