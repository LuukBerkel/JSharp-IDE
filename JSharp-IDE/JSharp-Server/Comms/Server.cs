using JSharp_Server.Data;
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
        private Manager manager;

        public Server(IPAddress IP, int port)
        {
            this.listener = new TcpListener(IP, port);
            this.manager = new Manager();
            MainWindow.SetDebugOutput($"Server set {IP} at {port}");
        }

        /// <summary>
        /// Start the tcplistener and begin accepting clients.
        /// </summary>
        public void Start()
        {
            this.listener.Start();
            this.listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            MainWindow.SetDebugOutput($"Server has started.");
        }

        /// <summary>
        /// Called when a client is connected.
        /// </summary>
        /// <param name="ar"></param>
        private void HandleClient(IAsyncResult ar)
        {
            try
            {
                //Handeling of the client
                Session session = new Session(this.listener.EndAcceptTcpClient(ar), manager);
                session.StartSession();
                this.manager.AddSession(session);

                //For the next client
                this.listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            }
            catch (Exception e)
            {
                //Weird errror occured
                MainWindow.SetDebugOutput(e.Message);
                this.listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
            }
        }
    }
}
