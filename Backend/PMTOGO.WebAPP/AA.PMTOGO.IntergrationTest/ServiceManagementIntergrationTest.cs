using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;

namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class ServiceManagementIntergrationTest
    {
        [TestMethod]
        public async Task AddAService_PASS()
        {
            // Arrange
            var dao = new ServiceDAO();
            Guid id = Guid.NewGuid();
            Service service = new Service(id, "Parking Lot Sweep", "Sweeping", "random description",
                 "Sierra Harris", "mssierra310@gmail.com", 200.50);
            // Act
            await dao.AddService(service);
            Result result = await dao.FindService(id);
            bool actual = result.IsSuccessful;


            //clean up
            await dao.DeleteService(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task GetServices_PASS()
        {
            // Arrange
            var dao = new ServiceDAO();

            // Act
            Result result = await dao.GetServices();
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task GetUserServicesPM_PASS()
        {
            // Arrange
            var dao = new UserServiceDAO();

            Guid id = Guid.NewGuid();
            ServiceRequest service = new (id,"New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(service);
            string query = "SELECT * FROM UserServices WHERE PropertyManagerEmail = @PropertyManagerEmail";
            // Act
            Result result = await dao.GetUserServices(query, "propertyManager@gmail.com", "PMRating");
            bool actual = result.IsSuccessful;


            //clean up
            await dao.DeleteUserService(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }
        [TestMethod]
        public async Task GetUserServicesSP_PASS()
        {
            // Arrange
            var dao = new UserServiceDAO();

            Guid id = Guid.NewGuid();
            ServiceRequest service = new(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(service);
            string query = "SELECT * FROM UserServices WHERE ServiceProviderEmail = @ServiceProviderEmail";
            // Act
            Result result = await dao.GetUserServices(query, "serviceProvider@gmail.com", "SPRating");
            bool actual = result.IsSuccessful;


            //clean up
            await dao.DeleteUserService(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

        }

        [TestMethod]
        public async Task AddServiceRequest_PASS()
        {
            // Arrange
            var dao = new ServiceRequestDAO();
            Guid id = Guid.NewGuid();

            ServiceRequest request = new(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
               "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            // Act

            await dao.AddServiceRequest(request);
            Result result = await dao.FindServiceRequest(id);
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task RateAUserServiceSP_PASS()
        {
            //arrange
            var service = new UserServiceManagement();
            var dao = new UserServiceDAO();

            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id,"New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(userService);
            string role = "Service Provider";
            string query = "SELECT SPRating FROM UserServices WHERE Id = @ID";
            
            //act
            await service.Rate(id, 4, role);
            Result result = await dao.CheckRating(id, 4, query, "SPRating");
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task RateAUserServicePM_PASS()
        {
            //arrange
            var service = new UserServiceManagement();
            var dao = new UserServiceDAO();

            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(userService);
            string role = "Property Manager";
            string query = "SELECT PMRating FROM UserServices WHERE Id = @ID";

            //act
            await service.Rate(id, 4, role);
            Result result = await dao.CheckRating(id, 4, query, "PMRating");
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task RateIsNotHigherThan5_FAIL()
        {
            //arrange
            var service = new UserServiceManagement();
            var dao = new UserServiceDAO();
            Guid id = Guid.NewGuid();

            ServiceRequest userService = new ServiceRequest(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(userService);

            string role = "Service Provider";

            //act
            Result result = await service.Rate(id, 6, role);
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);


        }

    }
}