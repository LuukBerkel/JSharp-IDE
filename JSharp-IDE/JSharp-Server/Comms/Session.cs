using CommClass;
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
    class Session
    {
        private TcpClient client;
        private ISender sender;

        public Session(TcpClient client)
        {
            this.client = client;
            this.sender = new EncryptedSender(client.GetStream());

        }

        public void StartSession()
        {
            new Thread(() =>
            {
                for (;;)
                {
                    string encoded = this.sender.ReadMessage();
                    JObject decoded = (JObject)JsonConvert.DeserializeObject(encoded);




                }
            }).Start();
        }


    }
}
