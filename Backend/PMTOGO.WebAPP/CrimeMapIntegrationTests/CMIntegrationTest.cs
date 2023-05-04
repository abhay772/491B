using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;


namespace CrimeMapIntegrationTests
{
    [TestClass]
    public class CMIntegrationTest
    {
        [TestMethod]
        public async Task AddAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var alert = new CrimeAlert
            {
                Email = "NewTest@gmail.com",
                Name = "Test Alert",
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
        public async Task AddAlertFailureHas2Alerts()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var alert = new CrimeAlert
            {
                Email = "NewTest3@gmail.com",
                Name = "Test Alert 1",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            };
            var alert2 = new CrimeAlert
            {
                Email = "NewTest3@gmail.com",
                Name = "Test Alert 2",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            }; var alert3 = new CrimeAlert
            {
                Email = "NewTest3@gmail.com",
                Name = "Test Alert 3",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            };

            // Act
            await crimeMapDAO.AddAlert(alert);
            await crimeMapDAO.AddAlert(alert2);
            result = await crimeMapDAO.AddAlert(alert3);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }

        [TestMethod]
        public async Task CheckAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var alert = new CrimeAlert
            {
                Email = "NewTestCheck@gmail.com",
                Name = "Test Alert",
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
                Email = "NewTest@gmail.com",
                Name = "Test Alert 2",
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
        public async Task CheckAlertFailureBlankEmail()
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
        [TestMethod]
        public async Task DeleteAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var newAlert = new CrimeAlert
            {
                Email = "NewTest5@gmail.com",
                Name = "Test Alert",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            };

            // Act
            result = await crimeMapDAO.AddAlert(newAlert);
            var alerts = crimeMapDAO.GetAlerts();
            foreach (var alert in alerts.Result)
            {
                result = await crimeMapDAO.DeleteAlert(alert.Email, alert.ID);
            }
            alerts = crimeMapDAO.GetAlerts();

            // Assert
            Assert.IsTrue(alerts.Result.Count == 0);
        }
        
        [TestMethod]
        public async Task EditAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);
            var newAlert = new CrimeAlert
            {
                Email = "NewTest5@gmail.com",
                Name = "Test Alert",
                Location = "location",
                Description = "This is a test alert",
                Time = "12:00",
                Date = "5/1/2023",
                X = 99.9,
                Y = 99.9
            };

            // Act
            result = await crimeMapDAO.AddAlert(newAlert);
            var alerts = crimeMapDAO.GetAlerts();
            var alert = alerts.Result[0];
            alert = new CrimeAlert
            {
                Email = "NewTest5@gmail.com",
                Name = "test",
                Location = "test",
                Description = "test",
                Time = "9:00",
                Date = "1/1/2023",
                X = 99.9,
                Y = 99.9
            };
            await crimeMapDAO.EditAlert(alert.Email, alert.ID, alert);

            alerts = crimeMapDAO.GetAlerts();
            foreach (var a in alerts.Result)
            {
                if (a.ID == alert.ID)
                {
                    alert = a;
                    break;
                }
            }


            // Assert
            Assert.IsTrue(alert.Name == "test");
            Assert.IsTrue(alert.Location == "test");
            Assert.IsTrue(alert.Description == "test");
            Assert.IsTrue(alert.Time == "9:00");
            Assert.IsTrue(alert.Date == "1/1/2023");
        }
        [TestMethod]
        public async Task GetAlertSuccess()
        {
            // Arrange
            var result = new Result();
            var crimeMapDAO = new CrimeMapDAO(result);

            // Act
            var alerts = await crimeMapDAO.GetAlerts();
            
            // Assert
            Assert.IsNotNull(alerts);
        }
    }
}