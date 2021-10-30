using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Shared
{
    public struct NetworkFile
    {
        public string filePath;
        public byte[] data;

        public NetworkFile(string filePath, byte[] data)
        {
            this.filePath = filePath;
            this.data = data;
        }
    }
}
