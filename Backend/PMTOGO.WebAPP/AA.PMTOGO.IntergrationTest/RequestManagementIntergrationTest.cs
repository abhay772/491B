using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class RequestManagementIntergrationTest // RequestDAO only test
    {
        [TestMethod]
        public async Task GetServiceRequest()
        {
            // Arrange
            var dao = new ServiceRequestDAO();
            var service = new UserServiceDAO();
            Guid id = Guid.NewGuid();
            await service.AddServiceRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

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
            var dao = new ServiceRequestDAO();
            var service = new UserServiceDAO();
            Guid id = Guid.NewGuid();
            await service.AddServiceRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month","random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            // Act
            await dao.DeleteServiceRequest(id);
            Result result = await dao.FindServiceRequest(id);
            bool actual = result.IsSuccessful;

            //clean up
            await service.DeleteUserService(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);


        }

        [TestMethod]
        public async Task DeclineServiceRequest_Pass()
        {
            // Arrange
            var dao = new ServiceRequestDAO();
            var service = new UserServiceDAO();
            Guid id = Guid.NewGuid();
            await service.AddServiceRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

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
            var userService = new UserServiceDAO();
            var request = new ServiceRequestDAO();

            Guid id = Guid.NewGuid();

            // Act
            await request.AddUserService(id, "Landscape", "material delivery", "soil installation ", "1x/month",
                "mssierra310@gmail.com", "Sara Jade", "sierra.harris01@student.csulb.edu", "Sierra Harris");
            Result result = await userService.FindUserService(id);
            bool actual = result.IsSuccessful;


            //clean up
            await request.DeleteServiceRequest(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }
      

    }
}
