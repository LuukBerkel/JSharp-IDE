using CommClass;
using JSharp_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JSharp_IDE.Network
{
    public class Connection
    {
        public static Connection Instance;
        public static bool IsConnectedToServer = false;

        private ISender sender;
        private TcpClient tcpClient;

        public Connection(string ip, int port)
        {
            try
            {
                this.tcpClient = new TcpClient(ip, port);
                //this.sender = new EncryptedSender(this.tcpClient.GetStream());
                this.sender = new PlaneTextSender(this.tcpClient.GetStream());
                IsConnectedToServer = true;
            } catch (Exception)
            {
                MessageBox.Show("Could not connect to server!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
                IsConnectedToServer = false;
            }

            new Thread(() =>
            {
                while (true) ReadMessage();
            }).Start();
        }

        public void SendCommand(object o)
        {
            try
            {
                if (this.tcpClient != null)
                {
                    this.sender.SendMessage(JsonConvert.SerializeObject(o));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Could not send message to server!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
                IsConnectedToServer = false;
            }
        }

        public string ReadMessage()
        {
            string msg = this.sender.ReadMessage();
            Debug.WriteLine("Client received: " + msg);
            return msg;
        }

        public static Connection GetConnection(string ip, int port)
        {
            if (Instance == null)
            {
                Instance = new Connection(ip, port);
            }
            return Instance;
        }
    }
}
