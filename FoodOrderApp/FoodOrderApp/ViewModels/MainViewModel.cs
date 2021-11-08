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

        public MainViewModel()
        {
            LoadedCommand = new RelayCommand<ControlBarUC>(parameter => true, parameter => Loaded(parameter));
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
        }

        private void Loaded(ControlBarUC cb)
        {
            cb.closeBtn.Command = CloseWindowCommand;
            cb.closeBtn.CommandParameter = cb;
        }
    }
}
