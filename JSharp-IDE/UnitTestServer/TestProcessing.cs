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
    public class TestProcessing
    {
        [TestMethod]
        public void Test_DataHashing()
        {
            // Arrange
            string password = "SuperVeiligWachtwoord123!@#";
            string expected = "12-3E-DB-A8-A9-E1-31-45-BF-BF-9D-39-2F-71-9A-CC-04-11-4F-5E-FB-16-17-2E-D8-F7-6E-4C-8B-96-74-15";

            // Act
            string result = Proccessing.HashUserPassword(password);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
