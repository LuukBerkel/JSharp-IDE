using JSharp_Server.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Comms
{
    public class Interpreter
    {
        //Variables
        private bool Authorized = false;
        private Manager manager;
        private Replyer replyer;
        private Session session;

        public event EventHandler<User> Event;

        /// <summary>
        /// This is the constructor of the interperter
        /// </summary>
        /// <param name="manager">This is the management object that handels users management</param>
        public Interpreter(Manager manager, Replyer replyer, Session session)
        {
            this.manager = manager;
            this.replyer = replyer;
            this.session = session;
        }

        /// <summary>
        /// This the commandhelper that sends the message to the right method
        /// </summary>
        /// <param name="json">This is the message received from the client</param>
        public void Command(JObject json)
        {
            JToken token;
            if (json.TryGetValue("instruction", out token))
            {
                string command = token.ToString();

                MethodInfo[] methods = typeof(Interpreter).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.ExactBinding);
                foreach (MethodInfo method in methods)
                {
                    if (method.GetCustomAttribute<AuthorizationAttribute>() != null 
                        && method.GetCustomAttribute<AuthorizationAttribute>().GetCommand() == command
                        && method.GetCustomAttribute<AuthorizationAttribute>().GetAuthorization() == Authorized)
                    {
                        method.Invoke(this, new object[] { json });
                    }
                }
            }
        }

        /// <summary>
        /// Checks the login if so than it is authorized
        /// </summary>
        /// <param name="json">The login message</param>
        [Authorization(false, "login")]
        private void Login(JObject json)
        {
            //Getting data from json
            JToken username = json.SelectToken("data.username");
            JToken password = json.SelectToken("data.password");
            if ( password != null && username != null)
            {
                //Finding user
                User user = manager.CheckUser(username.ToString(), password.ToString());
                Event.Invoke(this, user);

                //If it is a valid user then..
                if (user != null)
                {
                    this.Authorized = true;
                    this.replyer.Succes();
                    return;
                }

                //Else ...
                this.replyer.Failed();
            }
        }

        /// <summary>
        /// Registers a acount;
        /// </summary>
        /// <param name="json">The register message</param>
        [Authorization(false, "register")]
        private void Register(JObject json)
        {
            //Getting data from json
            JToken username = json.SelectToken("data.username"); 
            JToken password = json.SelectToken("data.password"); 
            if (username != null && password != null)
            {
                //Adding user if valid
                if (manager.AddUser(username.ToString(), password.ToString()))
                {
                    this.replyer.Succes();
                    return;
                }

                //Else..
                this.replyer.Failed();
            }
        }

        /// <summary>
        /// Creating a project based on a user
        /// </summary>
        /// <param name="json"></param>
        [Authorization(true, "createProject")]
        private void CreateProject(JObject json)
        {
            //Getting data from json
            JToken name = json.SelectToken("data.project");
            JToken user = json.SelectToken("data.users");
            JToken file = json.SelectToken("data.files");
            if(name != null && user != null && file != null)
            {
                //Getting name
                string project = name.ToString();

                //Getting files
                IDictionary<string, string> files = new Dictionary<string, string>();
                foreach (JObject o in (JArray) file)
                {
                    //Getting objects for dictionary
                    JToken path;
                    JToken data;
                    if (o.TryGetValue("filePath", out path) && o.TryGetValue("data", out data))
                    {
                        files.Add(path.ToString(), data.ToString());
                    }
                }

                //Getting users
                IList<string> users = new List<string>();
                foreach (string s in (JArray)user)
                {
                      users.Add(s);
                }

                //Adding it to active projects
                if (manager.AddProject(new Project(files, users, session.UserAcount, project) ,session))
                {
                    this.replyer.Succes();
                    MainWindow.SetDebugOutput("Project succesfully added");
                    return;
                }
                this.replyer.Failed();
                MainWindow.SetDebugOutput("Project add failed");
            }
            
        }

        /// <summary>
        /// Editing a 
        /// </summary>
        /// <param name="json"></param>
        [Authorization(true, "changeProject")]
        private void ChangeProject(JObject json)
        {
            //Parsing data
            JToken userToken = json.SelectToken("data.userFlag");
            JToken userList = json.SelectToken("data.users");
            JToken projectToken = json.SelectToken("data.fileFlag");
            JToken projectList = json.SelectToken("data.files");

            //For the users if they are in the message
            if (userToken != null && userList != null)
            {
                //Getting flags
                int userFlag = int.Parse(userToken.ToString());
                bool deleting = userFlag == 0 ? true : false;

                //Getting users
                IList<string> users = new List<string>();
                foreach (JObject o in (JArray)projectList)
                {
                    //Getting objects for list
                    JToken name;
                    if (o.TryGetValue("username", out name))
                    {
                        users.Add(name.ToString());   
                    }
                }

                //Executing
                this.manager.ChangeUserProject(users, session, deleting);
            }

            //For the files if they are in the message
            if (projectToken != null && projectList != null)
            {
                //Getting flags
                int fileFlag = int.Parse(projectToken.ToString());
                bool deleting = fileFlag == 0 ? true : false;

                //Getting files
                IDictionary<string, string> files = new Dictionary<string, string>();
                foreach (JObject o in (JArray)projectList)
                {
                    //Getting objects for dictionary
                    JToken path;
                    JToken data;
                    if (o.TryGetValue("filePath", out path) && o.TryGetValue("data", out data))
                    {
                        files.Add(path.ToString(), data.ToString());   
                    }
                }

                //Executing
                this.manager.ChangeFileProject(files, session, deleting);

            }
        }

        [Authorization(true, "removeProject")]
        private void RemoveProject(JObject json)
        {
            bool done = this.manager.RemoveProject(session);
            if (done) this.replyer.Succes();
            else this.replyer.Failed();
        }

        [Authorization(true, "joinProject")]
        private void JoinProject(JObject json)
        {


            JToken projectname = json.GetValue("project");
            if (projectname != null)
            {
                bool done = this.manager.JoinProject(session, projectname.ToString(), replyer);
                if (done) this.replyer.Succes();
                else this.replyer.Failed();
            }
        }
    }

    public class AuthorizationAttribute : Attribute
    {
        private bool AuthorizationRequirment;
        private string Command;

        public AuthorizationAttribute(bool authorizationRequirment, string command)
        {
            AuthorizationRequirment = authorizationRequirment;
            Command = command;
        }

        public bool GetAuthorization()
        {
            return this.AuthorizationRequirment;
        }

        public string GetCommand()
        {
            return this.Command;
        }
    }
}
