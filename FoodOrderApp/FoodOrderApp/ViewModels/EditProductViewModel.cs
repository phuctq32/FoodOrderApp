using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls.Admin;
using FoodOrderApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Forms;

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

        private string Image;

        public string IMAGE_
        { get => Image; set { Image = value; OnPropertyChanged("Image"); } }

        private string Name;

        public string NAME_
        { get => Name; set { Name = value; OnPropertyChanged("Name"); } }

        private string Price;

        public string PRICE_
        { get => Price; set { Price = value; OnPropertyChanged("Price"); } }

        private string Description;

        public string DESCRIPTION_
        { get => Description; set { Description = value; OnPropertyChanged("Description"); } }

        private string Discount;

        public string DISCOUNT_
        { get => Discount; set { Discount = value; OnPropertyChanged("Discount"); } }

        private PRODUCT Current_Product;

        public List<PRODUCT> pRODUCTs;
        public EditProductViewModel()
        {
            LoadedCommand = new RelayCommand<EditProductUC>((parameter) => true, (parameter) => Loaded(parameter));
            AddProductCommand = new RelayCommand<EditProductUC>((parameter) => true, (parameter) => Add(parameter));
            UpdateProductCommand = new RelayCommand<System.Windows.Controls.ListViewItem>((parameter) => true, (parameter) => Update(parameter));
            DeleteProductCommand = new RelayCommand<EditProductUC>((parameter) => true, (parameter) => Delete(parameter));
            SelectImageCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => SelectImage(parameter));
            UpdateButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => UpdateProduct(parameter));
            AddButtonCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => AddProduct(parameter));
        }
        public void Loaded(EditProductUC editProductUC)
        {
            pRODUCTs = Data.Ins.DB.PRODUCTs.ToList();
            editProductUC.ListView.ItemsSource = pRODUCTs;
        }
        public void Add(EditProductUC editProductUC)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.updatebtn.Visibility = Visibility.Collapsed;
            addProductWindow.ShowDialog();
        }
        public void Update(System.Windows.Controls.ListViewItem listViewItem)
        {
            PRODUCT pRODUCT = listViewItem.DataContext as PRODUCT;
            Current_Product = pRODUCT;
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.addbtn.Visibility = Visibility.Collapsed;
            addProductWindow.txtName.Text = pRODUCT.NAME_;
            addProductWindow.txtPrice.Text = pRODUCT.PRICE_.ToString();
            addProductWindow.txtDiscount.Text = (pRODUCT.DISCOUNT_ * 100).ToString();
            addProductWindow.txtDescription.Text = pRODUCT.DESCRIPTION_;
            System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(pRODUCT.IMAGE_, UriKind.Absolute);
            bitmap.EndInit();
            addProductWindow.Image.ImageSource = bitmap;
            addProductWindow.ShowDialog();
        }
        public void Delete(EditProductUC editProductUC)
        {

        }
        public void OpenImage(EditProductUC editProductUC)
        {

        }
        public void SelectImage(AddProductWindow addProductWindow)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bitmap.EndInit();
                addProductWindow.Image.ImageSource = bitmap;
                IMAGE_ = openFileDialog.FileName;
            }
        }
        public void UpdateProduct(AddProductWindow addProductWindow)
        {
            PRODUCT pRODUCT = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
            pRODUCT.NAME_ = addProductWindow.txtName.Text;
            pRODUCT.PRICE_ = Convert.ToInt32(addProductWindow.txtPrice.Text);
            pRODUCT.DESCRIPTION_ = addProductWindow.txtDescription.Text;
            pRODUCT.DISCOUNT_ = Convert.ToInt32(addProductWindow.txtDiscount.Text);
            Data.Ins.DB.SaveChanges();
        }
        public void AddProduct(AddProductWindow addProductWindow)
        {

        }
    } 
}
