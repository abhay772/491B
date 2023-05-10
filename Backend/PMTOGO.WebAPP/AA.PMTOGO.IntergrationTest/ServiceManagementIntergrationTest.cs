using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using Microsoft.Extensions.Configuration;

namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class ServiceManagementIntergrationTest
    {
        private readonly IConfiguration? _configuration;
       


        [TestMethod]
        public async Task AddAService_PASS()
        {
            ServiceDAO _serviceDAO = new ServiceDAO(_configuration!);
            // Arrange

            Guid id = Guid.NewGuid();
            Service service = new Service(id, "Steam Cleaning", "Pressure Wash", "Cleans hard surfaces such as sidewalks, exterior building walls, or walkways",
                 "Sierra Harris", "mssierra310@gmail.com", 450.50);
            // Act
            await _serviceDAO.AddService(service);
            Result result = await _serviceDAO.FindService(id);
            bool actual = result.IsSuccessful;


            //clean up
            await _serviceDAO.DeleteService(id);


            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task GetServices_PASS()
        { 
            ServiceDAO _serviceDAO = new ServiceDAO(_configuration!);

            // Arrange
            Result result = await _serviceDAO.GetServices();

            // Act
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task GetUserServicesPM_PASS()
        {

            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);

            // Arrange
            Guid id = Guid.NewGuid();
            ServiceRequest service = new (id,"New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _userServiceDAO.AddUserService(service);
            string query = "SELECT * FROM UserServices WHERE PropertyManagerEmail = @PropertyManagerEmail";

            // Act
            Result result = await _userServiceDAO.GetUserServices(query, "propertyManager@gmail.com", "PMRating");
            bool actual = result.IsSuccessful;


            //clean up
            await _userServiceDAO.DeleteUserService(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }
        [TestMethod]
        public async Task GetUserServicesSP_PASS()
        {
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
 
            // Arrange

            Guid id = Guid.NewGuid();
            ServiceRequest service = new(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _userServiceDAO.AddUserService(service);
            string query = "SELECT * FROM UserServices WHERE ServiceProviderEmail = @ServiceProviderEmail";

            // Act
            Result result = await _userServiceDAO.GetUserServices(query, "serviceProvider@gmail.com", "SPRating");
            bool actual = result.IsSuccessful;

            //clean up
            await _userServiceDAO.DeleteUserService(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }

        [TestMethod]
        public async Task AddServiceRequest_PASS()
        {

            ServiceRequestDAO _serviceRequestDAO = new ServiceRequestDAO(_configuration!);
            // Arrange
            Guid id = Guid.NewGuid();

            ServiceRequest request = new(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
               "mssierra310@gmail.com", "Sara Jade", "sierra.harris01@student.csulb.edu", "Sierra Harris");

            // Act

            await _serviceRequestDAO.AddServiceRequest(request);
            Result result = await _serviceRequestDAO.FindServiceRequest(id);
            bool actual = result.IsSuccessful;

            //clean up
            await _serviceRequestDAO.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }

        [TestMethod]
        public async Task RateAUserServiceSP_PASS()
        {
            LoggerDAO logdao = new LoggerDAO(_configuration!);
            UsersDAO _usersDAO = new UsersDAO(_configuration!);
            ServiceDAO _serviceDAO = new ServiceDAO(_configuration!);
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _serviceRequestDAO = new ServiceRequestDAO(_configuration!);
            //arrange
            Logger _logger = new Logger(logdao);
            var service = new UserServiceManagement(_usersDAO, _serviceDAO, _userServiceDAO, _serviceRequestDAO, _logger!);

            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id,"New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _userServiceDAO.AddUserService(userService);
            string role = "Service Provider";
            string query = "SELECT SPRating FROM UserServices WHERE Id = @ID";
            
            //act
            await service.Rate(id, 4, role);
            Result result = await _userServiceDAO.CheckRating(id, 4, query, "SPRating");
            bool actual = result.IsSuccessful;

            //clean up
            await _userServiceDAO.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public async Task RateAUserServicePM_PASS()
        {
            LoggerDAO logdao = new LoggerDAO(_configuration!);
            UsersDAO _usersDAO = new UsersDAO(_configuration!);
            ServiceDAO _serviceDAO = new ServiceDAO(_configuration!);
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _serviceRequestDAO = new ServiceRequestDAO(_configuration!);
            //arrange
            Logger _logger = new Logger(logdao);
            var userserviceM = new UserServiceManagement(_usersDAO, _serviceDAO, _userServiceDAO, _serviceRequestDAO, _logger!);

            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _userServiceDAO.AddUserService(userService);
            string role = "Property Manager";
            string query = "SELECT PMRating FROM UserServices WHERE Id = @ID";

            //act
            await userserviceM.Rate(id, 4, role);
            Result result = await _userServiceDAO.CheckRating(id, 4, query, "PMRating");
            bool actual = result.IsSuccessful;

            //clean up
            await _userServiceDAO.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task RateIsNotHigherThan5_FAIL()
        {
            LoggerDAO logdao = new LoggerDAO(_configuration!);
            UsersDAO _usersDAO = new UsersDAO(_configuration!);
            ServiceDAO _serviceDAO = new ServiceDAO(_configuration!);
            UserServiceDAO _userServiceDAO = new UserServiceDAO(_configuration!);
            ServiceRequestDAO _serviceRequestDAO = new ServiceRequestDAO(_configuration!);
            //arrange
            Logger _logger = new Logger(logdao);
            var service = new UserServiceManagement(_usersDAO, _serviceDAO, _userServiceDAO, _serviceRequestDAO, _logger!);

            Guid id = Guid.NewGuid();

            ServiceRequest userService = new ServiceRequest(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await _userServiceDAO.AddUserService(userService);

            string role = "Service Provider";

            //act
            Result result = await service.Rate(id, 6, role);
            bool actual = result.IsSuccessful;

            //clean up
            await _userServiceDAO.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }

    }
}