using FluentAssertions;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using HospitalLibrary.Core.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HospitalTestUnit.Systems.Services
{
    public class TestAppointmentService
    {
        private readonly List<Appointment> appointments;
        private Mock<IAppointmentRepository> appointmentRepositoryMock;
        private AppointmentService appointmentService;

        public TestAppointmentService()
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

            appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            appointmentService = new AppointmentService(appointmentRepositoryMock.Object);
        }

        [Fact]
        public void GetAll_ReturnAllAppointments()
        {
            // Arrange
            appointmentRepositoryMock.Setup(repo => repo.GetAll()).Returns(appointments);

            // Act
            var result = appointmentService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointments);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoAppointmentsExist()
        {
            // Arrange
            appointmentRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Appointment>());

            // Act
            var result = appointmentService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetById_ReturnsAppointmentWithValidId()
        {
            // Arrange
            int appointmentToFind = 1;
            appointmentRepositoryMock.Setup(repo => repo.GetById(appointmentToFind))
                .Returns((int id) => appointments.FirstOrDefault(a => a.Id == id));

            // Act
            var result = appointmentService.GetById(appointmentToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointments.FirstOrDefault(a => a.Id == appointmentToFind));
        }

        [Fact]
        public void GetById_ReturnsNullForInvalidId()
        {
            // Arrange
            int appointmentToFind = 999;
            appointmentRepositoryMock.Setup(repo => repo.GetById(appointmentToFind)).Returns((Appointment)null);

            // Act
            var result = appointmentService.GetById(appointmentToFind);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Create_AddAppointmentToRepository()
        {
            // Arrange
            var newAppointment = new Appointment
            {
                DoctorJmbg = 987654321,
                PatientJmbg = 1111111111,
                Date = new DateTime(2023, 11, 10, 14, 30, 0)
            };

            // Act
            appointmentService.Create(newAppointment);

            // Assert
            appointmentRepositoryMock.Verify(repo => repo.Create(newAppointment), Times.Once);
        }

        [Fact]
        public void Update_ModifiesExistingAppointment()
        {
            // Arrange
            var appointmentToUpdate = new Appointment
            {
                Id = 1,
                DoctorJmbg = 1234567890,
                PatientJmbg = 373737373,
                Date = new DateTime(2023, 12, 5, 11, 0, 0)
            };

            // Act
            appointmentService.Update(appointmentToUpdate);

            // Assert
            appointmentRepositoryMock.Verify(repo => repo.Update(appointmentToUpdate), Times.Once);
        }

        [Fact]
        public void Delete_ExistingAppointment_SuccessfullyDeletesAppointment()
        {
            // Arrange
            var appointmentToDelete = new Appointment
            {
                Id = 2,
                DoctorJmbg = 987654321,
                PatientJmbg = 1234567890,
                Date = new DateTime(2023, 10, 18, 14, 30, 0)
            };

            // Act
            appointmentService.Delete(appointmentToDelete);

            // Assert
            appointmentRepositoryMock.Verify(repo => repo.Delete(appointmentToDelete), Times.Once);
        }

        [Fact]
        public void FindMeAppointment_ReturnsAppointmentWithIdealPriority()
        {
            // Arrange
            int doctorJmbgToFind = 987654321;
            DateTime dateToFind = new DateTime(2023, 10, 15, 10, 0, 0);
            int priority = 1;

            var idealAppointment = new Appointment
            {
                Id = 1,
                DoctorJmbg = doctorJmbgToFind,
                PatientJmbg = 1234567890,
                Date = dateToFind
            };

            appointmentRepositoryMock.Setup(repo =>
                repo.FindMeAppointment(doctorJmbgToFind, dateToFind, priority))
                .Returns(idealAppointment);

            // Act
            var result = appointmentService.FindMeAppointment(doctorJmbgToFind, dateToFind, priority);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(idealAppointment);
        }

        [Fact]
        public void FindMeAppointment_ReturnsAppointmentWithDoctorPriority()
        {
            // Arrange
            int doctorJmbgToFind = 987654321;
            DateTime dateToFind = new DateTime(2023, 10, 18, 14, 30, 0);
            int priority = 1;

            var doctorPriorityAppointment = new Appointment
            {
                Id = 2,
                DoctorJmbg = doctorJmbgToFind,
                PatientJmbg = 1234567890,
                Date = new DateTime(2023, 10, 21, 14, 30, 0)
            };

            appointmentRepositoryMock.Setup(repo =>
                repo.FindMeAppointment(doctorJmbgToFind, dateToFind, priority))
                .Returns(doctorPriorityAppointment);

            // Act
            var result = appointmentService.FindMeAppointment(doctorJmbgToFind, dateToFind, priority);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(doctorPriorityAppointment);
        }

        [Fact]
        public void ReserveAppointment_ReservationSuccess()
        {
            // Arrange
            var appointmentToReserve = new Appointment
            {
                Id = 1,
                DoctorJmbg = 987654321,
                PatientJmbg = 0,
                Date = new DateTime(2023, 11, 10, 14, 30, 0)
            };

            var userJmbg = 1111111111;

            appointmentRepositoryMock.Setup(repo => repo.ReserveAppointment(appointmentToReserve, userJmbg))
                .Callback((Appointment appointment, int patientJmbg) =>
                {
                    appointment.PatientJmbg = patientJmbg;
                });

            // Act
            appointmentService.ReserveAppointment(appointmentToReserve, userJmbg);

            // Assert
            appointmentToReserve.PatientJmbg.Should().Be(userJmbg);
            appointmentRepositoryMock.Verify(repo => repo.ReserveAppointment(appointmentToReserve, userJmbg), Times.Once);
        }

        [Fact]
        public void DeclineAppointment_DecliningSuccess()
        {
            // Arrange
            var appointmentToDecline = new Appointment
            {
                Id = 1,
                DoctorJmbg = 987654321,
                PatientJmbg = 1234567890,
                Date = new DateTime(2023, 10, 15, 10, 0, 0)
            };

            appointmentRepositoryMock.Setup(repo => repo.DeclineAppointment(appointmentToDecline))
                .Callback((Appointment appointment) =>
                {
                    appointment.PatientJmbg = 0;
                });

            // Act
            appointmentService.DeclineAppointment(appointmentToDecline);

            // Assert
            appointmentToDecline.PatientJmbg.Should().Be(0);
            appointmentRepositoryMock.Verify(repo => repo.DeclineAppointment(appointmentToDecline), Times.Once);
        }

        [Fact]
        public void GetAllOfPatient_ReturnsAppointmentsForValidPatient()
        {
            // Arrange
            var targetPatientJmbg = 1234567890;
            var appointmentsForPatient = appointments.Where(a => a.PatientJmbg == targetPatientJmbg).ToList();
            appointmentRepositoryMock.Setup(repo => repo.GetAllOfPatient(targetPatientJmbg)).Returns(appointmentsForPatient);

            // Act
            var result = appointmentService.GetAllOfPatient(targetPatientJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(appointmentsForPatient);
        }

        [Fact]
        public void GetAllOfPatient_ReturnsEmptyListForInvalidPatient()
        {
            // Arrange
            var invalidPatientJmbg = 999999999;
            appointmentRepositoryMock.Setup(repo => repo.GetAllOfPatient(invalidPatientJmbg)).Returns(new List<Appointment>());

            // Act
            var result = appointmentService.GetAllOfPatient(invalidPatientJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }


    }
}
