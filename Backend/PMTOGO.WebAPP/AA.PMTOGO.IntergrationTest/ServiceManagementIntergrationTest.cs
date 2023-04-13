using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;

namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class ServiceManagementIntergrationTest
    {
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
            Result result = await dao.GetUserService(query, "propertyManager@gmail.com");
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
            Result result = await dao.GetUserService(query, "serviceProvider@gmail.com");
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
            Result result = await dao.CheckRating(id, 4, query);
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
            Result result = await dao.CheckRating(id, 4, query);
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

        // need frequency change test

        [TestMethod]
        public async Task ChangeUserServiceFrequency_PASS()
        {
            //arrange
            var service = new UserServiceManagement();
            var dao = new UserServiceDAO();
            Guid id = Guid.NewGuid();


            ServiceRequest userService = new ServiceRequest(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(userService);

            //act
            await service.FrequnecyChange(id, "3x/month");
            Result result = await dao.CheckFrequency(id, "3x/Month");
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
            var service = new UserServiceManagement();
            var dao = new UserServiceDAO();
            var request = new ServiceRequestDAO();
            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id, "New Request", "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await dao.AddUserService(userService);

            //act
            Result result = await service.CancelUserService(id);
            bool actual = result.IsSuccessful;


            //clean up
            //await dao.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);




        }
    }
}