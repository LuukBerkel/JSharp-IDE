using JSharp_Server.Comms;
using JSharp_Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestServer
{
    [TestClass]
    public class TestProject
    {
        [TestMethod]
        public void Test_AddUser_New()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 3;

            // Act
            project.AddUser("jesse");
            int result = project.GetUsers().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_AddUser_AlreadyExists()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 2;

            // Act
            project.AddUser("lars");
            int result = project.GetUsers().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_RemoveUser_Succes()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 1;

            // Act
            project.RemoveUser("lars");
            int result = project.GetUsers().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_RemoveUser_Fail()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 2;

            // Act
            project.RemoveUser("jesse");
            int result = project.GetUsers().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_AddFile_Success()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 4;

            // Act
            project.AddFile(@"C:\Users\Larsl\Desktop\test\Motor.java", "public class Motor { }");
            int result = project.GetFiles().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_AddFile_Fail()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 3;

            // Act
            project.AddFile(@"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor { }");
            int result = project.GetFiles().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_RemoveFile_Success()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 2;

            // Act
            project.RemoveFile(@"C:\Users\Larsl\Desktop\test\Sensor.java");
            int result = project.GetFiles().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_RemoveFile_Fail()
        {
            // Arrange
            Dictionary<string, string> files = new Dictionary<string, string>(){
                { @"C:\Users\Larsl\Desktop\test\users.txt", "lars \nluuk"},
                { @"C:\Users\Larsl\Desktop\test\Main.java", "public class Main {}"},
                { @"C:\Users\Larsl\Desktop\test\Sensor.java", "public class Sensor {}"},
            };
            List<string> users = new List<string>()
            {
                { "lars" },
                { "luuk" }
            };
            User owner = new User("lars", "wachtwoord", false);
            Project project = new Project(files, users, owner, "MyProject");
            int expected = 3;

            // Act
            project.RemoveFile(@"C:\Users\Larsl\Desktop\test\Motor.java");
            int result = project.GetFiles().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
