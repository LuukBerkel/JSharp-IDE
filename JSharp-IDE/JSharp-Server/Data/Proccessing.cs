using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    class Proccessing
    {
        public static List<User> LoadUserData()
        {
            List<User> users = new List<User>();

            if (File.Exists(Directory.GetCurrentDirectory() + @"\usersData.txt")) {
                string data = File.ReadAllText(Directory.GetCurrentDirectory() + @"\usersData.txt");


                JArray array = JArray.Parse(data);
                foreach (JObject o in array)
                {
                    users.Add(o.ToObject<User>());
                }
            }

            return users;
        }

        public static void SaveUserData(List<User> users)
        {
            JArray data = JArray.FromObject(users);
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\userData.txt", data.ToString());
        }

        private static SHA256 shaM = new SHA256Managed();

        public static string HashUserPassword(string data)
        {
            return BitConverter.ToString(shaM.ComputeHash(Encoding.UTF32.GetBytes(data)));
        }
    }
}
