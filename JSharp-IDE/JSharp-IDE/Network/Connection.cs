using CommClass;
using JSharp_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JSharp_IDE.Network
{
    public class Connection
    {
        public static Connection Instance;

        private ISender sender;
        private TcpClient tcpClient;

        public Connection(string ip, int port)
        {
            try
            {
                this.tcpClient = new TcpClient(ip, port);
                this.sender = new EncryptedSender(this.tcpClient.GetStream());
            } catch (Exception e)
            {
                MessageBox.Show("Could not connect to server!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SendCommand(object o)
        {
            try
            {
                Debug.WriteLine($"Connection: Sending to server: {JsonConvert.SerializeObject(o)}");
                if (this.tcpClient != null)
                {
                    Debug.WriteLine($"Connection: Sending to server: {JsonConvert.SerializeObject(o)}");
                    this.sender.SendMessage(JsonConvert.SerializeObject(o));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not send message to server!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
