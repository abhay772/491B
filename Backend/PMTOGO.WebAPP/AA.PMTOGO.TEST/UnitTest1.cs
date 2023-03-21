using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;

namespace AA.PMTOGO.TEST
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
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
    }
}