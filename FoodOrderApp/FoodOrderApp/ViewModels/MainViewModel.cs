using FoodOrderApp.Models;
using FoodOrderApp.Views;
using FoodOrderApp.Views.UserControls;
using FoodOrderApp.Views.UserControls.Admin;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

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

        public MainViewModel()
        {
            LoadedCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Loaded(parameter));
            CloseWindowCommand = CloseWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p =>
            {
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
            LogOutCommand = new RelayCommand<MainWindow>(p => true, (p) => LogOut(p));
        }

        private void Loaded(MainWindow mainWindow)
        {
            if (CurrentAccount.IsUser)
            {
                mainWindow.listViewMenu.SelectedIndex = 0;
                mainWindow.ucWindow.Children.Add(new MenuUC());
            }
            else
            {
                mainWindow.listViewMenu.SelectedIndex = 5;
                mainWindow.ucWindow.Children.Add(new DashBoardUC());
            }
            USER user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            AVATAR_ = user.AVATAR_;
            FULLNAME_ = user.FULLNAME_;
            Phone = user.PHONE_;
            UserName = user.USERNAME_;
            Mail = user.EMAIL_;
            Address = user.ADDRESS_;
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
                    mainWindow.ucWindow.Children.Add(new MyOrderUC());
                    break;

                case 3:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new AccountUC());
                    break;

                case 4:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new ContactUC());
                    break;

                case 5:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new DashBoardUC());
                    break;

                case 6:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new EditProductUC());
                    break;

                case 7:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new OrderManagementUC());
                    break;

                case 8:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new AccountUC());
                    break;
            }
        }

        private void LogOut(MainWindow mainWindow)
        {
            if (CustomMessageBox.Show("Bạn có muốn đăng xuất?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                mainWindow.Close();
            }
        }
    }
}