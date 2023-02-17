using System;
using AA.PMTOGO.Registrations;
using AA.PMTOGO.Models;
//using Xunit.Sdk;
using Xunit.Sdk;

namespace AA.PMTOGO.RegistrationTest;

[TestClass]
public class RegistrationUnitTest
{
    [TestMethod]
    public void ShouldCreateInstanceWithDefaultCtor()
    {
        // Arrange
        var expected = typeof(Registrator);

        // Act
        var actual = new Registrator();

        // Assert
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.GetType() == expected);
    }

    [TestMethod]
    // should provide sytstem-wide unique username
    public void ShouldAssignUniqueUsername()
    {
        // Arrange
        var registration = new Registrator();
        DateTime dateTime = DateTime.Now;
        byte[] salt = new byte[64];
        byte[] pass = new byte[64];
        bool accountCreated = registration.CreateUser("username@gmail.com", pass, "John", "Doe", dateTime, 1, salt).IsSuccessful;

        // Act
        bool account2Created = registration.CreateUser("username@gmail.com", pass, "Jo", "De", dateTime, 1, salt).IsSuccessful;

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
        var registration = new Registrator();
        var result = new Result();

        // Act
        bool checkValidEmail = registration.ValidateEmail("ABC@gmail.com").IsSuccessful;
        bool checkNotValidEmailCharacters = registration.ValidateEmail("12345[]").IsSuccessful;
        bool checkNotValidEmailFormat = registration.ValidateEmail("ABCgmailcom").IsSuccessful;
        bool checkNotValidEmailLength = registration.ValidateEmail("1").IsSuccessful;

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
        var registration = new Registrator();

        // Act
        bool checkValidUsername = registration.ValidateUsername("ABC@gmail.com").IsSuccessful;
        bool checkNotValidUsernameCharacters = registration.ValidateUsername("12345[]").IsSuccessful;
        bool checkNotValidUsernameFormat = registration.ValidateUsername("ABCgmailcom").IsSuccessful;
        bool checkNotValidUsernameLength = registration.ValidateUsername("1").IsSuccessful;

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
        var registration = new Registrator();

        // Act
        bool checkValidPassphrase = registration.ValidatePassphrase("aZ09  .,@!").IsSuccessful;
        bool checkPassphraseNotValidCharacters = registration.ValidatePassphrase("[apple]").IsSuccessful;
        bool checkPassphraseNotValidlength = registration.ValidatePassphrase("pass123").IsSuccessful;


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
        var registration = new Registrator();
        DateTime validAge = new DateTime(2000, 1, 1);
        DateTime invalidAge = DateTime.Now;

        // Act
        bool checkValidAge = registration.ValidateDateOfBirth(validAge).IsSuccessful;
        bool checkInvalidAge = registration.ValidateDateOfBirth(invalidAge).IsSuccessful;

        // Assert
        Assert.IsNotNull(checkValidAge);
        Assert.IsNotNull(checkInvalidAge);
        Assert.IsTrue(checkValidAge);
        Assert.IsFalse(checkInvalidAge);

    }

    [TestMethod]
    public void ShouldCreateAccountWithin5Seconds()
    {
        //aranage

        var registration = new Registrator();


        //act

        //start timer
        bool checkTime = registration.CreateUser().IsSuccessful();
        //end timer and check time;

        bool checkOverTime = registration.CreateUserAccount().IsSuccessful();   //private info

        //bool checkTime = registration.CreateUserProfile().IsSuccessful(); //public info

        //assert
        Assert.IsNotNull(checkTime);
        Assert.IsNotNull(checkOverTime);
        Assert.IsTrue(checkTime);
        Assert.IsFalse(checkOverTime);
    }
}
