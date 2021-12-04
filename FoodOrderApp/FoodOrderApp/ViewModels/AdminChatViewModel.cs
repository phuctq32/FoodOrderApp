﻿using FoodOrderApp.Views.UserControls.Admin;
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

        public string message = null;

        public TcpClient server;
        public AdminChatViewModel()
        {
            LoadedCommand = new RelayCommand<AdminChatWindow>((parameter) => true, (parameter) => Load(parameter));
            SendCommand = new RelayCommand<AdminChatWindow>((parameter) => true, (parameter) => Send(parameter));
        }

        private void Load(AdminChatWindow parameter)
        {
            //tự động scroll xuống thằng tin nhắn mới nhất
            if (parameter.listViewChat.Items.Count - 1 > 0)
                (parameter.listViewChat.Items.GetItemAt(parameter.listViewChat.Items.Count - 1) as ListViewItem).Focus();
            Connect();
            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }
        private void Send(AdminChatWindow parameter)
        {
            if(!string.IsNullOrEmpty(parameter.messageTxt.Text))
            {
                message = parameter.messageTxt.Text;
                byte[] snd = Serialize(message);
                NetworkStream networkStream = server.GetStream();
                networkStream.Write(snd, 0, snd.Length);
            }
        }
        private void Receive()
        {
            try
            {
                while(true)
                {
                    byte[] data = new byte[2048];
                    message = (string)Deserialize(data);
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
            NetworkStream networkStream = server.GetStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return binaryFormatter.Deserialize(networkStream);
        }
        public void Connect()
        {
            TcpListener server = null;
            USER uSER = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();

            // Set the TcpListener on port 13000.
            Int32 port = 13000;
            IPAddress serverAddr = IPAddress.Parse(uSER.IP);

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(serverAddr, port);

            // Start listening for client requests.
            server.Start();
            
        }
    }
}