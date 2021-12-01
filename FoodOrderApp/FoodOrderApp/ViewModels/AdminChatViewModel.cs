using FoodOrderApp.Views.UserControls.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class AdminChatViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }

        public AdminChatViewModel()
        {
            LoadedCommand = new RelayCommand<AdminChatWindow>((parameter) => true, (parameter) => Load(parameter));
        }

        private void Load(AdminChatWindow parameter)
        {
            //tự động scroll xuống thằng tin nhắn mới nhất
            (parameter.listViewChat.Items.GetItemAt(parameter.listViewChat.Items.Count - 1) as ListViewItem).Focus();
        }
    }
}