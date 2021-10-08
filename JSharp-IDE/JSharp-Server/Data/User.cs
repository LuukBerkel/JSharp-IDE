using JSharp_Server.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    public class User
    {
        /// <summary>
        /// User credentails
        /// </summary>
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Constructor for a user
        /// </summary>
        /// <param name="username">The username of the user account</param>
        /// <param name="password">The password of the user account </param>
        /// <param name="hash">Is a boolean that hashes the password if enabled</param>
        public User(string username, string password, bool hash)
        {
            Username = username;
            if (hash) Password = Proccessing.HashUserPassword(password);
            else Password = password;
        }
    }
}
