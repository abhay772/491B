
using AA.PMTOGO.Authentication;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Managers;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.UnitTest
{
    [TestClass]
    public class LoggerUnitTest
    {
        [TestMethod]
        public void ShouldCreateInstanceWithDefaultCtor()
        {
            // Arrange
            var expected = typeof(Logger);


            // Act

            var actual = new Logger();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldCreateLogObject()
        {
            // Arrange
            var expected = typeof(Log);


            // Act

            var actual = new Log();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public void ShouldCreateLoadedLogObject()
        {
            // Arrange
            var expected = typeof(Log);


            // Act

            var actual = new Log("Authenticate", 4, "Server", "message");
            Console.WriteLine(actual.LogId);
            Console.WriteLine(actual.Timestamp);
            Console.WriteLine(actual.Category);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        [TestMethod]
        public async Task ShouldLogtoDatabase_PASS()
        {
            // Arrange
            var _logger = new Logger();

            // Act
            Result message = new Result();
            message.ErrorMessage= "test result";

            Result result = await _logger.Log("Authenticate", 4, LogCategory.Server, message);
            var actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public async Task ShouldAuthenticateUser_PASS()
        {
            // Arrange
            var auth = new Authenticator();
            var _auth = new AuthManager(auth);

            // Act
            Result result = await _auth.Login("mssierra310@gmail.com","randomstring");
            var actual = result.IsSuccessful;

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }
    }
}
