using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    class Manager
    {
        private List<Project> projects;
        private List<User> users;

        public Manager()
        {
            this.projects = new List<Project>();
            this.users = Proccessing.LoadUserData();
        }

        public bool CheckUser( string username, string password)
        {
            foreach(User user in this.users) 
            {
                if (user.Password == Proccessing.HashUserPassword(password) && user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddUser(string username, string password)
        {
            foreach (User user in this.users)
            {
                if (user.Username == username)
                {
                    return false;
                }
            }

            this.users.Add(new User(username, password, true));
            return true;

        }
    }
}
