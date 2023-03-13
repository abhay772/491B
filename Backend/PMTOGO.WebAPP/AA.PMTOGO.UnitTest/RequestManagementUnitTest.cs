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
        public async void AcceptARequest()
        {
            // Arrange
            var service = new RequestManagement();
            Guid id = Guid.NewGuid();
            ServiceRequest request = new ServiceRequest(id, "Landscape", "trim palm tree leaves", "Clean", "1x/week", "nothing for now",
                "mssierra310@gmail.com", "Sierra Harris","mssierr2001@gmail.com", "Sierra Harris");

            // Act
            Result result = await service.AcceptRequest(request);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async void DeclineARequest()
        {
            // Arrange
            var service = new RequestManagement();
            Guid id = new Guid("36FDA2F7-FC46-4687-AEA8-22DF364688140");

            // Act
            Result result = await service.DeclineRequest(id,"mssierra310@gmail.com");
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        /*[TestMethod]
        public async void RateARequest()
        {
            // Arrange
            var service = new RequestManagement();
            Guid id = //id from database

            // Act
            //Result result = await service.RateService(id, 4);
            //bool actual = result.IsSuccesfull;

            // Assert
            //Assert.IsNotNull(actual);
            //Assert.IsTrue(actual);
        }*/
        [TestMethod]
        public async void AddAUserService()
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
        public async void AddAServiceRequest()
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
        /*[TestMethod]
        public void RateIsNotHigherThan5()
        {
            //arrange
            var service = new RequestManagement();
            //Guid id = 

            //act
            //Result result = await service.RateService(id, 6);
            //bool actual = result.IsSuccesfull;

            //Assert.IsNotNull(actual);
            //Assert.IsFalse(actual);
        }*/
    }
}

