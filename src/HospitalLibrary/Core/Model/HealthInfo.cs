using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class HealthInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int OwnersJmbg { get; set; }
        [Required]
        public int UpperBloodPreassure { get; set; }
        [Required]
        public int LowerBloodPreassure { get; set; }
        [Required]
        public int SugarLevel { get; set; }
        [Required]
        public double FatPercentage { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public DateTime LastMenstruation { get; set; }

        public HealthInfo(DateTime date, int ownersJmbg, int upperBloodPreassure, int lowerBloodPreassure, int sugarLevel, double fatPercentage, double weight, DateTime lastMenstruation)
        {
            Date = date;
            OwnersJmbg = ownersJmbg;
            UpperBloodPreassure = upperBloodPreassure;
            LowerBloodPreassure = lowerBloodPreassure;
            SugarLevel = sugarLevel;
            FatPercentage = fatPercentage;
            Weight = weight;
            LastMenstruation = lastMenstruation;
        }

        public HealthInfo() { }
    }
}
