using FluentAssertions;
using HospitalAPI.Controllers;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HospitalTestUnit.Systems.Controllers
{
    public class TestAppointmentController
    {
        private readonly List<Appointment> appointments;
        private Mock<IAppointmenService> appointmentServiceMock;
        private AppointmentsController appointmentController;

        public TestAppointmentController()
        {
            appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 15, 10, 0, 0)
                },
                new Appointment
                {
                    Id = 2,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 18, 14, 30, 0)
                },
                new Appointment
                {
                    Id = 3,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 20, 9, 15, 0)
                }
            };

            appointmentServiceMock = new Mock<IAppointmenService>();

            appointmentController = new AppointmentsController(appointmentServiceMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfAppointments()
        {
            // Arrange
            appointmentServiceMock.Setup(service => service.GetAll()).Returns(appointments);

            // Act
            var result = appointmentController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<Appointment>>();
            result.Value.Should().BeEquivalentTo(appointments);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoAppointmentsExist()
        {
            // Arrange
            appointmentServiceMock.Setup(service => service.GetAll()).Returns(new List<Appointment>());

            // Act
            var result = appointmentController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<Appointment>>();
            result.Value.As<List<Appointment>>().Should().HaveCount(0);
        }

        [Fact]
        public void GetById_ReturnsAppointmentWithValidId()
        {
            // Arrange
            var appointmentIdToFind = 1;
            var appointmentToReturn = new Appointment { Id = appointmentIdToFind, Date = new DateTime(2023, 10, 15, 10, 0, 0) };
            appointmentServiceMock.Setup(service => service.GetById(appointmentIdToFind)).Returns(appointmentToReturn);

            // Act
            var result = appointmentController.GetById(appointmentIdToFind) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Appointment>();
            result.Value.Should().BeEquivalentTo(appointmentToReturn);
        }

        [Fact]
        public void GetById_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            var invalidAppointmentId = 999;
            appointmentServiceMock.Setup(service => service.GetById(invalidAppointmentId)).Returns((Appointment)null);

            // Act
            var result = appointmentController.GetById(invalidAppointmentId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var newAppointment = new Appointment
            {
                DoctorJmbg = 987654321,
                PatientJmbg = 1111111111,
                Date = new DateTime(2023, 11, 10, 14, 30, 0)
            };

            // Act
            var result = appointmentController.Create(newAppointment) as CreatedAtActionResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
            result.ActionName.Should().Be("GetById");
            result.RouteValues["id"].Should().Be(newAppointment.Id);
            result.Value.Should().Be(newAppointment);
        }

        [Fact]
        public void Update_ReturnsOkResultForValidAppointment()
        {
            // Arrange
            var appointmentIdToUpdate = 1;
            var updatedAppointment = new Appointment
            {
                Id = appointmentIdToUpdate,
                DoctorJmbg = 1234567890,
                PatientJmbg = 373737373,
                Date = new DateTime(2023, 12, 5, 11, 0, 0)
            };

            appointmentServiceMock.Setup(service => service.Update(updatedAppointment));

            // Act
            var result = appointmentController.Update(appointmentIdToUpdate, updatedAppointment) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(updatedAppointment);
        }

        [Fact]
        public void Update_ReturnsBadRequestForInvalidId()
        {
            // Arrange
            var invalidAppointmentId = 999;
            var updatedAppointment = new Appointment
            {
                Id = invalidAppointmentId,
                DoctorJmbg = 1234567890,
                PatientJmbg = 373737373,
                Date = new DateTime(2023, 12, 5, 11, 0, 0)
            };

            appointmentServiceMock.Setup(service => service.Update(updatedAppointment)).Throws(new Exception());

            // Act
            var result = appointmentController.Update(invalidAppointmentId, updatedAppointment) as BadRequestResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public void Delete_ReturnsNoContentForValidAppointment()
        {
            // Arrange
            var appointmentIdToDelete = 1;
            var appointmentToDelete = new Appointment
            {
                Id = appointmentIdToDelete,
                DoctorJmbg = 987654321,
                PatientJmbg = 1234567890,
                Date = new DateTime(2023, 10, 15, 10, 0, 0)
            };

            appointmentServiceMock.Setup(service => service.GetById(appointmentIdToDelete)).Returns(appointmentToDelete);

            // Act
            var result = appointmentController.Delete(appointmentIdToDelete) as NoContentResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
            appointmentServiceMock.Verify(service => service.Delete(appointmentToDelete), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFoundForInvalidAppointment()
        {
            // Arrange
            var invalidAppointmentId = 999;
            appointmentServiceMock.Setup(service => service.GetById(invalidAppointmentId)).Returns((Appointment)null);

            // Act
            var result = appointmentController.Delete(invalidAppointmentId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void ReserveAppointment_ReturnsOkResultForValidReservation()
        {
            // Arrange
            var appointmentIdToReserve = 1;
            var userJmbg = 1111111111;

            var appointmentToReserve = new Appointment
            {
                Id = appointmentIdToReserve,
                DoctorJmbg = 987654321,
                PatientJmbg = 0,
                Date = new DateTime(2023, 10, 15, 10, 0, 0)
            };

            appointmentServiceMock.Setup(service => service.GetById(appointmentIdToReserve)).Returns(appointmentToReserve);

            // Act
            var result = appointmentController.ReserveAppointment(appointmentIdToReserve, new AppointmentsController.ReserveAppointmentRequest { PatientJmbg = userJmbg }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("Appointment reserved successfully.");
        }


        [Fact]
        public void DeclineAppointment_ReturnsOkResultForValidDecline()
        {
            // Arrange
            var appointmentIdToDecline = 1;

            var appointmentToDecline = new Appointment
            {
                Id = appointmentIdToDecline,
                DoctorJmbg = 987654321,
                PatientJmbg = 1111111111, // Reserved by the user
                Date = new DateTime(2023, 10, 15, 10, 0, 0)
            };

            appointmentServiceMock.Setup(service => service.GetById(appointmentIdToDecline)).Returns(appointmentToDecline);

            // Act
            var result = appointmentController.DeclineAppointment(appointmentIdToDecline) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("Appointment declined successfully.");
        }

        [Fact]
        public void FindAppointment_ReturnsAppointmentWithId1()
        {
            // Arrange
            var doctorJmbg = 987654321;
            var date = new DateTime(2023, 10, 15, 10, 0, 0);
            var priority = 1;

            // Create the appointment you expect to be returned
            var expectedAppointment = new Appointment
            {
                Id = 1,
                DoctorJmbg = doctorJmbg,
                PatientJmbg = 1234567890,
                Date = date
            };

            // Set up the mock for FindMeAppointment to return the expected appointment
            appointmentServiceMock.Setup(service =>
                service.FindMeAppointment(doctorJmbg, date, priority))
                .Returns(expectedAppointment);

            // Act
            var result = appointmentController.FindAppointment(new AppointmentsController.FindAppointmentRequest
            {
                DoctorJmbg = doctorJmbg,
                Date = date,
                Priority = priority
            }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Appointment>();
            result.Value.Should().BeEquivalentTo(expectedAppointment);
        }


    }
}
