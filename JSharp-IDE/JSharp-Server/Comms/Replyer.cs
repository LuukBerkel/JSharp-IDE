using JSharp_Server.Data;
using JSharp_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Comms
{
    public class Replyer
    {
        private ISender sender;

        public Replyer(ISender sender)
        {
            this.sender = sender;
        }

        public void Succes()
        {
            object o = new
            {
                instruction = "OK",
            };
            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public void Failed()
        {
            object o = new
            {
                instruction = "FAILED",
            };
            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public void SendAll(IDictionary<string, string> project, bool state)
        {
            

        }

        public void SendUpdate(IDictionary<string, string> project. bool state)
        {

        }

        public void SendExit(Project p)
        {

        }
    }
}
