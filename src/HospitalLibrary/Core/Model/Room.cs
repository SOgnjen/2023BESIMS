using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Number { get; set; }
        [Range(1, 10)]
        public int Floor { get; set; }
    }
}
