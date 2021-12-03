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

namespace FoodOrderApp.ViewModels
{
    internal class AdminChatViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand SendCommand { get; set; }

        public string message = null;

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
            CreateConnection();
        }
        private void CreateConnection()
        {
            TcpListener server = null;
            USER uSER = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress serverAddr = IPAddress.Parse(uSER.IP);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(serverAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }

                // Shutdown and end connection
                client.Close();

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
        private void Send(AdminChatWindow parameter)
        {
            if(!string.IsNullOrEmpty(parameter.messageTxt.Text))
            {

            }
        }


        

        public void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    message = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        message += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (message.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", message);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(message);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
    }
}
