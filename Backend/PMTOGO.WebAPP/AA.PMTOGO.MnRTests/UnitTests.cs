using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using Moq;

namespace AA.PMTOGO.MnRTests;

[TestClass]
public class UnitTests
{
    // Add Project Tests

    [TestMethod]
    public void AddProjectWithValidUsername_True()
    {
        //Arrange

        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        int evalChange = 0;
        int OEval = 0;

        var projectDetail = new ProjectDetail
        {
            ProjectName = "Tranquil Haven: Mindful Living",
            ServiceIDs = new List<int> { 0 },
            StartDate = new DateOnly(2023, 06, 11),
            EndDate = new DateOnly(2023, 08, 11),
            ServiceTime = new TimeOnly(16, 30),
            Budget = 10_000,
        };

        // Invalid username
        string username = "rajaFarley@gmail.com";

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.SaveProject(
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<ProjectDetail>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.SaveProject(username, evalChange, OEval, projectDetail).Result;

        // Assert
        Assert.IsTrue(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, evalChange, OEval, projectDetail), Times.Once());
    }

    [TestMethod]
    public void AddProjectWithInvalidUsername_False()
    {
        //Arrange

        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        int evalChange = 0;
        int OEval = 0;

        var projectDetail = new ProjectDetail
        {
            ProjectName = "Tranquil Haven: Mindful Living",
            ServiceID = 0,
            StartDate = new DateOnly(2023, 06, 11),
            EndDate = new DateOnly(2023, 08, 11),
            ServiceTime = new TimeOnly(16, 30),
            Budget = 10_000,
        };

        // Invalid username
        string username = "rajaFarleygmail.com";

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.SaveProject(
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<ProjectDetail>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.SaveProject(username, evalChange, OEval, projectDetail).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, evalChange, OEval, projectDetail), Times.Once());
    }

    [TestMethod]
    public void AddProjectWithInvalidName_False()
    {
        //Arrange

        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        int evalChange = 0;
        int OEval = 0;

        var projectDetail = new ProjectDetail
        {
            ProjectName = "Whispering Willow Retreat: A Serene Haven for Mindful Living",
            ServiceID = 0,
            StartDate = new DateOnly(2023, 06, 11),
            EndDate = new DateOnly(2023, 08, 11),
            ServiceTime = new TimeOnly(16, 30),
            Budget = 10_000,
        };

        // Invalid username
        string username = "rajaFarley@gmail.com";

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.SaveProject(
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<ProjectDetail>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.SaveProject( username, evalChange,OEval,projectDetail).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, evalChange, OEval, projectDetail), Times.Once());
    }

    [TestMethod]
    public void AddProjectWithInvalidServiceID_False()
    {
        //Arrange

        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        int evalChange = 0;
        int OEval = 0;

        var projectDetail = new ProjectDetail
        {
            ProjectName = "Tranquil Haven: Mindful Living",
            ServiceID = -1,
            StartDate = new DateOnly(2023, 06, 11),
            EndDate = new DateOnly(2023, 08, 11),
            ServiceTime = new TimeOnly(16, 30),
            Budget = 10_000,
        };

        // Invalid username
        string username = "rajaFarley@gmail.com";

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.SaveProject(
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<ProjectDetail>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.SaveProject( username,evalChange,OEval,projectDetail).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, evalChange, OEval, projectDetail), Times.Once());
    }

    [TestMethod]
    public void AddProjectWithInvalidDates_False()
    {
        //Arrange

        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        int evalChange = 0;
        int OEval = 0;

        var projectDetail = new ProjectDetail
        {
            ProjectName = "Tranquil Haven: Mindful Living",
            ServiceID = 0,
            StartDate = new DateOnly(2023, 06, 11),
            EndDate = new DateOnly(2023, 02, 11),
            ServiceTime = new TimeOnly(16, 30),
            Budget = 10_000,
        };

        // Invalid username
        string username = "rajaFarley@gmail.com";

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.SaveProject(
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<ProjectDetail>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.SaveProject(username,evalChange, OEval,projectDetail).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, evalChange, OEval, projectDetail), Times.Once());
    }

    [TestMethod]
    public void AddProjectWithInvalidEvalChange_False()
    {
        //Arrange
        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        int evalChange = "0";
        int OEval = 0;

        var projectDetail = new ProjectDetail
        {
            ProjectName = "Tranquil Haven: Mindful Living",
            ServiceID = 0,
            StartDate = new DateOnly(2023, 06, 11),
            EndDate = new DateOnly(2023, 08, 11),
            ServiceTime = new TimeOnly(16, 30),
            Budget = 10_000,
        };

        // Invalid username
        string username = "rajaFarley@gmail.com";

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.SaveProject(
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<ProjectDetail>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.SaveProject(username,evalChange,OEval,projectDetail).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, evalChange, OEval, projectDetail), Times.Once());

    }

    [TestMethod]
    public void AddProjectWithInvalidOriginalEval_False()
    {
        //Arrange
        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        int evalChange = 0;
        int OEval = -1;

        var projectDetail = new ProjectDetail
        {
            ProjectName = "Tranquil Haven: Mindful Living",
            ServiceID = 0,
            StartDate = new DateOnly(2023, 06, 11),
            EndDate = new DateOnly(2023, 08, 11),
            ServiceTime = new TimeOnly(16, 30),
            Budget = 10_000,
        };

        // Invalid username
        string username = "rajaFarley@gmail.com";

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.SaveProject(
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<ProjectDetail>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.SaveProject(username, evalChange, OEval, projectDetail).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, evalChange, OEval, projectDetail), Times.Once());

    }

    // Delete Project

    [TestMethod]
    public void DeleteProjectWithInvalidProjectID_False()
    {
        //Arrange
        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        // Valid username
        string username = "rajaFarley@gmail.com";

        int projectID = -1;

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.DeleteProject(It.IsAny<string>(),It.IsAny<int>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.DeleteProject(username,projectID).Result;

        // Assert
        Assert.IsFalse(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, projectID), Times.Once());

    }

    [TestMethod]
    public void DeleteProjectWithValidProjectID_True()
    {
        //Arrange
        var mockServiceProjectManager = new Mock<IServiceProjectManager>();
        var mockProjectOrganizer = new Mock<IProjectOrganizer>();
        var mockServiceProjectDAO = new Mock<IServiceProjectDAO>();

        // Valid username
        string username = "rajaFarley@gmail.com";

        int projectID = 1;

        // Set up mock objects to return expected results
        mockServiceProjectDAO.Setup(
            mock => mock.DeleteProject(
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns(Task.FromResult(new Result { IsSuccessful = true }));

        // Act
        Result result = mockServiceProjectManager.DeleteProject(
            username,
            projectID)
            .Result;

        // Assert
        Assert.IsTrue(result.IsSuccessful);

        mockServiceProjectDAO.Verify(mock =>
        mock.SaveProfile(username, projectID), Times.Once());
    }
}