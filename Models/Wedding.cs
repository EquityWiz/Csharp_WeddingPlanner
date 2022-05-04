using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [Display(Name = "Wedder One")]
        public string WedderF { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [Display(Name = "Wedder Two")]
        public string WedderS { get; set; }
        [Required(ErrorMessage = "Date of Wedding is Required.")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [Display(Name = "Wedding Address")]
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Attendance> AttendWed { get; set; }
        public int UserId { get; set; }
        public User Author { get; set; }
    }
}