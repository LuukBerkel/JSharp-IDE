using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_IDE.Network
{
    public struct File
    {
        public string filePath;
        public byte[] data;

        public File(string filePath, byte[] data)
        {
            this.filePath = filePath;
            this.data = data;
        }
    }
}
