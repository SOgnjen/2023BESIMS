using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class BloodAppointment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("date_of_appointment")]
        public DateTime When { get; set; }
        [Column("is_free")]
        public bool IsFree { get; set; }
        [Column("owner_jmbg")]
        public int OwnerJmbg { get; set; }

        public BloodAppointment() { }

        public BloodAppointment(DateTime when, bool isFree, int ownerJmbg) 
        {
            When = when;
            IsFree = isFree;
            OwnerJmbg = ownerJmbg;
        }
    }
}
