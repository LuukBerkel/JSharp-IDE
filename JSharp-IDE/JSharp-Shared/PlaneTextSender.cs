﻿using JSharp_Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommClass
{
    class PlaneTextSender : ISender
    {

        private NetworkStream stream;

        public PlaneTextSender(NetworkStream stream)
        {
            this.stream = stream;
        }

        public void SendMessage(string message)
        {
            Communications.WriteData(Encoding.ASCII.GetBytes(message), stream);
        }

        public string ReadMessage()
        {
            return Encoding.ASCII.GetString(Communications.ReadData(stream));
        }


    }
}
