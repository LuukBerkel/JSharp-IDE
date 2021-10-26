using JSharp_IDE.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Environment;

namespace UnitTestIDE
{
    [TestClass]
    public class TestSettings
    {
        [TestMethod]
        public void Test_SettingsFileExists_False()
        {
            // Arrange
            Settings.SettingsPath = Path.Combine(Environment.GetFolderPath(SpecialFolder.Desktop), "settings2.json");

            // Act
            bool result = Settings.Exists();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_SettingsFileExists_True()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            Settings.SettingsPath = filePath;
            Settings.CreateSettingsFile();

            // Act
            bool result = Settings.Exists();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_GetValue()
        {
            // Arrange
            Settings.SettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            string expected = "42069";

            // Act
            string result = Settings.GetServerPort();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_UpdateValue()
        {
            // Arrange
            Settings.SettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            string expected = "42069";

            // Act
            Settings.UpdateServerPort(expected);
            string result = Settings.GetServerPort();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
