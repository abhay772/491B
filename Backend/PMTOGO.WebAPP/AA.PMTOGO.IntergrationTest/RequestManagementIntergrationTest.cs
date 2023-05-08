using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Common;

namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class RequestManagementIntergrationTest // RequestDAO only test
    {
        private readonly IConfiguration? _configuration;

        LoggerDAO logdao = new LoggerDAO();

        [TestMethod]
        public async Task GetServiceRequest()
        {
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _requestDAO = new ServiceRequestDAO(_configuration!);
            // Arrange
            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Get Service", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _requestDAO.AddServiceRequest(request);

            // Act
            Result result = await _requestDAO.GetServiceRequests("serviceProvider@gmail.com");
            bool actual = result.IsSuccessful;

            //clean up
            await _requestDAO.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }

        [TestMethod]
        public async Task AcceptServiceRequest_Pass()
        {
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _requestDAO = new ServiceRequestDAO(_configuration!);
            // Arrange  
            Logger _logger = new Logger(logdao);
            var serviceRequest = new ServiceRequestManagement(_logger!, _requestDAO, _userServiceDAO);

            Guid id = Guid.NewGuid();

            ServiceRequest request = new ServiceRequest(id, "Accept Service", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _requestDAO.AddServiceRequest(request);

            // Act
            await serviceRequest.AcceptRequest(id);
            Result result = await _requestDAO.FindServiceRequest(id);
            bool actual = result.IsSuccessful;

            //clean up
            var service = new UserServiceDAO(_configuration!);
            await service.DeleteUserService(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);


        }

        [TestMethod]
        public async Task DeclineServiceRequest_Pass()
        {
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _requestDAO = new ServiceRequestDAO(_configuration!);
            // Arrange

            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Decline Service", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _requestDAO.AddServiceRequest(request);

            // Act
            await _requestDAO.DeleteServiceRequest(id);
            Result result = await _requestDAO.FindServiceRequest(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }


        [TestMethod]
        public async Task AddAUserService_Pass()
        {
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _requestDAO = new ServiceRequestDAO(_configuration!);
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            ServiceRequest service = new ServiceRequest(id, "Add Service", "Landscape", "material delivery", "soil installation ", "1x/month", "random comment",
                "mssierra310@gmail.com", "Sara Jade", "sierra.harris01@student.csulb.edu", "Sierra Harris");
            await _userServiceDAO.AddUserService(service);
            Result result = await _userServiceDAO.FindUserService(id);
            bool actual = result.IsSuccessful;


            //clean up
            await _requestDAO.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }
        // need frequency change test

        [TestMethod]
        public async Task ChangeUserServiceFrequency_PASS()
        {
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _requestDAO = new ServiceRequestDAO(_configuration!);
            //arrange

            Logger _logger = new Logger(logdao);
            var request = new ServiceRequestManagement(_logger!, _requestDAO, _userServiceDAO);
            Guid id = Guid.NewGuid();

            ServiceRequest userService = new ServiceRequest(id, "Frequency Example", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _userServiceDAO.AddUserService(userService);

            //act
            await request.FrequencyChange(id, "3x/month", "serviceProvider@gmail.com");
            Result result = await _userServiceDAO.CheckFrequency(id, "3x/month");
            bool actual = result.IsSuccessful;

            //clean up
            await _userServiceDAO.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        //need cancellation test 

        [TestMethod]
        public async Task CancelUserService_PASS()
        {
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _requestDAO = new ServiceRequestDAO(_configuration!);
            //arrange
            Logger _logger = new Logger(logdao);
            var request = new ServiceRequestManagement(_logger!, _requestDAO, _userServiceDAO);
            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id, "Cancel Example", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _requestDAO.AddServiceRequest(userService);
            await _userServiceDAO.AddUserService(userService);

            //act
            Result result = await request.CancelUserService(id, "serviceProvider@gmail.com");
            bool actual = result.IsSuccessful;


            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }


    }
}