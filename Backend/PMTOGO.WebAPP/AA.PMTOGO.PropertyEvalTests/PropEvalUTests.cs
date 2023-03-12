using AA.PMTOGO.Models.Entities;
using System.Net.Mail;

namespace AA.PMTOGO.PropertyEvalTests
{
    [TestClass]
    public class PropEvalUTests
    {
        [TestMethod]
        public void True_LoadProfileWithValidUsername()
        {
            // Arrage
            var _evaluator = new IPropEvalManager();
            var expected = typeof(PropertyProfile);

            // valid username
            string username = "abhay@gmail.com";
            // Act

            Result result = _evaluator.loadProfile(username);

            // Assert
            Assert.IsNotNull(result.Payload);
            Assert.Equals(expected, result.Payload.GetType());
        }

        [TestMethod]
        public void False_LoadProfileWithInvalidUsername()
        {
            // Arrage
            var _evaluator = new IPropEvalManager();
            var expected = typeof(PropertyProfile);

            // invalid username
            string username = "abhaygmail.com";
            // Act

            Result result = _evaluator.loadProfile(username);

            // Assert
            Assert.IsNull(result.Payload);
        }

        // saveProfile(username, propertyProfile), where username is valid and non-valid, and propertyProfile full, partial and empty.

        [TestMethod]
        public void True_SaveProfileWithValidUsername()
        {
            // Arrage

            var _evaluator = new IPropEvalManager();

            // fully populated
            var propertyProfile = new PropertyProfile {
                NoOfBedrooms = 1,
                NoOfBathrooms = 1,
                SqFeet = 1,
                Address1 = "6867 Something Street",
                Address2 = "#850",
                City = "Long Beach",
                State = "CA",
                Zip = "90815",
                Description = "This some test desciption."
            };

            // valid username
            string username = "abhay@gmail.com";

            // Act

            Result result = _evaluator.saveProfile(username, propertyProfile);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void False_SaveProfileWithInvalidUsername()
        {
            // Arrage

            var _evaluator = new IPropEvalManager();

            var propertyProfile = new PropertyProfile
            {
                NoOfBedrooms = 1,
                NoOfBathrooms = 1,
                SqFeet = 1,
                Address1 = "6867 Something Street",
                Address2 = "#850",
                City = "Long Beach",
                State = "CA",
                Zip = "90815",
                Description = "This some test desciption."
            };

            // invalid username
            string username = "abhaygmail.com";

            // Act

            Result result = _evaluator.saveProfile(username, propertyProfile);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }

        [TestMethod]
        public void True_SaveProfileWithPartialProfile()
        {
            // Arrage

            var _evaluator = new IPropEvalManager();

            // partially populated
            var propertyProfile = new PropertyProfile
            {
                NoOfBedrooms = 1,
                NoOfBathrooms = 1,
                SqFeet = 1,
            };

            // valid username
            string username = "abhay@gmail.com";

            // Act

            Result result = _evaluator.saveProfile(username, propertyProfile);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public void False_SaveProfileWithPartialProfile()
        {
            // Arrage

            var _evaluator = new IPropEvalManager();

            // empty profile or no change detected
            var propertyProfile = new PropertyProfile();

            // valid username
            string username = "abhay@gmail.com";

            // Act

            Result result = _evaluator.saveProfile(username, propertyProfile);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }
    }


    [TestMethod]
    public void False_EvaluatePropertyNotInDB()
    {
        // Arrage

        var _evaluator = new IPropEvalManager();

        var propertyProfile = new PropertyProfile
        {
            NoOfBedrooms = 1,
            NoOfBathrooms = 1,
            SqFeet = 1,
            Address1 = "6867 Something Street",
            Address2 = "#850",
            City = "Long Beach",
            State = "CA",
            Zip = "90815",
            Description = "This some test desciption."
        };

        // valid username
        string username = "abhay@gmail.com";

        // Act

        Result result = _evaluator.saveProfile(username, propertyProfile);

        // Assert
        Assert.IsFalse(result.IsSuccessful);
    }

    //[TestMethod]
    //public void True_EmailWithNotification()
    //{
    //}
}

// Arrage
// Act
// Assert