using JSharp_Server.Data;
using JSharp_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Comms
{
    public class Replyer
    {
        /// <summary>
        /// This is the sender object for sending responses back
        /// </summary>
        private ISender sender;

        /// <summary>
        /// This is the constructor for making a replyer
        /// </summary>
        /// <param name="sender">This is the sender that sends back the response</param>
        public Replyer(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Sends a succes back
        /// </summary>
        public void Succes()
        {
            object o = new
            {
                instruction = "OK",
            };
            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Sens a failed back
        /// </summary>
        public void Failed()
        {
            object o = new
            {
                instruction = "FAILED",
            };
            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }

       /// <summary>
       /// Sends back all project data back
       /// </summary>
       /// <param name="project">Projectfiles in dictonairy</param>
       /// <param name="projectname">Projectname as string</param>
        public void SendAll(IDictionary<string, string> project, string projectname)
        {
            NetworkFile[] networkFiles = new NetworkFile[project.Count];
            for (int i = 0; i < project.Count; i++)
            {
                networkFiles[i] = new NetworkFile(project.Keys.ElementAt(i), Convert.FromBase64String(project.Values.ElementAt(i)));
            }
            object o = new
            {
                instruction = "RequestedProject",

                data = new
                {
                    name = projectname,
                    files = networkFiles
                }

            };

            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Sends back the updated files
        /// </summary>
        /// <param name="project">The updated files in hashmap form</param>
        /// <param name="state"></param>
        public void SendUpdate(IDictionary<string, string> project, bool state)
        {
            int flag = state ? 0 : 1;

            NetworkFile[] networkFiles = new NetworkFile[project.Count];
            for (int i = 0; i < project.Count; i++)
            {
                networkFiles[i] = new NetworkFile(project.Keys.ElementAt(i), Convert.FromBase64String(project.Values.ElementAt(i)));
            }

            object o = new
            {
                instruction = "UpdatedProject",

                data = new
                {
                    flag = flag,
                    files = networkFiles
                }

            };

            this.sender.SendMessage(JsonConvert.SerializeObject(o));
        }

        /// <summary>
        /// Sends back that the project has exited
        /// </summary>
        public void SendExit()
        {
            object o = new
            {
                instruction = "ExitProject"
            };
        }
    }
}
