using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_IDE.Network
{
    public class JSONCommand
    {
        public static object SignUp(string username, string password)
        {
            return new
            {
                instruction = "register",
                data = new
                {
                    username = username,
                    password = password
                }
            };
        }

        public static object JoinProject(string projectName)
        {
            return new
            {
                instruction = "joinProject",
                project = projectName
            };
        }
    }
}
