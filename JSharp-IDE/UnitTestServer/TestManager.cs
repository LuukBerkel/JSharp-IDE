using JSharp_Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestServer
{
    [TestClass]
    public class TestManager
    {
        [TestMethod]
        public void Test_CheckUser_Success()
        {
            // Arrange
            Manager manager = new Manager();
            manager.AddUser("Gebruiker", "wachtwoord");

            // Act
            User result = manager.CheckUser("Gebruiker", "wachtwoord");

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test_CheckUser_Fail()
        {
            // Arrange
            Manager manager = new Manager();
            manager.AddUser("Gebruiker", "wachtwoord");

            // Act
            User result = manager.CheckUser("", "wachtwoord");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Test_AddUser_Success()
        {
            // Arrange
            Manager manager = new Manager();

            // Act
            bool result = manager.AddUser("xX_Programmer_Xx", "wachtwoord");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_AddUser_Fail()
        {
            // Arrange
            Manager manager = new Manager();
            manager.AddUser("Gebruiker", "wachtwoord");

            // Act
            bool result = manager.AddUser("Gebruiker", "wachtwoord");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
