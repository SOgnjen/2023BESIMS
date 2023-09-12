using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public interface IBloodAppointmentRepository
    {
        IEnumerable<BloodAppointment> GetAll();
        BloodAppointment GetById(int id);
        void Create(BloodAppointment bloodAppointment);
        void Update(BloodAppointment bloodAppointment);
        void Delete(BloodAppointment bloodAppointment);
        IEnumerable<BloodAppointment> GetAllFree();
        void ReserveBloodAppointment(BloodAppointment bloodAppointment, int userJmbg);

    }
}
