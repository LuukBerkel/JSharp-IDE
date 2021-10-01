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
            this.users = new List<User>();

           

        }
    }
}
