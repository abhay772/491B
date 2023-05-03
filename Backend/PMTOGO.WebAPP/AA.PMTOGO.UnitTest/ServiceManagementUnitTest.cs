using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace AA.PMTOGO.UnitTest
{
    //clean up test 
    [TestClass]
    public class ServiceManagementUnitTest
    {
       
        UsersDAO _usersDAO = new UsersDAO();
        ServiceDAO _serviceDAO = new ServiceDAO();
        UserServiceDAO _userServiceDAO = new UserServiceDAO();
        ServiceRequestDAO _serviceRequestDAO = new ServiceRequestDAO();
        LoggerDAO logdao = new LoggerDAO();
        

        [TestMethod]
        public void CreateServiceManagementInstance()
        {
            var expected = typeof(UserServiceManagement);
            Logger _logger = new Logger(logdao);
            // Arrange

            // Act
            var actual = new UserServiceManagement(_usersDAO, _serviceDAO, _userServiceDAO, _serviceRequestDAO, _logger!);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public async Task GetUserAccountsTest()
        {

            // Arrange
            var dao = new UsersDAO();

            // Act
            Result test = await dao.GetUserAccounts();
            bool actual = test.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }


    }
}
