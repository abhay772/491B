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
            Console.WriteLine(result.Payload);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task AcceptARequest()
        {
            // Arrange
            var service = new RequestManagement();
            Guid id = new Guid("D0295F56-CFF3-4C1D-BA15-DE2412CDBA02");
            ServiceRequest request = new ServiceRequest(id, "Landscape", "trim palm tree leaves", "Clean", "1x/week", "nothing for now",
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
            Guid id = new Guid("36FD48D7-6963-457B-AA69-A64D78856564");

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
            Guid id = new Guid("94B97BAF-0A2B-42B7-AC26-AB2444A9900C");

            // Act
            Result result = await service.RateService(id,"propertyManager@gmail.com", 4);
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
            Guid id = new Guid("DCEA868C-DEDC-4FD2-90E8-3CCFB55A87B4");

            //act
            Result result = await service.RateService(id,"propertyManager@gmail.com", 6);
            bool actual = result.IsSuccessful;

            //assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }
    }
}

