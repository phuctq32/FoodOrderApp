using FoodOrderApp.Views;
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
    internal class UserChatViewModel : BaseViewModel    
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand SendCommand { get; set; }

        public string message;
        public Socket client;
        UserChatWindow userChatWindow;
        public UserChatViewModel()
        {
            LoadedCommand = new RelayCommand<UserChatWindow>((parameter) => true, (parameter) => Load(parameter));
            SendCommand = new RelayCommand<UserChatWindow>((parameter) => true, (parameter) => Send(parameter));
        }
        private void Load(UserChatWindow parameter)
        {
            parameter.listViewChat.ItemsSource = Message.Ins.ms.MESSAGE_.Where(x=>x.USERNAME_ == CurrentAccount.Username).ToList();
            //tự động scroll xuống thằng tin nhắn mới nhất
            if (parameter.listViewChat.Items.Count - 1 > 0)
                (parameter.listViewChat.Items.GetItemAt(parameter.listViewChat.Items.Count - 1) as ListViewItem).Focus();
            userChatWindow = parameter;
            Connect();
            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }
        private void Send(UserChatWindow parameter)
        {
            if (!string.IsNullOrEmpty(parameter.messageTxt.Text))
            {
                if (client.Connected)
                {
                    message = parameter.messageTxt.Text;
                    byte[] snd = Serialize(message);
                    client.Send(snd, 0, snd.Length, SocketFlags.None);

                    MESSAGE_ sendMessage = new MESSAGE_();
                    sendMessage.DATE_ = DateTime.Now;
                    sendMessage.MESSAGE_DATA = message;
                    sendMessage.TYPE_ = "Sender";
                    sendMessage.ID = CurrentAccount.Username + Message.Ins.ms.MESSAGE_.Where(x => x.USERNAME_ == CurrentAccount.Username).Count().ToString();

                    parameter.listViewChat.Items.Add(sendMessage);
                    Message.Ins.ms.MESSAGE_.Add(sendMessage);
                    Message.Ins.ms.SaveChanges();
                }
                else
                {
                    CustomMessageBox.Show("Không thể kết nối đến máy chủ", System.Windows.MessageBoxButton.OK);
                }
            }
        }
        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[2048];
                    message = (string)Deserialize(data);

                    MESSAGE_ receiveMessage = new MESSAGE_();
                    receiveMessage.DATE_ = DateTime.Now;
                    receiveMessage.MESSAGE_DATA = message;
                    receiveMessage.TYPE_ = "Receiver";
                    receiveMessage.ID = CurrentAccount.Username + Message.Ins.ms.MESSAGE_.Where(x => x.USERNAME_ == CurrentAccount.Username).Count().ToString();

                    userChatWindow.listViewChat.Items.Add(receiveMessage);

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
            MemoryStream memoryStream = new MemoryStream(data);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return binaryFormatter.Deserialize(memoryStream);
        }
        public void Connect()
        {
            USER uSER = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();

            // Set the TcpClient on port 13000.
            Int32 port = 13000;
            IPAddress clientAddr = IPAddress.Parse(uSER.IP);

            USER admin = Data.Ins.DB.USERS.Where(x => x.TYPE_ == "admin").SingleOrDefault();
            IPAddress serverAddr = IPAddress.Parse(admin.IP);
            IPEndPoint iPEndPoint = new IPEndPoint(serverAddr, port);

            // TcpListener server = new TcpListener(port);

            client = new Socket(SocketType.Stream, ProtocolType.Tcp);

            // Start listening for client requests.
            client.Connect(serverAddr, port);

        }

    }
}
