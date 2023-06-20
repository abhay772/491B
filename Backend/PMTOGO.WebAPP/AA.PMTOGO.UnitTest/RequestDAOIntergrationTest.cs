using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.UnitTest
{
    [TestClass]
    public class RequestDAOIntergrationTest
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
            var dao = new RequestDAO();

            // Act
            await service.AcceptRequest(request);
            Result result = await dao.FindService(id, "propertyManager@gmail.com");
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
            Guid id = new Guid("36FD48D7-6963-457B-AA69-A64D78856564");

            // Act
            await service.DeclineRequest(id, "serviceProvider@gmail.com");
            Result result = await dao.FindRequest(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }
        [TestMethod]
        public async Task RateARequest()
        {
            // Arrange
            var service = new RequestManagement();
            var dao = new RequestDAO();
            Guid id = new Guid("94B97BAF-0A2B-42B7-AC26-AB2444A9900C");
            int rate = 4;

            // Act
            await service.RateService(id, "propertyManager@gmail.com", rate);
            Result result = await dao.CheckRating(id, "propertyManager@gmail.com", rate);
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
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            var request = new UserService(id, "Landscape", "soil installation ", "material delivery", "1x/month",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            // Act
            await service.CreateService(request);
            Result result = await dao.FindService(id, "propertyManager@gmail.com");
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
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Landscape", "material delivery", "soil installation ", "1x/month", "planters is far left of yard",
                "serviceProvider@gmail.com", "Sierra Harris", "propertyManager@gmail.com", "Sara Jade");

            // Act
            Result res = await service.CreateRequest(request);
            Result result = await dao.FindRequest(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
    }
}
