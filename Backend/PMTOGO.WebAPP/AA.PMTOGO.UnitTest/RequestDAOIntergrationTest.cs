using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.UnitTest
{
    [TestClass]
    public class RequestDAOIntergrationTest // RequestDAO only test
    {
        [TestMethod]
        public void CreateRequestDAOInstance()
        {
            // Arrange
            var expected = typeof(RequestDAO);

            // Act
            var actual = new RequestDAO();

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
        public async Task GetServices()
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
        public async Task DeclineARequest()
        {
            // Arrange
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();
            await dao.AddRequest(id, "Landscape", "soil installation ", "material delivery", "1x/month","random comment",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");

            // Act
            await dao.DeleteServiceRequest(id);
            Result result = await dao.FindRequest(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
        }
        [TestMethod]
        public async Task AddAUserService()
        {
            // Arrange
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();

            // Act
            Result res = await dao.AddUserService(id, "Landscape", "soil installation ", "material delivery", "1x/month",
                "serviceProvider@gmail.com", "Sara Jade", "propertyManager@gmail.com", "Sierra Harris");
            Result result = await dao.FindService(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task AddAServiceRequest()
        {
            // Arrange
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();

            // Act
            Result res = await dao.AddRequest(id, "Landscape", "material delivery", "soil installation ", "1x/month", "planters is far left of yard",
                "serviceProvider@gmail.com", "Sierra Harris", "propertyManager@gmail.com", "Sara Jade");
            Result result = await dao.FindRequest(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        //need add service method and find service method
        /*[TestMethod]
        public async Task AddAService()
        {
            // Arrange
            var dao = new RequestDAO();
            Guid id = Guid.NewGuid();

            // Act
            Result res = await dao.AddService("Landscape", "material delivery", "soil installation ", "1x/month",
                "serviceProvider@gmail.com", "Sierra Harris", "propertyManager@gmail.com", "Sara Jade");
            Result result = await dao.FindService(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }*/
    }
}
