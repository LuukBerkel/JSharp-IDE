using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            if (!Exists()) {
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(new
                {
                    username = "username",
                    password = "password",
                    serverAddress = "localhost",
                    serverPort = "6969",
                    javaBin = "C:\\Program Files\\Java\\jdk1.8.0_261\\bin"
                })); ;
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

        public static void UpdateServerAddress(string address)
        {
            UpdateValue("serverAddress", address);
        }

        public static void UpdateServerPort(string port)
        {
            UpdateValue("serverPort", port);
        }

        public static void UpdateJavaDir(string path)
        {
            UpdateValue("javaBin", path);
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

        public static string GetPassword()
        {
            return GetValue("password");
        }

        public static string GetServerAddress()
        {
            return GetValue("serverAddress");
        }

        public static string GetServerPort()
        {
            return GetValue("serverPort");
        }

        public static string GetJavaBin()
        {
            return GetValue("javaBin");
        }

        private static string GetValue(string key)
        {
            if (!Exists()) CreateSettingsFile();
            JObject settings = JObject.Parse(File.ReadAllText(SettingsPath));
            return settings.GetValue(key)?.ToString() ?? "";
        }
    }
}
