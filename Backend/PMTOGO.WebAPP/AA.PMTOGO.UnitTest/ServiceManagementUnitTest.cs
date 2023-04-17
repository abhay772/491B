using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;


namespace AA.PMTOGO.UnitTest
{
    //clean up test 
    [TestClass]
    public class ServiceManagementUnitTest
    {
        private IUsersDAO _usersDAO;

        public ServiceManagementUnitTest(IUsersDAO usersDAO)
        {
            _usersDAO = usersDAO;
        }

        [TestMethod]
        public void CreateServiceManagementInstance()
        {

            // Arrange
            var expected = typeof(UserServiceManagement);

            // Act
            var actual = new UserServiceManagement(_usersDAO);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }


    }
}
