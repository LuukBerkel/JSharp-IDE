using CommClass;
using JSharp_Server.Data;
using JSharp_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSharp_Server.Comms
{
    public class Session
    {
        private ISender sender;
        private Interpreter interpreter;
        private Replyer replyer;
        private TcpClient tcpClient;
        private Manager management;

        public User UserAcount;
       

        public Session(TcpClient client, Manager manager)
        {
            //this.sender = new EncryptedSender(client.GetStream());
            this.sender = new PlaneTextSender(client.GetStream());
            this.tcpClient = client;
            this.management = manager;
            this.replyer = new Replyer(sender);
            this.interpreter = new Interpreter(manager, replyer, this);
            this.interpreter.Event += (s, e) => this.UserAcount = e;

            //Debug output
            MainWindow.SetDebugOutput($"{client.Client.RemoteEndPoint} has connected");
        }

        public void StartSession()
        {
            new Thread(() =>
            {
                //Loop for reading
                while (true)
                {
                    //Trying to read from client
                    try
                    {
                        //Parsing data
                        string encoded = this.sender.ReadMessage();
                        JObject decoded = (JObject)JsonConvert.DeserializeObject(encoded);

                        if (decoded != null)
                        {

                            //Debug output
                            MainWindow.SetDebugOutput(encoded);

                            //Decoding data
                            this.interpreter.Command(decoded);
                        } else
                        {
                            throw new Exception();
                        }
                    }

                    //If connection is broken then...
                    catch (Exception)
                    {
                        //Debug outpot
                        MainWindow.SetDebugOutput("Connection error occured...");

                        //Clossing connection
                        this.tcpClient.Close();
                        this.management.Disconnect(this);

                        //Closing thread
                        break;
                    }
                }
            }).Start();
        }

        private Exception Exception()
        {
            throw new NotImplementedException();
        }
    }
}
