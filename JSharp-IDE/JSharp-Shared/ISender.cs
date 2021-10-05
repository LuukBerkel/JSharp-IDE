using System;
using System.Collections.Generic;
using System.Text;

namespace JSharp_Shared
{
    public interface ISender
    {
        public void SendMessage(string message);
        public string ReadMessage();
    }
}
