using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Comms
{
    class Server
    {
        private TcpListener listener;

        public Server(IPAddress IP, int port)
        {
            this.listener = new TcpListener(IP, port);
        }

        public void Start()
        {
            this.listener.Start();
            this.listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
        }

        private void HandleClient(IAsyncResult ar)
        {
            //For the next client
            this.listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            
            //Handeling of the client
            Session session = new Session(this.listener.EndAcceptTcpClient(ar));
        }
    }
}
