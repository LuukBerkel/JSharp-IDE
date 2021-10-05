using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    public class Manager : ObservableObject
    {
        public ObservableCollection<Project> projects { get; set; }
        private List<User> users;

        public Manager()
        {
            this.projects = new ObservableCollection<Project>();
            this.users = Proccessing.LoadUserData();
            this.projects.Add(new Project(null, null, new User("JoeMama", "help", false), "Weerstation"));
            NotifyPropertyChanged("projects");
        }

        public User? CheckUser( string username, string password)
        {
            foreach(User user in this.users) 
            {
                if (user.Password == Proccessing.HashUserPassword(password) && user.Username == username)
                {
                    return user;
                }
            }
            return null;
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
