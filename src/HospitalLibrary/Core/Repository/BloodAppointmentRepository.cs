using HospitalLibrary.Core.Model;
using HospitalLibrary.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public class BloodAppointmentRepository : IBloodAppointmentRepository
    {
        private HospitalDbContext _context;

        public BloodAppointmentRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public void Create(BloodAppointment bloodAppointment)
        {
            _context.blood_appointments.Add(bloodAppointment);
            _context.SaveChanges();
        }

        public void Delete(BloodAppointment bloodAppointment)
        {
            _context.blood_appointments.Remove(bloodAppointment);
            _context.SaveChanges();
        }

        public IEnumerable<BloodAppointment> GetAll()
        {
            return _context.blood_appointments.ToList();
        }

        public IEnumerable<BloodAppointment> GetAllFree()
        {
            return _context.blood_appointments.Where(appointment => appointment.IsFree).ToList();
        }

        public BloodAppointment GetById(int id)
        {
            return _context.blood_appointments.Find(id);
        }

        public void ReserveBloodAppointment(BloodAppointment bloodAppointment, int userJmbg)
        {
            if (bloodAppointment.IsFree)
            {
                bloodAppointment.OwnerJmbg = userJmbg;
                bloodAppointment.IsFree = false;

                _context.Entry(bloodAppointment).State = EntityState.Modified;

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        public void Update(BloodAppointment bloodAppointment)
        {
            _context.Entry(bloodAppointment).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
