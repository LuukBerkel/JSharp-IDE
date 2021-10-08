using CommClass;
using JSharp_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
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
                while (MainWindow.Running && this.tcpClient.Connected)
                {
                    try
                    {
                        ReadMessage();
                    } catch (Exception)
                    {
                        this.tcpClient.Close();
                        MessageBox.Show("Lost connection to the server!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }

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
            try
            {
                Command((JObject)JsonConvert.DeserializeObject(msg));
            } catch (Exception)
            {
                MessageBox.Show("Received invalid data!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new SocketException();
            }
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


        private void Command(JObject json)
        {
            JToken? token;
            if (json.TryGetValue("instruction", out token))
            {
                string command = token.ToString();

                MethodInfo[] methods = typeof(Connection).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.ExactBinding);
                foreach (MethodInfo method in methods)
                {
                    if (method.GetCustomAttribute<CommandAttribute>() != null 
                        && method.GetCustomAttribute<CommandAttribute>().GetCommand() == command)
                    {
                        method.Invoke(this, new object[] { json });
                    }
                }
            }
        }

        [Command("OK")]
        private void Ok(JObject json)
        {
            Debug.WriteLine("Action successfull");
        }

        [Command("FAILED")]
        private void Failed(JObject json)
        {
            Debug.WriteLine("Action failed");
        }


        public class CommandAttribute : Attribute
        {
            private string Command;

            public CommandAttribute(string command)
            {
                Command = command;
            }

            public string GetCommand()
            {
                return this.Command;
            }
        }
    }
}
