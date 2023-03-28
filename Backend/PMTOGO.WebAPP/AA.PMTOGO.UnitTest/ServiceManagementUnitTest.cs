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
    public class ServiceManagementUnitTest
    {
        [TestMethod]
        public void CreateServiceManagementInstance()
        {
            // Arrange
            var expected = typeof(ServiceManagement);

            // Act
            var actual = new ServiceManagement();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }
        [TestMethod]
        public async Task GetServiceService()
        {
            // Arrange
            var dao = new RequestDAO();

            // Act
            Result result = await dao.GetServices();
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task AddAServiceRequest()
        {
            // Arrange
            ServiceManagement service = new ServiceManagement();
            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Landscape", "material delivery", "soil installation ", "1x/month", "planters is far left of yard",
                "mssierra310@gmail.com", "Sierra Harris", "propertyManager@gmail.com", "Sara Jade");

            // Act
            Result result = await service.AddRequest(request);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task RateIsNotHigherThan5()
        {
            //arrange
            var service = new ServiceManagement();
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            await dao.AddUserService(id, "Landscape", "soil installation ", "material delivery", "1x/month",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            //act
            Result result = await service.RateService(id, 6);
            bool actual = result.IsSuccessful;

            //assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }
        [TestMethod]
        public async Task RateAUserService()
        {
            //arrange
            var service = new ServiceManagement();
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            await dao.AddUserService(id, "Landscape", "soil installation ", "material delivery", "1x/month",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            //act
            Result result = await service.RateService(id, 4);
            bool actual = result.IsSuccessful;

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
    }
}
