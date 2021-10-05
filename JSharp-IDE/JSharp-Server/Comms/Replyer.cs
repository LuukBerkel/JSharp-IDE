using JSharp_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Comms
{
    class Replyer
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
                command = "OK",
            };
            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        public void Failed()
        {
            object o = new
            {
                command = "FAILED",
            };
            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }


    }
}
