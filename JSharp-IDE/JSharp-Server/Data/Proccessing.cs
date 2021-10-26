using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    class Proccessing
    {
        /// <summary>
        /// Loads all the user form a file from disk
        /// </summary>
        /// <returns>Returns all the users as list</returns>
        public static List<User> LoadUserData()
        {
            //List for user empty
            List<User> users = new List<User>();

            //Filling the list if the file exists...
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "userData.txt"))) {
                string data = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "userData.txt"));
             


                //Filling...
                JArray array = JArray.Parse(data);
                foreach (JObject o in array)
                {
                    users.Add(o.ToObject<User>());
                }
            }

            

            //Returning list.
            return users;
        }

        /// <summary>
        /// Saves the usersdata to disk
        /// </summary>
        /// <param name="users">The list with users</param>
        public static void SaveUserData(IList<User> users)
        {
            JArray data = JArray.FromObject(users);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory() , "userData.txt"), data.ToString());
        }

        //This is the algoritm for hashing passwords..
        private static SHA256 shaM = new SHA256Managed();

        /// <summary>
        /// Hashfunction for passwords..
        /// </summary>
        /// <param name="data">This the data that is beining hashed</param>
        /// <returns></returns>
        public static string HashUserPassword(string data)
        {
            return BitConverter.ToString(shaM.ComputeHash(Encoding.UTF32.GetBytes(data)));
        }
    }
}
