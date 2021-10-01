using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password, bool hash)
        {
            Username = username;
            if (hash) Password = ;
            else Password = password;
        }
    }
}
