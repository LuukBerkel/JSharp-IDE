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
        private Replyer replyer;

        public event EventHandler<User> Event;

        /// <summary>
        /// This is the constructor of the interperter
        /// </summary>
        /// <param name="manager">This is the management object that handels users management</param>
        public Interpreter(Manager manager, Replyer replyer)
        {
            this.manager = manager;
            this.replyer = replyer;
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
           /* //Getting data from json
            JToken name = json.SelectToken("data.username");
            JToken user = json.SelectToken("data.password");
            JToken 

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
            }*/
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
