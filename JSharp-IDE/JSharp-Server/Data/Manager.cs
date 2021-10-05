using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace JSharp_Server.Data
{
    [AddINotifyPropertyChangedInterface]
    public class Manager : INotifyPropertyChanged
    {


        public ObservableCollection<Project> projects { get; set; }
        private List<User> users;

        public event PropertyChangedEventHandler PropertyChanged;


        public Manager()
        {

            this.projects = new ObservableCollection<Project>();
            this.users = Proccessing.LoadUserData();


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
           
            if (this.projects.Where(e => e.name == p.name).ToList().Count <= 0)
            {

               


               this.projects.Add(p);
                MainWindow.SetLisview(this.projects);
               
            
            };
         
            return false;
        }

        

    }
}
