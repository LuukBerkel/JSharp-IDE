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
        /// <summary>
        /// Creates a command that can be sent to the server to add the user to the server.
        /// It automatically gets the username and password from the settings file.
        /// </summary>
        /// <returns>Json object as Object</returns>
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

        /// <summary>
        /// Creates a command that can be sent to the server so we can log in.
        /// It automatically gets the username and password from the settings file.
        /// </summary>
        /// <returns>Json object as Object</returns>
        public static object Login()
        {
            return new
            {
                instruction = "login",
                data = new
                {
                    username = Settings.GetUsername(),
                    password = Settings.GetPassword()
                }
            };
        }

        /// <summary>
        /// This command lets the server know that you want to join a project.
        /// </summary>
        /// <param name="projectName">The name of the project you want to join.</param>
        /// <returns>Json object as Object</returns>
        public static object JoinProject(string projectName)
        {
            return new
            {
                instruction = "joinProject",
                project = projectName
            };
        }

        /// <summary>
        /// Creates a command that can be sent to the server to host a project.
        /// </summary>
        /// <param name="projectName">The name of the project</param>
        /// <param name="userNames">The list of usernames that are allowed to join (excluding your own username).</param>
        /// <param name="fileList">The list of files that have to be sent to the server.</param>
        /// <returns>Json object as Object</returns>
        public static object HostProject(string projectName, string[] userNames, Network.NetworkFile[] fileList)
        {
            return new
            {
                instruction = "createProject",
                data = new
                {
                    project = projectName,
                    users = userNames,
                    files = fileList
                }
            };
        }

        /// <summary>
        /// Creates a command that indicates that we want to update or remove some files.
        /// </summary>
        /// <param name="fileList">Files to add/edit or remove</param>
        /// <param name="flag">0: Remove, 1: Add/Edit</param>
        /// <returns>Json object as Object</returns>
        public static object UpdateFiles(NetworkFile[] fileList, int flag)
        {
            return new
            {
                instruction = "changeProject",
                data = new
                {
                    fileFlag = flag,
                    files = fileList
                }
            };
        }
    }
}
