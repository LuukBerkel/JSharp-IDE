using JSharp_Server.Comms;
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
    public class Manager
    {
        //Active
        private static IList<Project> projects;
        private static IList<Session> active;

        //Database
        private IList<User> users;
    


        public Manager()
        {
            projects = new List<Project>();
            active = new List<Session>();
            this.users = Proccessing.LoadUserData();
        }

        /// <summary>
        /// Manages the active users by adding one
        /// </summary>
        /// <param name="s"></param>
        public void AddSession(Session s)
        {
            //Because this list is static it can be
            //accesed by multipule threads at once
            lock (active) {
                active.Add(s);
            }
        }

        /// <summary>
        /// Removes a active session from the list
        /// </summary>
        /// <param name="s"></param>
        public void RemoveSessoin(Session s)
        {
            //Because this list is static it can be
            //accesed by multipule threads at once
            lock (active)
            {
                active.Remove(s);
            }
        }
        


        /// <summary>
        /// Checks a users credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User CheckUser( string username, string password)
        {
            //Locked because entry by multipl threads
            lock (users)
            {
                //If the credentails are found then it will return the user.
                foreach (User user in this.users)
                {
                    //if it are ther credentails then..
                    if (user.Password == Proccessing.HashUserPassword(password) && user.Username == username)
                    {
                        return user;
                    }
                }
            }

            //Else it will return null
            return null;
        }

        /// <summary>
        /// Add a user to the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AddUser(string username, string password)
        {
            //Locking because else current modifcation exception
            lock (users)
            {
                //If the user doesn't exist then..
                if (this.users.Where(e => e.Username == username).ToList().Count <= 0)
                {
                    //Adding user
                    this.users.Add(new User(username, password, true));
                    return true;
                }
            }

            //If not then return false
            return false;
        }

        /// <summary>
        /// Adds projects to a list
        /// </summary>
        /// <param name="project">The project</param>
        /// <param name="session">Adding the user</param>
        /// <returns></returns>
        public bool AddProject(Project project, Session session)
        {
            //Locking for threads safty
            lock (projects)
            {
                //If the project name doesn't exist..
                if (projects.Where(e => e.name == project.name).ToList().Count <= 0)
                {
                    projects.Add(project);
                    project.AddSession(session);
                    MainWindow.SetListview(projects);
                    return true;
                };
            }
            return false;
        }

        /// <summary>
        /// Changing files of the project
        /// </summary>
        /// <param name="changes">The changes</param>
        /// <param name="session">The session for finding the project</param>
        public void ChangeFileProject(IDictionary<string, string>changes, Session session, bool deleting)
        {
            //Locking for safty
            lock (projects)
            {
                //Looping throug all projects
                foreach (Project p in projects)
                {

                    //If project contains active session
                    if (p.GetSessions().Where(s => s == session).ToList().Count > 0)
                    {
                        //Add files
                        foreach (KeyValuePair<string, string> entry in changes)
                        {
                            if (!deleting) p.AddFile(entry.Key, entry.Value);
                            else p.RemoveFile(entry.Key);
                        }

                        //Still logic needed for sending to other users.......

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Changes the users of a project..
        /// </summary>
        /// <param name="changeUsers"></param>
        /// <param name="session"></param>
        /// <param name="deleting"></param>
        public void ChangeUserProject(IList<string> changeUsers, Session session, bool deleting) 
        {
            //locking for thread safty
            lock (projects)
            {
                //Looping throug all projects
                foreach (Project p in projects)
                {

                    //If project contains active session
                    if (p.owner.Username == session.UserAcount.Username)
                    {
                        foreach (string user in changeUsers)
                        {
                            if (!deleting) p.AddUser(user);
                            else p.RemoveUser(user);

                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Exites the project from the server...
        /// </summary>
        /// <param name="session">The session of the owner</param>
        public void RemoveProject(Session session)
        {
            //locking for thread safty
            lock (projects)
            {
                //Looping throug all projects
                foreach (Project p in projects)
                {
                    //If project contains active session
                    if (p.owner.Username == session.UserAcount.Username)
                    {
                        projects.Remove(p);
                    }
                }
            }
        }
    }


}
