using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class Information
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("date_of_appointment")]
        public DateTime When { get; set; }
        [Column("status")]
        public InformationStatus Status { get; set; }

        public Information() { }

        public Information(DateTime when, InformationStatus status)
        {
            When = when;
            Status = status;
        }
    }
}
