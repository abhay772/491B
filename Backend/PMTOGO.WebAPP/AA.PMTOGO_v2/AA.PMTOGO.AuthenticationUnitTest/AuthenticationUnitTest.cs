
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using AA.PMTOGO.Authentication;
//using AA.PMTOGO.Authentications;

namespace AA.PMTOGO.AuthenticationTest
{
    [TestClass]
    public class AuthenticationUnitTest
    {
        public void ShouldCreateInstanceWithDefaultCtor()
        {
            // Arrange
            var expected = typeof(Authenticator);

            // Act
            var actual = new Authenticator();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        // The user must provide valid security credentials whenever attempting to authenticate with the system

        // Valid username
        [TestMethod]
        public void ShouldProvideValidUsername()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkValidUsername = authenticator.ValidateUsername("Abc.-@123").IsSuccessful;
            bool checkInvalidMinimumUsernameLength = authenticator.ValidateUsername("abc123").IsSuccessful;
            bool checkInvalidMaximumUsernameLength = authenticator.ValidateUsername("abcdefghijklmnopqrstuvwxyz01234567890abcdefghijklmnopqrstuvwxyz123").IsSuccessful;
            bool checkInvalidUsernameCharacters = authenticator.ValidateUsername("Abc.-@123]").IsSuccessful;
            bool checkBlankUsername = authenticator.ValidateUsername(null).IsSuccessful;

            // Assert
            Assert.IsNotNull(checkValidUsername);
            Assert.IsNotNull(checkInvalidMinimumUsernameLength);
            Assert.IsNotNull(checkInvalidMaximumUsernameLength);
            Assert.IsNotNull(checkInvalidUsernameCharacters);
            Assert.IsNotNull(checkBlankUsername);

            Assert.IsTrue(checkValidUsername);
            Assert.IsFalse(checkInvalidMinimumUsernameLength);
            Assert.IsFalse(checkInvalidMaximumUsernameLength);
            Assert.IsFalse(checkInvalidUsernameCharacters);
            Assert.IsFalse(checkBlankUsername);
        }

        // Valid password
        [TestMethod]
        public void ShouldProvideValidPassphrase()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkValidPassphrase = authenticator.ValidatePassphrase("aZ09  .,@!").IsSuccessful;
            bool checkPassphraseNotValidCharacters = authenticator.ValidatePassphrase("[apple]").IsSuccessful;
            bool checkPassphraseNotValidlength = authenticator.ValidatePassphrase("pass123").IsSuccessful;


            // Assert
            Assert.IsNotNull(checkValidPassphrase);
            Assert.IsNotNull(checkPassphraseNotValidCharacters);
            Assert.IsNotNull(checkPassphraseNotValidlength);
            Assert.IsTrue(checkValidPassphrase);
            Assert.IsFalse(checkPassphraseNotValidCharacters);
            Assert.IsFalse(checkPassphraseNotValidlength);
        }

        //Valid OTP dont have this function
        [TestMethod]
        public void ShouldProvideValidOTP()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkValidOTP = authenticator.CheckValidOTP("abcABC123");
            bool checkInvalidMinimumOTPLength = authenticator.CheckValidOTP("abc123");
            bool checkInvalidMaximumOTPLength = authenticator.CheckValidOTP("abcdefghijklmnopqrstuvwxyz01234567890abcdefghijklmnopqrstuvwxyz123");
            bool checkInvalidOTPCharacters = authenticator.CheckValidOTP("abcABC123");
            bool checkBlankOTP = authenticator.CheckValidOTP(null);

            // Assert
            Assert.IsNotNull(checkValidOTP);
            Assert.IsNotNull(checkInvalidMinimumOTPLength);
            Assert.IsNotNull(checkInvalidMaximumOTPLength);
            Assert.IsNotNull(checkInvalidOTPCharacters);
            Assert.IsNotNull(checkBlankOTP);

            Assert.IsTrue(checkValidOTP);
            Assert.IsFalse(checkInvalidMinimumOTPLength);
            Assert.IsFalse(checkInvalidMaximumOTPLength);
            Assert.IsFalse(checkInvalidOTPCharacters);
            Assert.IsFalse(checkBlankOTP);
        }

        [TestMethod]
        public void ShouldGenerateValidOTP()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            var checkGenerateOTP = authenticator.GenerateOTP();


            // Assert
            Assert.IsNotNull(checkGenerateOTP);
            Assert.IsTrue(checkGenerateOTP.Length >= 8);
            Assert.IsTrue(checkGenerateOTP.Length <= 64);
            Assert.IsTrue(Regex.IsMatch(checkGenerateOTP, @"^[a-zA-Z0-9\s]+$"));
        }


        [TestMethod]
        public void ShouldAuthenticateUser()
        {
            // Arrange
            var authenticator = new Authenticator();
            string pass = "weakpass";
            
            // Act
            bool checkValidUserAuthentication = authenticator.Authenticate("username@gmail.com", pass).IsSuccessful;
            bool checkInvalidUsernameAuthentication = authenticator.Authenticate("abc", pass).IsSuccessful;
            bool checkInvalidOTPAuthentication = authenticator.Authenticate("username@gmail.com", "abc").IsSuccessful;
            bool checkNullUserAuthentication = authenticator.Authenticate(null, null).IsSuccessful;

            // Asserrt
            Assert.IsNotNull(checkValidUserAuthentication);
            Assert.IsNotNull(checkInvalidUsernameAuthentication);
            Assert.IsNotNull(checkInvalidOTPAuthentication);
            Assert.IsNotNull(checkNullUserAuthentication);

            Assert.IsTrue(checkValidUserAuthentication);
            Assert.IsFalse(checkInvalidUsernameAuthentication);
            Assert.IsFalse(checkInvalidOTPAuthentication);
            Assert.IsFalse(checkNullUserAuthentication);
        }

        [TestMethod]
        public void ShouldLockAccount()
        {
            // Arrange
            var authenticator = new Authenticator();

            

            // Act
            bool failAuthenticationFirstTime = authenticator.Authenticate("Username@gmail.com", "abc").IsSuccessful;
            bool failAuthenticationSecondTime = authenticator.Authenticate("Username@gmail.com", "abc").IsSuccessful;
            bool failAuthenticationThirdTime = authenticator.Authenticate("Username@gmail.com", "abc").IsSuccessful;
           
            // Assert
            Assert.IsNotNull(failAuthenticationFirstTime);
            Assert.IsNotNull(failAuthenticationSecondTime);
            Assert.IsNotNull(failAuthenticationThirdTime);

            Assert.IsFalse(failAuthenticationFirstTime);
            Assert.IsFalse(failAuthenticationSecondTime);
            Assert.IsFalse(failAuthenticationThirdTime);

        }

    }
}
