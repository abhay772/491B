using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;


namespace AA.PMTOGO.UnitTest
{
    //clean up test 
    [TestClass]
    public class ServiceManagementUnitTest
    {
        [TestMethod]
        public void CreateServiceManagementInstance()
        {
            // Arrange
            var expected = typeof(UserServiceManagement);

            // Act
            var actual = new UserServiceManagement();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }


    }
}
