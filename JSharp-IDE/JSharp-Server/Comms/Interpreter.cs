﻿using JSharp_Server.Data;
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
            JToken? token;
            if (json.TryGetValue("instruction", out token))
            {
                string command = token.ToString();
                Debug.WriteLine(command);

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
                    JToken? path;
                    JToken? data;
                    if (o.TryGetValue("filePath", out path) && o.TryGetValue("data", out data))
                    {
                        files.Add(path.ToString(), data.ToString());
                    }
                }

                //Getting users
                IList<string> users = new List<string>();
                foreach (JObject o in (JArray)file)
                {
                      users.Add(o.ToString());
                }

                //Adding it to active projects
                if (manager.AddProject(new Project(files, users, session.UserAcount, project)))
                {
                    this.replyer.Succes();
                    return;
                }
                this.replyer.Failed();
            }
            
        }

        [Authorization(true, "changeProject")]
        private void ChangeProject(JObject json)
        {

        }

        [Authorization(true, "removeProject")]
        private void RemoveProject(JObject json)
        {
            
        }

        [Authorization(true, "notificateProject")]
        private void NotificateProject(JObject json)
        {

        }

        [Authorization(true, "joinProject")]
        private void JoinProject(JObject json)
        {

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