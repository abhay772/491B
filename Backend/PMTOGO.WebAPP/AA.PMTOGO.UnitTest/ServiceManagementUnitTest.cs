using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;


namespace AA.PMTOGO.UnitTest
{
    //clean up test 
    [TestClass]
    public class ServiceManagementUnitTest
    {
        private IUsersDAO _usersDAO;
        private readonly ILogger? _logger;

        public ServiceManagementUnitTest(IUsersDAO usersDAO, ILogger? logger)
        {
            _usersDAO = usersDAO;
            _logger = logger;
        }

        [TestMethod]
        public void CreateServiceManagementInstance()
        {

            // Arrange
            var expected = typeof(UserServiceManagement);

            // Act
            var actual = new UserServiceManagement(_usersDAO, _logger!);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }


    }
}
