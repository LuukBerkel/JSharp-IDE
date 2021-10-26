using JSharp_IDE.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTestIDE
{
    [TestClass]
    public class TestJSONCommand
    {
        [TestMethod]
        public void Test_SignUpObject()
        {
            // Arrange
            object received = JSONCommand.SignUp();
            object expected = new
            {
                instruction = "register",
                data = new
                {
                    username = "username",
                    password = "password"
                }
            };

            // Act
            string receivedJson = JsonConvert.SerializeObject(received);
            string expectedJson = JsonConvert.SerializeObject(expected);

            // Assert
            Assert.AreEqual(expectedJson, receivedJson);
        }

        [TestMethod]
        public void Test_LoginObject()
        {
            // Arrange
            object received = JSONCommand.Login();
            object expected = new
            {
                instruction = "login",
                data = new
                {
                    username = "username",
                    password = "password"
                }
            };

            // Act
            string receivedJson = JsonConvert.SerializeObject(received);
            string expectedJson = JsonConvert.SerializeObject(expected);

            // Assert
            Assert.AreEqual(expectedJson, receivedJson);
        }

        [TestMethod]
        public void Test_JoinProjectObject()
        {
            // Arrange
            string projectName = "MyProject";

            object received = JSONCommand.JoinProject(projectName);
            object expected = new
            {
                instruction = "joinProject",
                project = projectName
            };

            // Act
            string receivedJson = JsonConvert.SerializeObject(received);
            string expectedJson = JsonConvert.SerializeObject(expected);

            // Assert
            Assert.AreEqual(expectedJson, receivedJson);
        }

        [TestMethod]
        public void Test_HostProjectObject()
        {
            // Arrange
            string projectName = "MyProject";
            string[] users = new string[] { "joe", "luuk"};
            NetworkFile[] files = new NetworkFile[] { new NetworkFile(@"src\test.java", new byte[] { 0, 12, 135, 4}), new NetworkFile(@"src\users.txt", new byte[] { 1, 0, 5, 7}) };

            object received = JSONCommand.HostProject(projectName, users, files);
            object expected = new
            {
                instruction = "createProject",
                data = new
                {
                    project = projectName,
                    users = users,
                    files = files
                }
            };

            // Act
            string receivedJson = JsonConvert.SerializeObject(received);
            string expectedJson = JsonConvert.SerializeObject(expected);

            // Assert
            Assert.AreEqual(expectedJson, receivedJson);
        }

        [TestMethod]
        public void Test_UpdateFilesObject()
        {
            // Arrange
            NetworkFile[] files = new NetworkFile[] { new NetworkFile(@"src\test.java", new byte[] { 0, 12, 135, 4 }), new NetworkFile(@"src\users.txt", new byte[] { 1, 0, 5, 7 }) };
            int flag = 0;

            object received = JSONCommand.UpdateFiles(files, 0);
            object expected = new
            {
                instruction = "changeProject",
                data = new
                {
                    fileFlag = flag,
                    files = files
                }
            };

            // Act
            string receivedJson = JsonConvert.SerializeObject(received);
            string expectedJson = JsonConvert.SerializeObject(expected);

            // Assert
            Assert.AreEqual(expectedJson, receivedJson);
        }
    }
}
