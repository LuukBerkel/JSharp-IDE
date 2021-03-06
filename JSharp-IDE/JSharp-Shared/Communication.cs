using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace JSharp_Shared
{
    class Communications
    {
        public static void WriteData(byte[] data, NetworkStream stream)
        {
            byte[] payload = data;
            byte[] lenght = new byte[4];
            lenght = BitConverter.GetBytes(data.Length);
            byte[] final = Combine(lenght, payload);

            //Debug print of data that is send
            stream.Write(final, 0, data.Length + 4);
            stream.Flush();
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        public static byte[] ReadData(NetworkStream stream)
        {
            // 4 bytes length == 32 bits, always positive unsigned
            byte[] lengthArray = new byte[4];
            stream.Read(lengthArray, 0, 4);
            int length = BitConverter.ToInt32(lengthArray, 0);

            byte[] buffer = new byte[length];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < length)
            {
                int read = stream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
            }

            return buffer;
        }
    }
}
