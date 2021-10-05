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

namespace JSharp_IDE.Network
{
    public class Connection
    {
        public static Connection Instance;

        private ISender sender;
        private TcpClient tcpClient;

        public Connection(string ip, int port)
        {
            this.tcpClient = new TcpClient(ip, port);
            this.sender = new EncryptedSender(this.tcpClient.GetStream());
        }

        public void SendCommand(object o)
        {
            this.sender.SendMessage(JsonConvert.SerializeObject(o));
            Debug.WriteLine($"Connection: Sent to server: {JsonConvert.SerializeObject(o)}");
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
