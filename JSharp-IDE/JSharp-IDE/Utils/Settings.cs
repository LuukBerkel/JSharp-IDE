using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp_IDE.Utils
{
    public class Settings
    {
        public static string SettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");

        public static bool Exists()
        {
            return File.Exists(SettingsPath);
        }

        public static void CreateSettingsFile()
        {
            if (!File.Exists(SettingsPath))
            {
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(new
                {
                    username = "username",
                    password = "password"
                }));
            }
        }

        public static void UpdateUsername(string username)
        {
            UpdateValue("username", username);
        }

        public static void UpdatePassword(string password)
        {
            //TODO Implement hashing
            UpdateValue("password", password);
        }

        public static void UpdateValue(string key, string value)
        {
            if (!Exists()) CreateSettingsFile();

            JObject settings = JObject.Parse(File.ReadAllText(SettingsPath));
            JToken token = settings.SelectToken(key);
            token.Replace(value);
            string updatedSettings = settings.ToString();
            File.WriteAllText(SettingsPath, updatedSettings);
        }

        public static string GetUsername()
        {
            return GetValue("username");
        }

        private static string GetValue(string key)
        {
            if (!Exists()) CreateSettingsFile();
            JObject settings = JObject.Parse(File.ReadAllText(SettingsPath));
            

            return settings.GetValue(key).ToString();
        }
    }
}
