using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class CrimeMapIntergrationTest
    {
        [TestMethod]
        public async Task AddAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var alert = new CrimeAlert
            {
                Email = "Test Alert",
                Name = "NewTest@gmail.com",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            };

            // Act
            result = await crimeMapDAO.AddAlert(alert);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        [TestMethod]
        public async Task AddAlertFailure()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var alert = new CrimeAlert(); // empty alert object

            // Act
            result = await crimeMapDAO.AddAlert(alert);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [TestMethod]
        public async Task CheckAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var alert = new CrimeAlert
            {
                Email = "Test Alert",
                Name = "NewTests@gmail.com",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            };

            await crimeMapDAO.AddAlert(alert); // add alert first

            // Act
            result = await crimeMapDAO.CheckAlert(alert.Email);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        [TestMethod]
        public async Task CheckAlertFailure2Alerts()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var alert = new CrimeAlert
            {
                Email = "Test Alerts",
                Name = "NewTests@gmail.com",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            };
            await crimeMapDAO.AddAlert(alert); // add second alert to email

            // Act
            result = await crimeMapDAO.CheckAlert(alert.Email);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }
        [TestMethod]
        public async Task CheckAlertFailureNullEmail()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var email = "";

            // Act
            result = await crimeMapDAO.CheckAlert(email);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }
        /*[TestMethod]
        public async Task DeleteAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);

            // Act


            // Assert

        }
        [TestMethod]
        public async Task DeleteAlertFailure()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);

            // Act


            // Assert

        }
        [TestMethod]
        public async Task EditAlertSuccess()
        {
            // Arrange

            // Act


            // Assert

        }
        [TestMethod]
        public async Task EditAlertFailure()
        {
            // Arrange

            // Act


            // Assert

        }
        [TestMethod]
        public async Task GetAlertSuccess()
        {
            // Arrange

            // Act


            // Assert

        }
        [TestMethod]
        public async Task GetAlertFailure()
        {
            // Arrange

            // Act


            // Assert

        }*/
    }
}
