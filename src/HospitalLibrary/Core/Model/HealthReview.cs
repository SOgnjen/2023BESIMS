using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class HealthReview
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int PatientJmbg { get; set; }
        [Required]
        public string Diagnosis { get; set; }
        [Required]
        public string Cure { get; set; }

        public HealthReview(DateTime date, int patientJmbg, string diagnosis, string cure)
        {
            Date = date;
            PatientJmbg = patientJmbg;
            Diagnosis = diagnosis;
            Cure = cure;
        }

        public HealthReview() { }
    }
}
