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
        public User UserAcount;
       

        public Session(TcpClient client, Manager manager)
        {
            //this.sender = new EncryptedSender(client.GetStream());
            this.sender = new PlaneTextSender(client.GetStream());
            this.replyer = new Replyer(sender);
            this.interpreter = new Interpreter(manager, replyer, this);
            this.interpreter.Event += (s, e) => this.UserAcount = e;
        }

        public void StartSession()
        {
            new Thread(() =>
            {
                while (true)
                {
                    string encoded = this.sender.ReadMessage();
                    JObject decoded = (JObject)JsonConvert.DeserializeObject(encoded);
                    this.interpreter.Command(decoded);
                }
            }).Start();
        }
    }
}
