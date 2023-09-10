using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public interface IAppointmenService
    {
        IEnumerable<Appointment> GetAll();
        Appointment GetById(int id);
        void Create(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Appointment appointment);
        Appointment FindMeAppointment(int doctorsJmbg, DateTime date, int priority);
        void ReserveAppointment(Appointment appointment, int userJmbg);
        void DeclineAppointment(Appointment appointment);
        IEnumerable<Appointment> GetAllOfPatient(int patientsJmbg);
    }
}
