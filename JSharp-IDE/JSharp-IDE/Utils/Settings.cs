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
        /// <summary>
        /// Path of the settings file.
        /// </summary>
        public static string SettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");

        /// <summary>
        /// Check if the file exists and if it has content in it.
        /// </summary>
        /// <returns>True if the file exists and has content in it.</returns>
        public static bool Exists()
        {
            return File.Exists(SettingsPath) && File.ReadAllLines(SettingsPath).Length != 0;
        }

        /// <summary>
        /// Create a new settings file.
        /// </summary>
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
                }));
            }
        }

        /// <summary>
        /// Update the username in the settings file.
        /// </summary>
        /// <param name="username">New username</param>
        public static void UpdateUsername(string username)
        {
            UpdateValue("username", username);
        }

        /// <summary>
        /// Update the password in the settings file.
        /// </summary>
        /// <param name="username">New password</param>
        public static void UpdatePassword(string password)
        {
            //TODO Implement hashing
            UpdateValue("password", password);
        }

        /// <summary>
        /// Update the server ip in the settings file.
        /// </summary>
        /// <param name="username">New ip</param>
        public static void UpdateServerAddress(string address)
        {
            UpdateValue("serverAddress", address);
        }

        /// <summary>
        /// Update the server port in the settings file.
        /// </summary>
        /// <param name="username">New port</param>
        public static void UpdateServerPort(string port)
        {
            UpdateValue("serverPort", port);
        }

        /// <summary>
        /// Update the java directory in the settings file.
        /// </summary>
        /// <param name="username">New java bin directory</param>
        public static void UpdateJavaDir(string path)
        {
            UpdateValue("javaBin", path);
        }

        /// <summary>
        /// Update a value in the settings file.
        /// </summary>
        /// <param name="key">Key to search for</param>
        /// <param name="value">Value to edit</param>
        public static void UpdateValue(string key, string value)
        {
            if (!Exists()) CreateSettingsFile();

            JObject settings = JObject.Parse(File.ReadAllText(SettingsPath));
            JToken token = settings.SelectToken(key);
            token.Replace(value);
            string updatedSettings = settings.ToString();
            File.WriteAllText(SettingsPath, updatedSettings);
        }

        /// <summary>
        /// Gets the username from the settings file.
        /// </summary>
        /// <returns>Username</returns>
        public static string GetUsername()
        {
            return GetValue("username");
        }

        /// <summary>
        /// Gets the password from the settings file.
        /// </summary>
        /// <returns>Password</returns>
        public static string GetPassword()
        {
            return GetValue("password");
        }

        /// <summary>
        /// Gets the server ip from the settings file.
        /// </summary>
        /// <returns>Server ip</returns>
        public static string GetServerAddress()
        {
            return GetValue("serverAddress");
        }

        /// <summary>
        /// Gets the server port from the settings file.
        /// </summary>
        /// <returns>Server port</returns>
        public static string GetServerPort()
        {
            return GetValue("serverPort");
        }

        /// <summary>
        /// Gets the java bin directory from the settings file.
        /// </summary>
        /// <returns>Java bin directory</returns>
        public static string GetJavaBin()
        {
            return GetValue("javaBin");
        }

        /// <summary>
        /// Gets a value from the settings file.
        /// </summary>
        /// <param name="key">Key that holds the value you want to have.</param>
        /// <returns>Value that was searched for via the key</returns>
        private static string GetValue(string key)
        {
            if (!Exists()) CreateSettingsFile();
            JObject settings = JObject.Parse(File.ReadAllText(SettingsPath));
            return settings.GetValue(key)?.ToString() ?? "";
        }
    }
}
