using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAll();
        Appointment GetById(int id);
        void Create(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Appointment appointment);
    }
}
