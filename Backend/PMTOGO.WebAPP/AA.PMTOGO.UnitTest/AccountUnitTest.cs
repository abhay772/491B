using AA.PMTOGO.Libary;
using AA.PMTOGO.Services;
using System.Diagnostics;

namespace AA.PMTOGO.UnitTest
{
    [TestClass]
    public class AccountUnitTest
    {
        [TestMethod]
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

        [TestMethod]
        // should provide sytstem-wide unique username
        public void ShouldAssignUniqueUsername()
        {
            // Arrange
            var registration = new UserManagement();
           //username = email
           
            bool accountCreated = registration.CreateAccount("user@gmail.com", "randomstring", "John", "Doe", "Property Manager").IsSuccessful;

            // Act
            bool account2Created = registration.CreateAccount("user@gmail.com", "randompass", "Jo", "De", "Property Manager").IsSuccessful;
            
            // Assert
            Assert.IsNotNull(accountCreated);
            Assert.IsNotNull(account2Created);
            Assert.IsTrue(accountCreated);
            Assert.IsFalse(account2Created);
        }

        //The user provides a valid email address that belongs to the user.
        [TestMethod]
        public void ShouldProvideValidEmail()
        {
            // Arrange
            var input = new InputValidation();
            //var result = new Result();

            // Act
            bool checkValidEmail = input.ValidateEmail("ABC@gmail.com").IsSuccessful;
            bool checkNotValidEmailCharacters = input.ValidateEmail("12345[]").IsSuccessful;
            bool checkNotValidEmailFormat = input.ValidateEmail("ABCgmailcom").IsSuccessful;
            bool checkNotValidEmailLength = input.ValidateEmail("1").IsSuccessful;

            // Assert
            Assert.IsNotNull(checkValidEmail);
            Assert.IsNotNull(checkNotValidEmailCharacters);
            Assert.IsNotNull(checkNotValidEmailFormat);
            Assert.IsNotNull(checkNotValidEmailLength);
            Assert.IsTrue(checkValidEmail);
            Assert.IsFalse(checkNotValidEmailCharacters);
            Assert.IsFalse(checkNotValidEmailFormat);
            Assert.IsFalse(checkNotValidEmailLength);
        }
        //The user is provided with a valid username
        [TestMethod]
        public void ShouldProvideValidUsername()
        {
            // Arrange
            var input = new InputValidation();

            // Act
            bool checkValidUsername = input.ValidateUsername("abc@gmail.com").IsSuccessful;
            bool checkNotValidUsernameCharacters = input.ValidateUsername("12345AB[]").IsSuccessful;
            bool checkNotValidUsernameFormat = input.ValidateUsername("ABCgmailcom").IsSuccessful;
            bool checkNotValidUsernameLength = input.ValidateUsername("1").IsSuccessful;

            // Assert
            Assert.IsNotNull(checkValidUsername);
            Assert.IsNotNull(checkNotValidUsernameCharacters);
            Assert.IsNotNull(checkNotValidUsernameFormat);
            Assert.IsNotNull(checkNotValidUsernameLength);
            Assert.IsTrue(checkValidUsername);
            Assert.IsFalse(checkNotValidUsernameCharacters);
            Assert.IsFalse(checkNotValidUsernameFormat);
            Assert.IsFalse(checkNotValidUsernameLength);
        }

        // The user provides a secret passphrase for requesting OTP
        [TestMethod]
        public void ShouldProvideValidPassphrase()
        {
            // Arrange
            var input = new InputValidation();

            // Act
            bool checkValidPassphrase = input.ValidatePassphrase("aZ09  .,@-!").IsSuccessful;
            bool checkPassphraseNotValidCharacters = input.ValidatePassphrase("[apple]").IsSuccessful;
            bool checkPassphraseNotValidlength = input.ValidatePassphrase("pass123").IsSuccessful;


            // Assert
            Assert.IsNotNull(checkValidPassphrase);
            Assert.IsNotNull(checkPassphraseNotValidCharacters);
            Assert.IsNotNull(checkPassphraseNotValidlength);
            Assert.IsTrue(checkValidPassphrase);
            Assert.IsFalse(checkPassphraseNotValidCharacters);
            Assert.IsFalse(checkPassphraseNotValidlength);
        }

        // The user should provide valid age
        [TestMethod]
        public void ShouldProvideValidAge()
        {
            // Arrange
            var input = new InputValidation();
            DateTime validAge = new DateTime(2000, 1, 1);
            DateTime invalidAge = DateTime.Now;

            // Act
            bool checkValidAge = input.ValidateDateOfBirth(validAge).IsSuccessful;
            bool checkInvalidAge = input.ValidateDateOfBirth(invalidAge).IsSuccessful;

            // Assert
            Assert.IsNotNull(checkValidAge);
            Assert.IsNotNull(checkInvalidAge);
            Assert.IsFalse(checkInvalidAge);
            Assert.IsTrue(checkValidAge);


        }
        [TestMethod]
        public void ShouldCreateAccountWithin5Seconds()
        {
            //aranage

            var registration = new UserManagement();

            //act
            var time = Stopwatch.StartNew();
            bool OnTime = registration.CreateAccount("ggnamegmail.com", "randompass", "John", "Doe", "Property Manager").IsSuccessful;
            time.Stop();
            var second = time.ElapsedMilliseconds / 1000;
            if (second < 5)
            {
                OnTime = true;
            }

            var timer = Stopwatch.StartNew();
            Thread.Sleep(6000);
            bool OverTime = registration.CreateAccount("namegmail.com", "randomstring", "John", "Doe", "Property Manager").IsSuccessful;
            timer.Stop();
            var seconds = timer.ElapsedMilliseconds / 1000;
            if (seconds > 5)
            {
                OverTime = true;
            }
                //private info

                //assert
            Assert.IsNotNull(OnTime);
            Assert.IsNotNull(OverTime);
            Assert.IsTrue(OnTime);
            Assert.IsTrue(OverTime);

        }
    }
}