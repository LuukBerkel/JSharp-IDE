using JSharp_Server.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    public class Project
    {
        //Actual data
        //Key: path, value: data
        private IDictionary<string, string> data;
        private IList<Session> activeUsers;

        //Logistics
        private IList<string> users;
        public User owner { get; set; }
        public string date { get; set; }
        public string name { get; set; }

        /// <summary>
        /// The constructor for a project
        /// </summary>
        /// <param name="data">This the project files in dictionary form</param>
        /// <param name="users">These are the users that are invited in list form</param>
        /// <param name="owner">This is the user that makes the project</param>
        /// <param name="name">This is the name of the project..</param>
        public Project(IDictionary<string, string> data, IList<string> users, User owner, string name)
        {
            this.data = data;
            this.users = users;
            this.owner = owner;
            this.name = name;
            this.date = DateTime.Now.ToString();
            this.activeUsers = new List<Session>();
        }

        /// <summary>
        /// Adds an invited user to the list of invited users
        /// </summary>
        /// <param name="user">the string of username</param>
        public void AddUser(string user)
        {
            if (!this.users.Contains(user)) this.users.Add(user);
        }

        /// <summary>
        /// Removes an invited user from the list and kicks him from the session..
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(string user)
        {
            if (this.users.Contains(user))
            {
                this.users.Remove(user);

                foreach (Session s in activeUsers)
                {
                    if (s.UserAcount.Username == user)
                    {
                        activeUsers.Remove(s);
                    }
                }
            }
        }

        /// <summary>
        /// This function adds a file to the dictionary with path or replaces it if it already exists.
        /// </summary>
        /// <param name="path">The path of the file</param>
        /// <param name="data">The data in string form</param>
        public void AddFile(string path, string data)
        {
            if (this.data.ContainsKey(path)) this.data[path] = data;
            else this.data.Add(path, data);
        }

        /// <summary>
        /// This removes a file from the dictionary with path and data..
        /// </summary>
        /// <param name="path"></param>
        public void RemoveFile(string path)
        {
            if (this.data.ContainsKey(path))
            {
                this.data.Remove(path);
            } 
        }

        /// <summary>
        /// This adds a session to the session list..
        /// </summary>
        /// <param name="s"></param>
        public void AddSession(Session s)
        {
            //For the members
            foreach (string username in users)
            {
                if (username == s.UserAcount.Username)
                {
                    this.activeUsers.Add(s);
                }
            }

            //For the owner
            if (s.UserAcount.Username == owner.Username)
            {
                this.activeUsers.Add(s);
            }
        }

        public IDictionary<string, string> GetFiles()
        {
            return data;
        }
        
        /// <summary>
        /// This ends a session from the session list..
        /// </summary>
        /// <param name="s"></param>
        public void EndSession(Session s)
        {
            this.activeUsers.Remove(s);
        }
        
        /// <summary>
        /// This gets all the sessions..
        /// </summary>
        /// <returns></returns>
        public IList<Session> GetSessions()
        {
            return this.activeUsers;
        }
        /// <summary>
        /// Gets all the invited users..
        /// </summary>
        /// <returns></returns>
        public IList<string> GetUsers()
        {
            return this.users;
        }
    }
}
