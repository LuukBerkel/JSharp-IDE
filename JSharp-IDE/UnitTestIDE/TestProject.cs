using JSharp_IDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestIDE
{
    [TestClass]
    public class TestProject
    {
        [TestMethod]
        public void Test_GetLocalPath()
        {
            // Arrange
            string filedir = @"C:\Users\test\Project\src\java.class";
            Project.ProjectDirectory = @"C:\Users\test\Project";
            string expected = @"src\java.class";

            // Act
            string result = Project.GetLocalPath(filedir);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
