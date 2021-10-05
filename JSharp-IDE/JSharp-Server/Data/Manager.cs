using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    public class Manager : ObservableObject
    {
        private ObservableCollection<Project> mProjects;
        public ObservableCollection<Project> Projects
        {
            get
            {
                return mProjects;
            }

            set
            {
                mProjects = value;
                NotifyPropertyChanged();
            }
        }

        private List<User> users;

        public Manager()
        {
            this.mProjects = new ObservableCollection<Project>();
            this.users = Proccessing.LoadUserData();
            this.mProjects.Add(new Project(null, null, new User("JoeMama", "help", false), "Weerstation"));
            NotifyPropertyChanged();
        }

        public User CheckUser( string username, string password)
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
            if (this.users.Where(e => e.Username == username).ToList().Count <= 0)
            {
                this.users.Add(new User(username, password, true));
                return true;
            }
            return false;
        }

        public bool AddProject(Project p)
        {
            if (this.mProjects.Where(e => e.name == p.name).ToList().Count <= 0)
            {
                Debug.WriteLine("project added");
                this.mProjects.Add(p);
                //Projects = mProjects;
                NotifyPropertyChanged();
                return true;
            }
            return false;
        }
    }
}
