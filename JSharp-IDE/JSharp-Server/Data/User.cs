using JSharp_Server.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{

    public class User : ObservableObject
    {
        public string Username { get; set; }
        public string Password { get; set; }



        private Session Current;
        public Session GetSession() { return Current; }
        private void SetSession(Session s) { this.Current = s; }

        public User(string username, string password, bool hash)
        {
            Username = username;
            if (hash) Password = Proccessing.HashUserPassword(password);
            else Password = password;
        }
    }
}
