using FluentAssertions;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using HospitalLibrary.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HospitalTestUnit.Systems.Repositories
{
    public class TestAppointmentRepository : IDisposable
    {
        private readonly List<Appointment> appointments;
        private HospitalDbContext dbContext;

        public TestAppointmentRepository()
        {
            appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 9, 9, 10, 0, 0)
                },
                new Appointment
                {
                    Id = 2,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 10, 18, 14, 30, 0)
                },
                new Appointment
                {
                    Id = 3,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 20, 9, 15, 0)
                },
                new Appointment
                {
                    Id = 4,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 10, 15, 10, 0, 0)
                }
            };
        }

        private HospitalDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<HospitalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new HospitalDbContext(dbContextOptions);
        }

        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }
        }

        [Fact]
        public void GetAll_ReturnAllAppointments()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            // Act
            var result = appointmentRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointments);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoAppointmentsExist()
        {
            // Arrange
            dbContext = CreateDbContext();
            var appointmentRepository = new AppointmentRepository(dbContext);

            // Act
            var result = appointmentRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetById_ReturnsAppointmentWithValidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int appointmentToFind = 1;
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            // Act
            var result = appointmentRepository.GetById(appointmentToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointments.FirstOrDefault(a => a.Id == appointmentToFind));
        }

        [Fact]
        public void GetById_ReturnsNullForInvalidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int appointmentToFind = 999;
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            // Act
            var result = appointmentRepository.GetById(appointmentToFind);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Create_AddAppointmentToDatabase()
        {
            // Arrange
            dbContext = CreateDbContext();
            var appointmentRepository = new AppointmentRepository(dbContext);
            var newAppointment = new Appointment
            {
                DoctorJmbg = 987654321,
                PatientJmbg = 1111111111,
                Date = new DateTime(2023, 11, 10, 14, 30, 0)
            };

            // Act
            appointmentRepository.Create(newAppointment);
            dbContext.SaveChanges();

            // Assert
            var addedAppointment = dbContext.Appointments.FirstOrDefault(a => a.Id == newAppointment.Id);
            addedAppointment.Should().NotBeNull();
            addedAppointment.DoctorJmbg.Should().Be(newAppointment.DoctorJmbg);
            addedAppointment.PatientJmbg.Should().Be(newAppointment.PatientJmbg);
            addedAppointment.Date.Should().Be(newAppointment.Date);
        }

        [Fact]
        public void Update_ModifiesExistingAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            var appointmentRepository = new AppointmentRepository(dbContext);

            var appointmentToAdd = new Appointment
            {
                DoctorJmbg = 987654321,
                PatientJmbg = 1111111111,
                Date = new DateTime(2023, 11, 10, 14, 30, 0)
            };

            dbContext.Appointments.Add(appointmentToAdd);
            dbContext.SaveChanges();

            var retrievedAppointment = dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentToAdd.Id);

            retrievedAppointment.DoctorJmbg = 1234567890;
            retrievedAppointment.PatientJmbg = 373737373;
            retrievedAppointment.Date = new DateTime(2023, 12, 5, 11, 0, 0);

            // Act
            appointmentRepository.Update(retrievedAppointment);
            dbContext.SaveChanges();

            var updatedAppointment = dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentToAdd.Id);

            // Assert
            updatedAppointment.Should().NotBeNull();
            updatedAppointment.DoctorJmbg.Should().Be(1234567890);
            updatedAppointment.PatientJmbg.Should().Be(373737373);
            updatedAppointment.Date.Should().Be(new DateTime(2023, 12, 5, 11, 0, 0));
        }

        [Fact]
        public void Delete_ExistingAppointment_SuccessfullyDeletesAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            var appointmentRepository = new AppointmentRepository(dbContext);

            var appointmentToDelete = new Appointment
            {
                DoctorJmbg = 987654321,
                PatientJmbg = 1111111111,
                Date = new DateTime(2023, 11, 10, 14, 30, 0)
            };

            dbContext.Appointments.Add(appointmentToDelete);
            dbContext.SaveChanges();

            // Act
            appointmentRepository.Delete(appointmentToDelete);
            dbContext.SaveChanges();

            var deletedAppointment = dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentToDelete.Id);

            // Assert
            deletedAppointment.Should().BeNull();
        }

        [Fact]
        public void FindAppointment_ShouldReturnMatchingAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            int doctorJmbgToFind = 987654321;
            DateTime dateToFind = new DateTime(2023, 10, 15, 10, 0, 0);

            // Act
            var result = appointmentRepository.FindIdealAppointment(doctorJmbgToFind, dateToFind);

            // Assert
            result.Should().NotBeNull();
            result.DoctorJmbg.Should().Be(doctorJmbgToFind);
            result.Date.Should().Be(dateToFind);
        }

        [Fact]
        public void FindAppointment_ShouldReturnNullForNonExistentAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            int doctorJmbgToFind = 123456789;
            DateTime dateToFind = new DateTime(2023, 10, 15, 10, 0, 0);

            // Act
            var result = appointmentRepository.FindIdealAppointment(doctorJmbgToFind, dateToFind);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FindAppointmentDoctorPriority_ReturnsMatchingAppointmentInDateRange()
        {
            // Arrange
            dbContext = CreateDbContext();
            var appointmentRepository = new AppointmentRepository(dbContext);

            var doctorJmbg = 987654321;
            var targetDate = new DateTime(2023, 10, 18, 14, 30, 0);
            var matchingAppointment = new Appointment
            {
                DoctorJmbg = doctorJmbg,
                PatientJmbg = 0,
                Date = targetDate.AddDays(3) // This appointment is within the date range
            };
            var nonMatchingAppointment = new Appointment
            {
                DoctorJmbg = doctorJmbg,
                PatientJmbg = 0,
                Date = targetDate.AddDays(10) // This appointment is outside the date range
            };

            dbContext.Appointments.AddRange(matchingAppointment, nonMatchingAppointment);
            dbContext.SaveChanges();

            // Act
            var result = appointmentRepository.FindAppointmentDoctorPriority(doctorJmbg, targetDate);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(matchingAppointment);
        }

        [Fact]
        public void FindAppointmentDoctorPriority_ReturnsNullWhenNoMatchingAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            var appointmentRepository = new AppointmentRepository(dbContext);

            var doctorJmbg = 987654321;
            var targetDate = new DateTime(2023, 10, 18, 14, 30, 0);
            var nonMatchingAppointment = new Appointment
            {
                DoctorJmbg = doctorJmbg,
                PatientJmbg = 0,
                Date = targetDate.AddDays(10)
            };

            dbContext.Appointments.Add(nonMatchingAppointment);
            dbContext.SaveChanges();

            // Act
            var result = appointmentRepository.FindAppointmentDoctorPriority(doctorJmbg, targetDate);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FindAppointmentDatePriority_ReturnsNullForNonExistentDoctor()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            int doctorJmbgToFind = 123456789;
            DateTime dateToFind = new DateTime(2023, 10, 15, 10, 0, 0);

            // Act
            var result = appointmentRepository.FindAppointmentDatePriority(doctorJmbgToFind, dateToFind);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FindMeAppointment_ShouldReturnIdealAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            int doctorJmbgToFind = 987654321;
            DateTime dateToFind = new DateTime(2023, 10, 15, 10, 0, 0);

            // Act
            var result = appointmentRepository.FindMeAppointment(doctorJmbgToFind, dateToFind, 1);

            // Assert
            result.Should().NotBeNull();
            result.DoctorJmbg.Should().Be(doctorJmbgToFind);
            result.Date.Should().Be(dateToFind);
        }

        [Fact]
        public void FindMeAppointment_ShouldReturnDoctorPriorityAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            int doctorJmbgToFind = 987654321;
            DateTime dateToFind = new DateTime(2023, 10, 8, 14, 30, 0);

            // Act
            var result = appointmentRepository.FindMeAppointment(doctorJmbgToFind, dateToFind, 1);

            // Assert
            result.Should().NotBeNull();
            result.DoctorJmbg.Should().Be(doctorJmbgToFind);
            result.Date.Should().Be(new DateTime(2023, 10, 15, 10, 0, 0));
        }

        [Fact]
        public void ReserveAppointment_ReservesAvailableAppointmentWithWholeAppointmentObject()
        {
            // Arrange
            dbContext = CreateDbContext();
            var appointmentRepository = new AppointmentRepository(dbContext);

            var appointmentToReserve = new Appointment
            {
                DoctorJmbg = 987654321,
                PatientJmbg = 0,
                Date = new DateTime(2023, 11, 10, 14, 30, 0)
            };

            dbContext.Appointments.Add(appointmentToReserve);
            dbContext.SaveChanges();

            int userJmbg = 1111111111;

            // Act
            appointmentRepository.ReserveAppointment(appointmentToReserve, userJmbg);

            // Assert
            var reservedAppointment = dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentToReserve.Id);
            reservedAppointment.Should().NotBeNull();
            reservedAppointment.PatientJmbg.Should().Be(userJmbg);
        }

        [Fact]
        public void DeclineAppointment_ShouldSetPatientJmbgToZeroForFutureAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            int appointmentIdToDecline = 2;

            // Act
            var appointmentToDecline = dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentIdToDecline);
            appointmentRepository.DeclineAppointment(appointmentToDecline);

            // Assert
            var declinedAppointment = dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentIdToDecline);
            declinedAppointment.Should().NotBeNull();
            declinedAppointment.PatientJmbg.Should().Be(0);
        }

        [Fact]
        public void DeclineAppointment_ShouldThrowExceptionForPastAppointment()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            int appointmentIdToDecline = 1;

            // Act & Assert
            var appointmentToDecline = dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentIdToDecline);
            Action act = () => appointmentRepository.DeclineAppointment(appointmentToDecline);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot decline appointment that is less than 2 days away.");
        }

        [Fact]
        public void GetAllOfPatient_ReturnsAppointmentsForMatchingPatient()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            var targetPatientJmbg = 1234567890;
            var appointmentsWithMatchingPatient = appointments.Where(a => a.PatientJmbg == targetPatientJmbg).ToList();

            // Act
            var result = appointmentRepository.GetAllOfPatient(targetPatientJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointmentsWithMatchingPatient);
        }

        [Fact]
        public void GetAllOfPatient_ReturnsEmptyListForInvalidPatient()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Appointments.AddRange(appointments);
            dbContext.SaveChanges();
            var appointmentRepository = new AppointmentRepository(dbContext);

            var invalidPatientJmbg = 999999999;

            // Act
            var result = appointmentRepository.GetAllOfPatient(invalidPatientJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

    }
}
