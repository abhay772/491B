namespace AccountRecoveryUnitTest
{
    [TestClass]
    public class AccountRecoveryTests
    {
        private AccountRecovery _accountRecovery;

        [TestInitialize]
        public void Setup()
        {
            _accountRecovery = new AccountRecovery();
        }

        [TestMethod]
        public void GenerateOTP_Returns8CharacterString()
        {
            // Act
            var result = _accountRecovery.GenerateOTP();

            // Assert
            Assert.AreEqual(8, result.Length);
        }



        [TestMethod]
        public async Task ValidateOTP_InvalidOTP_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var otp = "invalid-otp";
            _accountRecovery.GenerateOTP();

            // Act
            var result = await _accountRecovery.ValidateOTP(email, otp);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task AutomaticEmail_ValidEmailAndOTP_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var otp = "12345678";

            // Act
            var result = await _accountRecovery.AutomaticEmail(email, otp);

            // Assert
            Assert.IsTrue(result);
        }
    }
}