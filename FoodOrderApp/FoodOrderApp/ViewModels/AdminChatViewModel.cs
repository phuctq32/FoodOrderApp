using FoodOrderApp.Views.UserControls.Admin;
using FoodOrderApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace FoodOrderApp.ViewModels
{
    internal class AdminChatViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand SelectionChangeCommand { get; set; }

        public string message = null;

        public TcpListener server;
        public TcpClient client;
        AdminChatWindow adminChatWindow;
        List<USER> uSERs;
        public List<USER> User
        {
            get => uSERs;
            set
            {
                uSERs = value;
                OnPropertyChanged("User");
            }
        }
        public AdminChatViewModel()
        {
            LoadedCommand = new RelayCommand<AdminChatWindow>((parameter) => true, (parameter) => Load(parameter));
            SendCommand = new RelayCommand<AdminChatWindow>((parameter) => true, (parameter) => Send(parameter));
            SelectionChangeCommand = new RelayCommand<ListViewItem>((parameter) => true, (parameter) => SelectionChange(parameter));
        }
        private void Load(AdminChatWindow parameter)
        {
            uSERs = Data.Ins.DB.USERS.Where(x => x.TYPE_ == "user").ToList();
            parameter.listviewUser.ItemsSource = uSERs;
            //tự động scroll xuống thằng tin nhắn mới nhất
            if (parameter.listViewChat.Items.Count - 1 > 0)
                (parameter.listViewChat.Items.GetItemAt(parameter.listViewChat.Items.Count - 1) as ListViewItem).Focus();
            adminChatWindow = parameter;
            Connect();
            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }
        private void Send(AdminChatWindow parameter)
        {
            if (!string.IsNullOrEmpty(parameter.messageTxt.Text))
            {
                if (client != null)
                {
                    message = parameter.messageTxt.Text;
                    byte[] snd = Serialize(message);
                    NetworkStream networkStream = client.GetStream();
                    networkStream.Write(snd, 0, snd.Length);
                    MESSAGE_ sendMessage = new MESSAGE_();
                    sendMessage.ID = CurrentAccount.Username + Message.Ins.ms.MESSAGE_.Where(x => x.USERNAME_ == CurrentAccount.Username).Count().ToString();
                    sendMessage.MESSAGE_DATA = message;
                    sendMessage.TYPE_ = "Sender";
                    sendMessage.DATE_ = DateTime.Now;
                    parameter.listViewChat.Items.Add(sendMessage);
                    Message.Ins.ms.MESSAGE_.Add(sendMessage);
                    Message.Ins.ms.SaveChanges();
                }
                else
                {
                    CustomMessageBox.Show("Người dùng hiện không có mặt hoặc lỗi kết nối", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }
        private void Receive()
        {
            try
            {
                while(true)
                {
                    client = server.AcceptTcpClient();
                    byte[] data = new byte[2048];
                    message = (string)Deserialize(data);

                    MESSAGE_ receiveMessage = new MESSAGE_();
                    receiveMessage.MESSAGE_DATA = message;
                    receiveMessage.ID = CurrentAccount.Username + Message.Ins.ms.MESSAGE_.Where(x => x.USERNAME_ == CurrentAccount.Username).Count().ToString();
                    receiveMessage.TYPE_ = "Receiver";
                    receiveMessage.DATE_ = DateTime.Now;

                    adminChatWindow.listViewChat.Items.Add(receiveMessage);

                    Message.Ins.ms.MESSAGE_.Add(receiveMessage);
                    Message.Ins.ms.SaveChanges();
                }
            }
            catch
            {

            }
        }
        private byte[] Serialize(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
        private object Deserialize(byte[] data)
        {
            NetworkStream networkStream = client.GetStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return binaryFormatter.Deserialize(networkStream);
        }
        public void Connect()
        {
            USER uSER = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();

            // Set the TcpListener on port 13000.
            Int32 port = 13000;

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(IPAddress.Any, port);

            // Start listening for client requests.
            server.Start();
            
        }
        public void SelectionChange(ListViewItem listViewItem)
        {
            USER user = listViewItem.DataContext as USER;
            adminChatWindow.listViewChat.ItemsSource = Message.Ins.ms.MESSAGE_.Where(x => x.USERNAME_ == user.USERNAME_).ToList();
            listViewItem.IsSelected = true;

        }
    }
}
