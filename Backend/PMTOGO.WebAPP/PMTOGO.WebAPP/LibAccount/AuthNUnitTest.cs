using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMTOGO.WebAPP.LibAccount;
using PMTOGO.WebAPP.Services;

namespace PMTOGO.WebAPP.LibAccount
{
    [TestClass]
    public class AuthNUnitTest
    {
        [TestMethod] //ex test to change
        public void ShouldCreateInstanceWithDefaultCtor()
        {
            // Arrange
            var expected = typeof(UserManagement);

            // Act
            var actual = new UserManagement();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }
    }
}