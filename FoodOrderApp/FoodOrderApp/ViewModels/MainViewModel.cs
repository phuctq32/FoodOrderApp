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

        public string Fullname { get => CurrentAccount.User.FULLNAME_; set { CurrentAccount.User.FULLNAME_ = value; OnPropertyChanged("Fullname"); } }
        public string Avatar { get => CurrentAccount.User.AVATAR_; set { CurrentAccount.User.AVATAR_ = value; OnPropertyChanged("Avatar"); } }

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

        public MainViewModel()
        {
            LoadedCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Loaded(parameter));
            CloseWindowCommand = CloseWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p =>
            {
                if (CustomMessageBox.Show("Thoát ứng dụng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    FrameworkElement window = ControlBarViewModel.GetParentWindow(p);
                    var w = window as Window;
                    try 
                    {
                        Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().FULLNAME_ = "Administrator";
                        Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().PHONE_ = "0123456789";
                        Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).Single().ADDRESS_ = "Khu phố 6, P.Linh Trung, Tp.Thủ Đức, Tp.Hồ Chí Minh";
                        Data.Ins.DB.SaveChanges();
                    }
                    catch 
                    {
                        CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
            //Avatar = CurrentAccount.User.AVATAR_;
            //Fullname = CurrentAccount.User.FULLNAME_;
            Phone = CurrentAccount.User.PHONE_;
            UserName = CurrentAccount.User.USERNAME_;
            Mail = CurrentAccount.User.EMAIL_;
            Address = CurrentAccount.User.ADDRESS_;
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