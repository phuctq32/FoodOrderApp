using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MenuViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public List<PRODUCT> Products { get; set; }

        public MenuViewModel()
        {
            
            LoadedCommand = new RelayCommand<MenuUC>(p => p == null ? false : true, p => Loaded(p));
        }

        private void Loaded(MenuUC menu)
        {
            Products = Data.Ins.DB.PRODUCTs.ToList();
            menu.ViewListProducts.ItemsSource = Products;
        }
    }
}