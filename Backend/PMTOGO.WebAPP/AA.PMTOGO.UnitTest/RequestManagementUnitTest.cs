using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using System.Diagnostics;

namespace AA.PMTOGO.UnitTest
{
    [TestClass]
    public class RequestManagementUnitTest
    {
        [TestMethod]
        public void CreateRequestManagementInstance()
        {
            // Arrange
            var expected = typeof(RequestManagement);

            // Act
            var actual = new RequestManagement();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }
        [TestMethod]
        public async Task GetServiceRequest()
        {
            // Arrange
            var dao = new RequestDAO();

            // Act
            Result result = await dao.GetUserRequest("serviceProvider@gmail.com");
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task AcceptARequest()
        {
            // Arrange
            var service = new RequestManagement();
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            await dao.AddRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month","no comment for now",
                 "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            ServiceRequest request = new ServiceRequest(id, "Landscape", "trim palm tree leaves", "Clean", "1x/week", "no comment for now",
                "serviceProvider@gmail.com", "Sierra Harris", "Sara Jade", "propertyManager@gmail.com");

            // Act
            Result result = await service.AcceptRequest(request);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task DeclineARequest()
        {
            // Arrange
            var service = new RequestManagement();
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            await dao.AddRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month", "random comment",
                 "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            // Act
            Result result = await service.DeclineRequest(id, "serviceProvider@gmail.com");
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task RateARequest()
        {
            // Arrange
            var service = new RequestManagement();
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
           await dao.AddService(id, "Landscape", "soil installation ", "material delivery", "1x/month",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            // Act
            Result result = await service.RateService(id, 4);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task AddAUserService()
        {
            // Arrange
            var service = new RequestManagement();
            Guid id = Guid.NewGuid();
            var request = new UserService(id,"Landscape", "soil installation ", "material delivery", "1x/month",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            // Act
            Result result = await service.CreateService(request);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task AddAServiceRequest()
        {
            // Arrange
            RequestManagement service = new RequestManagement();
            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Landscape", "material delivery", "soil installation ", "1x/month", "planters is far left of yard", 
                "serviceProvider@gmail.com", "Sierra Harris", "propertyManager@gmail.com", "Sara Jade");

            // Act
            Result result = await service.CreateRequest(request);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task RateIsNotHigherThan5()
        {
            //arrange
            var service = new RequestManagement();
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            await dao.AddService(id, "Landscape", "soil installation ", "material delivery", "1x/month",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            //act
            Result result = await service.RateService(id, 6);
            bool actual = result.IsSuccessful;

            //assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }
    }
}

