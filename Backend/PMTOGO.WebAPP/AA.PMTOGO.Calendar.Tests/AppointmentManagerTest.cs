using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AA.PMTOGO.Managers;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using Autofac.Extras.Moq;
using Moq;
using Xunit;

namespace AA.PMTOGO.Calendar.Tests;

public class AppointmentManagerTests : IDisposable
{
    private readonly Mock<IAppointmentService> _appointmentServiceMock;
    private readonly Mock<IUserManagement> _userServiceMock;
    private readonly AppointmentManager _appointmentManager;

    public AppointmentManagerTests()
    {
        _appointmentServiceMock = new Mock<IAppointmentService>();
        _userServiceMock = new Mock<IUserManagement>();
        _appointmentManager = new AppointmentManager(_appointmentServiceMock.Object, _userServiceMock.Object);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task DeleteAppointment_AppointmentNotFound_ReturnsErrorResult()
    {
        // Arrange
        int appointmentId = It.IsAny<int>();

        _appointmentServiceMock
            .Setup(x => x.GetAsync(appointmentId))
            .ReturnsAsync((Appointment?)null);

        // Act
        var result = await _appointmentManager.DeleteAppointment(appointmentId);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Appointment not found.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(x => x.DeleteAsync(appointmentId), Times.Never);
    }

    [Fact]
    public async Task DeleteAppointment_ErrorDeletingAppointment_ReturnsErrorResult()
    {
        // Arrange
        int appointmentId = It.IsAny<int>();
        var appointment = new Appointment { AppointmentId = appointmentId };

        _appointmentServiceMock
            .Setup(x => x.GetAsync(appointmentId))
            .ReturnsAsync(appointment);
        _appointmentServiceMock
            .Setup(x => x.DeleteAsync(appointmentId))
            .ReturnsAsync(false);

        // Act
        var result = await _appointmentManager.DeleteAppointment(appointmentId);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("An error occured.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(x => x.DeleteAsync(appointmentId), Times.Once);
    }

    [Fact]
    public async Task DeleteAppointment_SuccessfullyDeleted_ReturnsSuccessfulResult()
    {
        // Arrange
        var appointmentId = It.IsAny<int>();
        var appointment = new Appointment { AppointmentId = appointmentId };
        _appointmentServiceMock
            .Setup(x => x.GetAsync(appointmentId))
            .ReturnsAsync(appointment);
        _appointmentServiceMock
            .Setup(x => x.DeleteAsync(appointmentId))
            .ReturnsAsync(true);

        // Act
        var result = await _appointmentManager.DeleteAppointment(appointmentId);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.True((bool)result.Payload);

        _appointmentServiceMock.Verify(x => x.DeleteAsync(appointmentId), Times.Once);
    }

    [Fact]
    public async Task GetUserAppointments_UnableToFetchUser_ReturnsErrorResult()
    {
        // Arrange
        var username = It.IsAny<string>();

        _userServiceMock
            .Setup(x => x.GetUser(username))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _appointmentManager.GetUserAppointments(username);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Unable to fetch user.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(x => x.GetAllByUserIdAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetUserAppointments_Successful_ReturnsSuccessfulResult()
    {
        // Arrange
        var username = It.IsAny<string>();
        var user = new User { Username = username };
        var appointments = new List<Appointment> { new Appointment(), new Appointment() };

        _userServiceMock
            .Setup(x => x.GetUser(username))
            .ReturnsAsync(user);
        _appointmentServiceMock
            .Setup(x => x.GetAllByUserIdAsync(user.Username))
            .ReturnsAsync(appointments);

        // Act
        var result = await _appointmentManager.GetUserAppointments(username);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(appointments, result.Payload);

        _appointmentServiceMock.Verify(x => x.GetAllByUserIdAsync(user.Username), Times.Once);
    }

    [Fact]
    public async Task InsertAppointment_InvalidAppointmentTime_ReturnsUnsuccessfulResult()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentTime = DateTime.UtcNow.AddHours(-1)
        };

        // Act
        var result = await _appointmentManager.InsertAppointment(appointment, It.IsAny<string>());

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Invalid appointment time.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(service => service.InsertAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task InsertAppointment_UserNotFound_ReturnsUnsuccessfulResult()
    {
        // Arrange
        _userServiceMock
            .Setup(service => service.GetUser(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        var appointment = new Appointment
        {
            AppointmentTime = DateTime.UtcNow.AddHours(1)
        };

        // Act
        var result = await _appointmentManager.InsertAppointment(appointment, It.IsAny<string>());

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Unable to fetch user.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(service => service.InsertAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task InsertAppointment_OverlappingAppointmentTime_ReturnsUnsuccessfulResult()
    {
        // Arrange
        var appointment1 = new Appointment
        {
            AppointmentTime = DateTime.UtcNow.AddHours(1),
            Username = It.IsAny<string>()
        };
        var appointment2 = new Appointment
        {
            AppointmentTime = DateTime.UtcNow.AddHours(1).AddMinutes(30),
            Username = It.IsAny<string>()
        };
        var appointments = new List<Appointment> { appointment1, appointment2 };

        _userServiceMock
            .Setup(service => service.GetUser(It.IsAny<string>()))
            .ReturnsAsync(new User { Username = It.IsAny<string>() });
        _appointmentServiceMock
            .Setup(service => service.GetAllByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(appointments);

        var appointment = new Appointment
        {
            AppointmentTime = DateTime.UtcNow.AddHours(1),
            Username = It.IsAny<string>()
        };

        // Act
        var result = await _appointmentManager.InsertAppointment(appointment, It.IsAny<string>());

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Invalid appointment time.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(service => service.InsertAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task InsertAppointment_ReturnsSuccessfulResult()
    {
        // Arrange
        _userServiceMock
            .Setup(service => service.GetUser(It.IsAny<string>()))
            .ReturnsAsync(new User { Username = It.IsAny<string>() });
        _appointmentServiceMock
            .Setup(service => service.GetAllByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Appointment>());
        _appointmentServiceMock
            .Setup(service => service.InsertAsync(It.IsAny<Appointment>()))
            .ReturnsAsync(true);

        var appointment = new Appointment
        {
            AppointmentTime = DateTime.UtcNow.AddHours(1),
            Username = It.IsAny<string>()
        };

        // Act
        var result = await _appointmentManager.InsertAppointment(appointment, It.IsAny<string>());

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public async Task UpdateAppointment_ReturnsSuccessfulResult()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Username = "user1",
            Title = "Appointment 1",
            AppointmentTime = DateTime.UtcNow.AddHours(1),
        };
        var appointments = new List<Appointment> { appointment };

        _appointmentServiceMock
            .Setup(x => x.GetAllByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(appointments);
        _appointmentServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Appointment>()))
            .ReturnsAsync(true);

        // Act
        appointment.AppointmentTime = appointment.AppointmentTime.AddHours(2);
        var result = await _appointmentManager.UpdateAppointment(appointment);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Null(result.ErrorMessage);
        Assert.True((bool)result.Payload);

        _appointmentServiceMock.Verify(x => x.GetAllByUserIdAsync(It.IsAny<string>()), Times.Once);
        _appointmentServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAppointment_InvalidAppointmentTime_ReturnsUnsuccessfulResult()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Username = "user1",
            Title = "Appointment 1",
            AppointmentTime = DateTime.UtcNow.AddHours(-1),
        };

        // Act
        var result = await _appointmentManager.UpdateAppointment(appointment);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Invalid appointment time.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(x => x.GetAllByUserIdAsync(It.IsAny<string>()), Times.Never);
        _appointmentServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAppointment_OverlappingAppointmentTime_ReturnsUnsuccessfulResult()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Username = "user1",
            Title = "Appointment 1",
            AppointmentTime = DateTime.UtcNow.AddHours(1),
        };
        var appointments = new List<Appointment>
        {
            appointment,
            new Appointment
            {
                AppointmentId = 2,
                Username = "user2",
                Title = "Appointment 2",
                AppointmentTime = appointment.AppointmentTime.AddMinutes(-30),
            }
        };

        _appointmentServiceMock.Setup(x => x.GetAllByUserIdAsync(It.IsAny<string>())).ReturnsAsync(appointments);

        // Act
        var result = await _appointmentManager.UpdateAppointment(appointment);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Invalid appointment time.", result.ErrorMessage);
        Assert.Null(result.Payload);

        _appointmentServiceMock.Verify(x => x.GetAllByUserIdAsync(It.IsAny<string>()), Times.Once);
        _appointmentServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
    }
}