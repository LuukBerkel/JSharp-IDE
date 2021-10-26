using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_IDE.Network
{
    /// <summary>
    /// Struct that is used to make the sharing of files a bit easier in the code.
    /// </summary>
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
