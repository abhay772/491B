using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Managers;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using System.Net.Mail;

namespace AA.PMTOGO.PropertyEvalTests;

[TestClass]
public class PropEvalUTests
{

    [TestMethod]
    public void True_SaveProfileWithValidUsername()
    {
        // Arrage
        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);

        // fully populated
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
        string username = "rajaFarley@gmail.com";

        // Act

        Result result = propEvalManager.saveProfileAsync(username, propertyProfile).Result;

        // Assert
        Assert.IsTrue(result.IsSuccessful);
    }

    [TestMethod]
    public void False_SaveProfileWithInvalidUsername()
    {
        // Arrage

        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);

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

        Result result = propEvalManager.saveProfileAsync(username, propertyProfile).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);
    }

    [TestMethod]
    public void False_SaveProfileWithPartialProfile()
    {
        // Arrage

        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);

        // partially populated
        var propertyProfile = new PropertyProfile
        {
            NoOfBedrooms = 1,
            NoOfBathrooms = 1,
            SqFeet = 1,
        };

        // valid username
        string username = "rajaFarley@gmail.com";

        // Act

        Result result = propEvalManager.saveProfileAsync(username, propertyProfile).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);
    }


    [TestMethod]
    public void False_SaveProfileWithEmptyProfile()
    {
        // Arrage

        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);

        // empty profile or no change detected
        var propertyProfile = new PropertyProfile();

        // valid username
        string username = "rajaFarley@gmail.com";

        // Act

        Result result = propEvalManager.saveProfileAsync(username, propertyProfile).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);
    }

    [TestMethod]
    public void True_LoadProfileWithValidUsername()
    {

        // Arrage
        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);
        var expected = typeof(PropertyProfile);

        // valid username
        string username = "rajaFarley@gmail.com";
        // Act

        Result result = propEvalManager.loadProfileAsync(username).Result;

        // Assert
        Assert.IsNotNull(result.Payload);
        Assert.AreEqual(expected, result.Payload.GetType());
    }

    [TestMethod]
    public void False_LoadProfileWithInvalidUsername()
    {
        // Arrage
        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);
        var expected = typeof(PropertyProfile);

        // invalid username
        string username = "abhaygmail.com";
        // Act

        Result result = propEvalManager.loadProfileAsync(username).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);
    }

    // saveProfile(username, propertyProfile), where username is valid and non-valid, and propertyProfile full, partial and empty.



    [TestMethod]
    public void False_EvaluatePropertyNotInDB()
    {
        // Arrage

        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);

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
        string username = "rajaFarley@gmail.com";

        // Act

        Result result = propEvalManager.evaluateAsync(username, propertyProfile).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);
    }

    [TestMethod]
    public void True_EvaluateProperty()
    {
        // Arrage

        var sqlPropEvalDao = new SqlPropEvalDAO();
        var historicalDAO = new HistoricalSalesDAO();
        var evaluator = new Services.PropertyEvaluator(historicalDAO);

        var propEvalManager = new PropEvalManager(sqlPropEvalDao, evaluator);


        var propertyProfile = new PropertyProfile
        {
            NoOfBedrooms = 5,
            NoOfBathrooms = 2,
            SqFeet = 5785,
            Address1 = "9662 Golden Leaf Junction",
            Address2 = "Suite 12",
            City = "Sacramento",
            State = "CA",
            Zip = "95852",
            Description = "This some test desciption."
        };

        // valid username
        string username = "rajaFarley@gmail.com";

        // Act

        Result result = propEvalManager.evaluateAsync(username, propertyProfile).Result;

        // Assert
        Assert.IsTrue(result.IsSuccessful);
    }
}
