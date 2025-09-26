using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            User QM = new User();

            // Act
            QM.Name = "Nina";

            // Assert
            Assert.IsTrue(QM.Name == "Nina");
        }
    }
}
