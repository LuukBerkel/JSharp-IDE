using CommClass;
using JSharp_Server.Data;
using JSharp_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        protected Manager manager;

        public Session(TcpClient client, Manager manager)
        {
            this.sender = new EncryptedSender(client.GetStream());
            this.manager = manager;
            this.interpreter = new Interpreter(manager);
        }

        public void StartSession()
        {
            new Thread(() =>
            {
                for (;;)
                {
                    string encoded = this.sender.ReadMessage();
                    JObject decoded = (JObject)JsonConvert.DeserializeObject(encoded);

                    this.interpreter.Command(decoded);
                }
            }).Start();
        }

        public void NotifySession(JObject json)
        {
            this.sender.SendMessage(JsonConvert.SerializeObject(json));
        }


    }
}
