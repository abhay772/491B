using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using Microsoft.Extensions.Configuration;

namespace AA.PMTOGO.UnitTest
{
    //clean up test 
    [TestClass]
    public class ServiceManagementUnitTest
    {
        private IUsersDAO _usersDAO;
        private readonly IServiceDAO _serviceDAO;
        private readonly IUserServiceDAO _userServiceDAO;
        private readonly IServiceRequestDAO _serviceRequestDAO;
        private readonly ILogger? _logger;
        private readonly string _connectionString;
        private readonly IConfiguration? _configuration;
        public ServiceManagementUnitTest(IUsersDAO usersDAO, IServiceDAO serviceDAO, IServiceRequestDAO servicerequestDAO, IUserServiceDAO userServiceDAO, ILogger logger, IConfiguration _configuration)
        {
            _usersDAO = usersDAO;
            _serviceDAO = serviceDAO;
            _serviceRequestDAO = servicerequestDAO;
            _userServiceDAO = userServiceDAO;
            _logger = logger;
            _connectionString = _configuration!.GetConnectionString("ServiceDbConnectionString")!;
        }

        [TestMethod]
        public void CreateServiceManagementInstance()
        {

            // Arrange
            var expected = typeof(UserServiceManagement);

            // Act
            var actual = new UserServiceManagement(_usersDAO, _serviceDAO, _userServiceDAO, _serviceRequestDAO, _logger!);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }


    }
}
