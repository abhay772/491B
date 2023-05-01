using AA.PMTOGO.Services;


namespace AA.PMTOGO.UnitTest
{
    //clean up test
    [TestClass]
    public class RequestManagementUnitTest
    {
        [TestMethod]
        public void CreateRequestManagementInstance()
        {
            // Arrange
            var expected = typeof(ServiceRequestManagement);

            // Act
            var actual = new ServiceRequestManagement();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

    }
}

