using JSharp_Server.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Comms
{
    class Interpreter
    {
        //Variables
        private bool Authorized = false;
        private Manager manager;

        /// <summary>
        /// This is the constructor of the interperter
        /// </summary>
        /// <param name="manager">This is the management object that handels users management</param>
        public Interpreter(Manager manager)
        {
            this.manager = manager;
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

                MethodInfo[] methods = typeof(Interpreter).GetMethods(BindingFlags.NonPublic);
                foreach (MethodInfo method in methods)
                {
                    if (method.GetCustomAttribute<AuthorizationAttribute>().GetCommand() == command
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
            JToken? username;
            JToken? password;
            if (json.TryGetValue("password", out password) && json.TryGetValue("username", out username))
            {
                this.Authorized = manager.CheckUser(username.ToString(), password.ToString());
            }
        }

        [Authorization(false, "register")]
        private void Register(JObject json)
        {
            JToken? username;
            JToken? password;
            if (json.TryGetValue("password", out password) && json.TryGetValue("username", out username))
            {
                manager.AddUser(username.ToString(), password.ToString());
            }
        }

        [Authorization(true, "createProject")]
        private void CreateProject(JObject json)
        {

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





    }

    internal class AuthorizationAttribute : Attribute
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
