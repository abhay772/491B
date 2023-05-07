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
    public class RequestManagementUnitTest
    {


        /* private readonly IServiceDAO _serviceDAO;
         private readonly IUserServiceDAO _userServiceDAO;
         private readonly IServiceRequestDAO _serviceRequestDAO;
         private readonly ILogger? _logger;
         private readonly string _connectionString;
         private readonly IConfiguration? _configuration;
         public RequestManagementUnitTest(IServiceDAO serviceDAO, IServiceRequestDAO servicerequestDAO, IUserServiceDAO userServiceDAO, ILogger logger, IConfiguration _configuration)
         {
             _serviceDAO = serviceDAO;
             _serviceRequestDAO = servicerequestDAO;
             _userServiceDAO = userServiceDAO;
             _logger = logger;
             _connectionString = _configuration!.GetConnectionString("ServiceDbConnectionString")!;
         }*/
        private readonly IConfiguration? _configuration;

        LoggerDAO logdao = new LoggerDAO();
        
        [TestMethod]
        public void CreateRequestManagementInstance()
        {
            UsersDAO _usersDAO = new UsersDAO(_configuration!);
            ServiceDAO _serviceDAO = new ServiceDAO(_configuration!);
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _serviceRequestDAO = new ServiceRequestDAO(_configuration!);
            // Arrange
            var expected = typeof(ServiceRequestManagement);
            Logger _logger = new Logger(logdao);

            // Act
            var actual = new ServiceRequestManagement(_logger!, _serviceRequestDAO, _userServiceDAO);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

    }
}

