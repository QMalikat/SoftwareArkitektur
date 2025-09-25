namespace test;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
        // Arrange
        User QM = new User();

        // Act
        QM.name = "Nina";

        // Asset
        Assert.isTrue(QM.name, "Nina");
    }

}