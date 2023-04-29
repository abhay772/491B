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
        
        private readonly IUsersDAO _authNDAO;
        private readonly IServiceRequestDAO _requestDAO;
        private readonly IUserServiceDAO _userServiceDAO;
        private readonly string _connectionString;
        private readonly ILogger? _logger;
        private readonly IConfiguration? _configuration;


        public RequestManagementIntergrationTest(IUsersDAO usersDAO, IServiceRequestDAO requestDAO, IUserServiceDAO userserviceDAO,  ILogger logger, IConfiguration configuration)
        {
            _authNDAO = usersDAO;
            _requestDAO = requestDAO;
            _userServiceDAO = userserviceDAO;
            _logger = logger;
            _connectionString = _configuration!.GetConnectionString("ServiceDbConnectionString")!;
        }

        [TestMethod]
        public async Task GetServiceRequest()
        {
            // Arrange
            var dao = new ServiceRequestDAO(_configuration!);
            var service = new UserServiceDAO(_configuration!);
            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Get Service", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddServiceRequest(request);

            // Act
            Result result = await dao.GetServiceRequests("serviceProvider@gmail.com");
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);



        }

        [TestMethod]
        public async Task AcceptServiceRequest_Pass()
        {
            // Arrange

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
            // Arrange
            var dao = new ServiceRequestDAO(_configuration!);

            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Decline Service", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddServiceRequest(request);

            // Act
            await dao.DeleteServiceRequest(id);
            Result result = await dao.FindServiceRequest(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }


        [TestMethod]
        public async Task AddAUserService_Pass()
        {
            // Arrange
            var userService = new UserServiceDAO(_configuration!);
            var request = new ServiceRequestDAO(_configuration!);

            Guid id = Guid.NewGuid();

            // Act
            ServiceRequest service = new ServiceRequest(id, "Add Service", "Landscape", "material delivery", "soil installation ", "1x/month", "random comment",
                "mssierra310@gmail.com", "Sara Jade", "sierra.harris01@student.csulb.edu", "Sierra Harris");
            await userService.AddUserService(service);
            Result result = await userService.FindUserService(id);
            bool actual = result.IsSuccessful;


            //clean up
            await request.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }
        // need frequency change test

        [TestMethod]
        public async Task ChangeUserServiceFrequency_PASS()
        {
            //arrange
            var dao = new UserServiceDAO(_configuration!);
            var request = new ServiceRequestManagement(_logger!, _requestDAO, _userServiceDAO);
            Guid id = Guid.NewGuid();

            ServiceRequest userService = new ServiceRequest(id, "Frequency Example", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(userService);

            //act
            await request.FrequencyChange(id, "3x/month", "serviceProvider@gmail.com");
            Result result = await dao.CheckFrequency(id, "3x/month");
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        //need cancellation test 

        [TestMethod]
        public async Task CancelUserService_PASS()
        {
            //arrange
            var service = new ServiceRequestDAO( _configuration!);
            var dao = new UserServiceDAO(_configuration!);
            var request = new ServiceRequestManagement(_logger!, _requestDAO, _userServiceDAO);
            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id, "Cancel Example", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await service.AddServiceRequest(userService);
            await dao.AddUserService(userService);

            //act
            Result result = await request.CancelUserService(id, "serviceProvider@gmail.com");
            bool actual = result.IsSuccessful;


            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }


    }
}