using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;


namespace AA.PMTOGO.UnitTest
{
    //clean up test
    [TestClass]
    public class RequestManagementUnitTest
    {

  
        private readonly ILogger? _logger;

        public RequestManagementUnitTest( ILogger logger)
        {
            _logger = logger;
        }
        [TestMethod]
        public void CreateRequestManagementInstance()
        {
            // Arrange
            var expected = typeof(ServiceRequestManagement);

            // Act
            var actual = new ServiceRequestManagement(_logger!);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

    }
}

