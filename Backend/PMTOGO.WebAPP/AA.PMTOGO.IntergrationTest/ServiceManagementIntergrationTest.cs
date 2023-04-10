using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;

namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class ServiceManagementIntergrationTest
    {
        [TestMethod]
        public async Task GetUserServices_PASS()
        {
            // Arrange
            var dao = new UserServiceDAO();
            var request = new ServiceRequestDAO();

            Guid id = Guid.NewGuid();
            ServiceRequest service = new ServiceRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await request.AddUserService(service);
            // Act
            Result result = await dao.GetUserService("serviceProvider@gmail.com", "serviceProvider@gmail.com");
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
            var service = new UserServiceDAO();
            Guid id = Guid.NewGuid();

            // Act
            await service.AddServiceRequest(id, "Landscape", "material delivery", "soil installation ", "2x/month", "planters is far left of yard",
                "mssierra310@gmail.com", "Sierra Harris", "sierra.harris01@student.csulb.edu", "Sara Jade");
            Result result = await dao.FindServiceRequest(id);
            bool actual = result.IsSuccessful;

            //clean up
            //await dao.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task RateAUserService_PASS()
        {
            //arrange
            var service = new UserServiceManagement();
            var dao = new UserServiceDAO();
            var request = new ServiceRequestDAO();
            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await request.AddUserService(userService);

            //act
            await service.RateService(id, 4);
            Result result = await dao.CheckRating(id, 4);
            bool actual = result.IsSuccessful;

            //clean up
            //await dao.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task RateIsNotHigherThan5_FAIL()
        {
            //arrange
            var service = new UserServiceManagement();
            var request = new ServiceRequestDAO();
            var dao = new UserServiceDAO();
            Guid id = Guid.NewGuid();
            ServiceRequest userService = new ServiceRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            await request.AddUserService(userService);

            //act
            Result result = await service.RateService(id, 6);
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteUserService(id);

            //assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);


        }
        // need frequency change test

        //need cancellation test 
    }
}