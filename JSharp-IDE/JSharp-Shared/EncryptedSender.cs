using JSharp_Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace CommClass
{
    class EncryptedSender : ISender
    {
        private RSACryptoServiceProvider RSAIN;
        private RSACryptoServiceProvider RSAOUT;

        private NetworkStream stream;

        public EncryptedSender(NetworkStream stream)
        {
            //Setting up rsa objects
            this.RSAIN = new RSACryptoServiceProvider();
            this.RSAOUT = new RSACryptoServiceProvider();

            //Sending the correct keys
            stream.Write(RSAIN.ExportRSAPublicKey());

            //Receiving the correct keys
            byte[] publicKeyServer = new byte[140];
            int bytesRead = 0;
            stream.Read(publicKeyServer);
            RSAOUT.ImportRSAPublicKey(publicKeyServer, out bytesRead);

            //Saving the stream
            this.stream = stream;
        }

        public void SendMessage(string message)
        {
            Communications.WriteData(this.RSAOUT.Encrypt(Encoding.ASCII.GetBytes(message), false), stream);
        }

        public string ReadMessage()
        {
            return Encoding.ASCII.GetString(this.RSAIN.Decrypt(Communications.ReadData(stream), false));
        }

    }
}
