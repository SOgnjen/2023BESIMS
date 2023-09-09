using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Emails { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        public UserRole Role { get; set; }
        [Required]
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public int Jmbg { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public bool IsBlocked { get; set; }
        public GuidanceTo Guidance { get; set; }

        public User(string emails, string password, string firstName, string lastName, UserRole role, string address, string phoneNumber, int jmbg, Gender gender)
        {
            Emails = emails;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Address = address;
            PhoneNumber = phoneNumber;
            Jmbg = jmbg;
            Gender = gender;
            IsBlocked = false;
            Guidance = GuidanceTo.None;
        }

        public User() { }
    }
}
