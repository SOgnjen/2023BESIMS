using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public class AppointmentService : IAppointmenService
    {
        private readonly IAppointmentRepository _appointmentReposiotry;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentReposiotry = appointmentRepository;
        }

        public void Create(Appointment appointment)
        {
            _appointmentReposiotry.Create(appointment);
        }

        public void Delete(Appointment appointment)
        {
            _appointmentReposiotry.Delete(appointment);
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _appointmentReposiotry.GetAll();
        }

        public Appointment GetById(int id)
        {
            return _appointmentReposiotry.GetById(id);
        }

        public void Update(Appointment appointment)
        {
            _appointmentReposiotry.Update(appointment);
        }
    }
}
