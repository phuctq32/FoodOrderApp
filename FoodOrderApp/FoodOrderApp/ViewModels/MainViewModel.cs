using FoodOrderApp;
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

namespace FoodOrderApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }

        public MainViewModel()
        {
            LoadedCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Loaded(parameter));
            CloseWindowCommand = CloseWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p => {
                if (CustomMessageBox.Show("Thoát ứng dụng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    FrameworkElement window = ControlBarViewModel.GetParentWindow(p);
                    var w = window as Window;

                    if (w != null)
                    {
                        w.Close();
                    }
                }
            });
            SwitchTabCommand = new RelayCommand<MainWindow>(p => true, (p) => SwitchTab(p));
        }

        private void Loaded(MainWindow mainWindow)
        {
            if(CurrentAccount.IsUser)
            {
                mainWindow.ucWindow.Children.Add(new MenuUC());
            }
            
            
            mainWindow.controlBar.closeBtn.Command = CloseWindowCommand;
            mainWindow.controlBar.closeBtn.CommandParameter = mainWindow.controlBar;
        }

        private void SwitchTab(MainWindow mainWindow) 
        {
            switch (mainWindow.listViewMenu.SelectedIndex)
            {
                case 0:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new MenuUC());
                    break;
                case 1:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new CartUC());
                    break;
                case 2:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new AccountUC());
                    break;
                case 3:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new ContactUC());
                    break;
            }
        }
    }
}
