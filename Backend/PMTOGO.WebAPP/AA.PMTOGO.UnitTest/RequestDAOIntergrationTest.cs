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
            Result result = await dao.GetServiceRequest("serviceProvider@gmail.com");
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
            Result res = await dao.AddUserService(id, "Landscape", "material delivery", "soil installation ", "1x/month",
                "mssierra310@gmail.com", "Sara Jade", "sierra.harris01@student.csulb.edu", "Sierra Harris");
            Result result = await dao.FindUserService(id);
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
            await dao.AddRequest(id, "Landscape", "material delivery", "soil installation ", "2x/month", "planters is far left of yard",
                "mssierra310@gmail.com", "Sierra Harris", "sierra.harris01@student.csulb.edu", "Sara Jade");
            Result result = await dao.FindRequest(id);
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
      
        [TestMethod]
        public async Task AddAService()
        {
            // Arrange
            var dao = new RequestDAO();

            // Act
            await dao.AddService("Parking Lot Sweep", "Sweeping", "random description",
                "mssierra310@gmail.com", "Sierra Harris");
            Result result = await dao.FindService("Parking Lot Sweep", "Sweeping", "random description");
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
    }
}
