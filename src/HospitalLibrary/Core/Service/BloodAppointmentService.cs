using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public class BloodAppointmentService : IBloodAppointmentService
    {
        private readonly IBloodAppointmentRepository _bloodAppointmentRepository;

        public BloodAppointmentService(IBloodAppointmentRepository bloodAppointmentRepository)
        {
            _bloodAppointmentRepository = bloodAppointmentRepository;
        }

        public void Create(BloodAppointment bloodAppointment)
        {
            _bloodAppointmentRepository.Create(bloodAppointment);
        }

        public void Delete(BloodAppointment bloodAppointment)
        {
            _bloodAppointmentRepository.Delete(bloodAppointment);
        }

        public IEnumerable<BloodAppointment> GetAll()
        {
            return  _bloodAppointmentRepository.GetAll();
        }

        public IEnumerable<BloodAppointment> GetAllFree()
        {
            return _bloodAppointmentRepository.GetAllFree();
        }

        public BloodAppointment GetById(int id)
        {
            return _bloodAppointmentRepository.GetById(id);
        }

        public void ReserveBloodAppointment(BloodAppointment bloodAppointment, int userJmbg)
        {
            _bloodAppointmentRepository.ReserveBloodAppointment(bloodAppointment, userJmbg);
        }

        public void Update(BloodAppointment bloodAppointment)
        {
            _bloodAppointmentRepository.Update(bloodAppointment);
        }
    }
}
