using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;


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
            var service = new RequestManagement();

            // Act
            Result result = await service.GatherServiceRequest("mssierra310@gmail.com");
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
                "serviceProvider@gmail.com", "Sierra Harris", "propertyManager@gmail.com", "Sara Jade");

            // Act
            Result result = await service.AcceptRequest(id);
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
    }
}

