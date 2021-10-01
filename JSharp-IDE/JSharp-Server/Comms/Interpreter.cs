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
        private bool Authorized = false;
        private Interpreter interpreter;

        public Interpreter(Interpreter interpreter)
        {
            this.interpreter = interpreter;
        }

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

        [Authorization(false, "login")]
        private void Login(JObject json)
        {

            this.Authorized = true;
        }

        [Authorization(false, "register")]
        private void Register(JObject json)
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
