using System;
using System.Collections.Generic;
using System.Text;

namespace JSharp_Shared
{
    interface ISender
    {
        public void SendMessage(string message);
        public string ReadMessage();

    }
}
