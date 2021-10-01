using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_Server.Data
{
    class Proccessing
    {
        public static List<User> LoadUserData()
        {
            List<User> users = new List<User>();
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + @"\usersData.txt");


            JArray array = JArray.Parse(data);
            foreach (JObject o in array)
            {
                users.Add(o.ToObject<User>());
            }

            return users;
        }

        public static void SaveUserData(List<User> users)
        {
            JArray data = JArray.FromObject(users);
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\userData.txt", data.ToString());
        }
    }
}
