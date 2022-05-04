using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "First Name Required")]
        [MinLength(2, ErrorMessage = "Minimum of 2 Characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [MinLength(2, ErrorMessage = "Minimum of 2 Characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [MinLength(8, ErrorMessage = "Minimum of 8 Characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password")]
        [MinLength(8, ErrorMessage = "Minimum of 8 Characters.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email is invalid!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Attendance> AttendWed { get; set; }
    }
}