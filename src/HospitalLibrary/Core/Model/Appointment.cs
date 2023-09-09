using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DoctorJmbg { get; set; }
        [Required]
        public int PatientJmbg { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public Appointment(int id, int doctorJmbg, int patientJmbg, DateTime date )
        {
            DoctorJmbg = doctorJmbg;
            PatientJmbg = patientJmbg;
            Date = date;
        }

        public Appointment() { }
    }
}
