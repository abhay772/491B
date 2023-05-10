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
        private readonly IConfiguration? _configuration;

        LoggerDAO logdao = new LoggerDAO();
        
        [TestMethod]
        public void CreateRequestManagementInstance()
        {
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