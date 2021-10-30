using JSharp_IDE.View;
using JSharp_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
using ToastNotifications.Messages;

namespace JSharp_IDE.Network
{
    public class Connection
    {
        public static Connection Instance;
        public static bool IsConnectedToServer = false;

        private ISender sender;
        private TcpClient tcpClient;
        private Thread readThread;

        private Queue<string> feedbackQueu;

        /// <summary>
        /// Delivers feedback to the users in the gui.
        /// </summary>
        /// <param name="errorMessage"></param>
        public void SetErrorQueu(string errorMessage)
        {
            feedbackQueu.Enqueue(errorMessage);
        }

        /// <summary>
        /// Connects with the server
        /// </summary>
        /// <param name="ip">IP-address</param>
        /// <param name="port">Portnumber</param>
        public Connection(string ip, int port)
        {
            feedbackQueu = new Queue<string>();
            //Trying to connect to the sever
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

            //Reading data form the server
            this.readThread = new Thread(() =>
            {
                while (IsConnectedToServer)
                {
                    ReadMessage();
                }
                if (this.tcpClient != null) this.tcpClient.Close();
                MessageBox.Show("Lost connection to the server!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
                IsConnectedToServer = false;
            });
            this.readThread.Start();
        }

        /// <summary>
        /// Ends the connection with the server.
        /// </summary>
        public void Stop()
        {
            IsConnectedToServer = false;
            //this.tcpClient.Close();
            this.readThread.Join();
            Debug.WriteLine("Closed connection with the server.");
        }

        /// <summary>
        /// Sends a command to the server.
        /// </summary>
        /// <param name="o"></param>
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

        /// <summary>
        /// Reads a string from the server.
        /// </summary>
        /// <returns>String that is read from the server</returns>
        public string ReadMessage()
        {
            string msg = "";
            try
            {
                if (IsConnectedToServer)
                {
                    msg = this.sender.ReadMessage();
                    Command((JObject)JsonConvert.DeserializeObject(msg));
                }
            } catch (Exception)
            {
                MessageBox.Show("Received invalid data!", "JSharp IDE", MessageBoxButton.OK, MessageBoxImage.Error);
                IsConnectedToServer = false;
            }
            return msg;
        }

        /// <summary>
        /// Method to get access to the connection instance (singleton).
        /// </summary>
        /// <param name="ip">Ip of the server</param>
        /// <param name="port">Port of the server</param>
        /// <returns>An instance of connection.</returns>
        public static Connection GetConnection(string ip, int port)
        {
            //If this instance does not exist, or if the connection is lost: Create a new connection
            if (Instance == null || !IsConnectedToServer)
            {
                Instance = new Connection(ip, port);
            }
            return Instance;
        }

        /// <summary>
        /// Check which json command needs to be executed.
        /// </summary>
        /// <param name="json">The json with a command in it.</param>
        private void Command(JObject json)
        {
            JToken token;
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
            } else
            {
                IsConnectedToServer = false;
            }
        }

        /// <summary>
        /// Confirmation command
        /// </summary>
        /// <param name="json">Json from the server</param>
        [Command("OK")]
        private void Ok(JObject json)
        {
            //Deques it because it is ok
            if (this.feedbackQueu.Count > 0)
            this.feedbackQueu.Dequeue();
            Debug.WriteLine("Action successfull");
        }

        /// <summary>
        /// Failed command
        /// </summary>
        /// <param name="json">Json from the server</param>
        [Command("FAILED")]
        private void Failed(JObject json)
        {
            //Show the dequed message because an error
            if (this.feedbackQueu.Count > 0)
                MessageBox.Show(this.feedbackQueu.Dequeue(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Debug.WriteLine("Action failed");
        }

        /// <summary>
        /// This command downloads a project from the server.
        /// </summary>
        /// <param name="json">Json from the server</param>
        [Command("RequestedProject")]
        private void RequestedProject(JObject json)
        {
            JToken token = json.SelectToken("data.files");
            JArray array = JArray.FromObject(token);
                //Add/Edit files
                foreach (JObject file in array)
                {
                    Project.UpdateFile(file.SelectToken("filePath").ToString(), file.SelectToken("data").ToString());
                }

                Project.UpdateTreeView(Project.ProjectDirectory);
            Debug.WriteLine("Updated file(s)");
            Debug.WriteLine("Received all project data");


            Application.Current.Dispatcher.Invoke(() => Project.notifier.ShowInformation("Project is now downloaded and ready for edit."));
        }

        /// <summary>
        /// Update one or more files from the project.
        /// </summary>
        /// <param name="json">Json from the server</param>
        [Command("UpdatedProject")]
        private void UpdatedProject(JObject json)
        {
            JToken token = json.SelectToken("data.files");
            JArray array = JArray.FromObject(token);
            if (json.SelectToken("data.flag").ToString() == "0")
            {
                //Remove files
                

            } else
            {
                //Add/Edit files
                foreach (JObject file in array)
                {
                    Project.UpdateFile(file.SelectToken("filePath").ToString(), file.SelectToken("data").ToString());
                }
            }
            Debug.WriteLine("Updated file(s)");
            MainWindow.CodePanels.Dispatcher.Invoke(() =>
            {
                MainWindow.CodePanels.UpdateLayout();
            });
        }

        /// <summary>
        /// This class is used as an attribute to connect a json command with a method.
        /// </summary>
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
