using JSharp_IDE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_IDE.Network
{
    public class JSONCommand
    {
        public static object SignUp()
        {
            return new
            {
                instruction = "register",
                data = new
                {
                    username = Settings.GetUsername(),
                    password = Settings.GetPassword()
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
