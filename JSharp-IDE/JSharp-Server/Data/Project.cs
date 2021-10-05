using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    public class Project : ObservableObject
    {
        private IDictionary<string, string> data;
        private IList<User> users;
        public User owner { get; set; }
        public string date { get; set; }
        public string name { get; set; }


        public Project(IDictionary<string, string> data, IList<User> users, User owner, string name)
        {
            this.data = data;
            this.users = users;
            this.owner = owner;
            this.name = name;
            this.date = DateTime.Now.ToString();
        }

        public void AddUser(User user)
        {
            if (!this.users.Contains(user)) this.users.Add(user);
        }
        public void RemoveUser(User user)
        {
            if (this.users.Contains(user)) this.users.Remove(user);
        }

        public void AddFile(string path, string data)
        {
            if (this.data.ContainsKey(path)) this.data[path] = data;
            else this.data.Add(path, data);
        }

        public void RemoveFile(string path)
        {
            if (this.data.ContainsKey(path))
            {
                //this.data.Add();
            }
        }



    }
}
