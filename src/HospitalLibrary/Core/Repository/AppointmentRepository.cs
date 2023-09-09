using HospitalLibrary.Core.Model;
using HospitalLibrary.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalDbContext _context;

        public AppointmentRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public void Create(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public void Delete(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
        }

        public Appointment FindIdealAppointment(int doctorsJmbg, DateTime date)
        {
            return _context.Appointments
                      .FirstOrDefault(appointment => appointment.DoctorJmbg == doctorsJmbg &&
                                      appointment.Date.Date == date.Date &&
                                      appointment.PatientJmbg == 0);
        }

        public Appointment FindAppointmentDoctorPriority(int doctorsJmbg, DateTime date)
        {
            DateTime startDate = date.AddDays(-7);
            DateTime endDate = date.AddDays(7);

            return _context.Appointments
                .FirstOrDefault(appointment =>
                    appointment.DoctorJmbg == doctorsJmbg &&
                    appointment.Date.Date >= startDate.Date &&
                    appointment.Date.Date <= endDate.Date &&
                    appointment.PatientJmbg == 0);
        }

        public Appointment FindAppointmentDatePriority(int doctorsJmbg, DateTime date)
        {
            var doctor = _context.Users.FirstOrDefault(u => u.Jmbg == doctorsJmbg);
            if (doctor == null)
            {
                return null;
            }

            var usersWithSameRole = _context.Users.Where(u => u.Role == doctor.Role && u.Jmbg != doctorsJmbg);

            foreach (var user in usersWithSameRole)
            {
                var appointment = _context.Appointments.FirstOrDefault(app =>
                app.DoctorJmbg == user.Jmbg &&
                app.PatientJmbg == 0 &&
                app.Date.Date == date.Date);

                if (appointment != null)
                {
                    return appointment;
                }
            }

            return null;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments.ToList();
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments.Find(id);
        }

        public void Update(Appointment appointment)
        {
            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public Appointment FindMeAppointment(int doctorsJmbg, DateTime date, int priority)
        {
            Appointment appointment = null;

            appointment = FindIdealAppointment(doctorsJmbg, date);

            if (appointment == null && priority == 1)
            {
                appointment = FindAppointmentDoctorPriority(doctorsJmbg, date);
            }
            else if (appointment == null && priority == 2)
            {
                appointment = FindAppointmentDatePriority(doctorsJmbg, date);
            }

            return appointment;
        }

        public void ReserveAppointment(Appointment appointment, int userJmbg)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            if (userJmbg <= 0)
            {
                throw new ArgumentException("Invalid userJmbg value.", nameof(userJmbg));
            }

            if (appointment.PatientJmbg == 0)
            {
                appointment.PatientJmbg = userJmbg;
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Appointment is already reserved.");
            }
        }

        public void DeclineAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            if (IsAppointmentInFuture(appointment, TimeSpan.FromDays(2)))
            {
                appointment.PatientJmbg = 0;
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Cannot decline appointment that is less than 2 days away.");
            }
        }

        private bool IsAppointmentInFuture(Appointment appointment, TimeSpan threshold)
        {
            TimeSpan timeUntilAppointment = appointment.Date - DateTime.Now;

            return timeUntilAppointment >= threshold;
        }
    }
}
