using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeddingPlanner.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Wedding Wedding { get; set; }
        public int WeddingId { get; set; }
    }
}