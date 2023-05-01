using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.IntergrationTest
{
    public class SPServicesIntergrationTest
    {
        [TestMethod]
        public async Task AddAService_PASS()
        {
            // Arrange
            var dao = new ServiceDAO();

            // Act
            await dao.AddService("Parking Lot Sweep", "Sweeping", "random description",
                "mssierra310@gmail.com", "Sierra Harris");
            Result result = await dao.FindService("Parking Lot Sweep", "Sweeping", "random description");
            bool actual = result.IsSuccessful;

            //clean up
            await dao.DeleteService("Parking Lot Sweep", "Sweeping", "mssierra310@gmail.com");

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);


        }

        [TestMethod]
        public async Task GetServices_PASS()
        {
            // Arrange
            var dao = new ServiceDAO();

            // Act
            Result result = await dao.GetServices();
            bool actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
    }
}
