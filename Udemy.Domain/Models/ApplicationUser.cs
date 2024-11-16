using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? PricureUrl { get; set; }

        public ICollection<Course>? Instructor_Courses { get; set; }
        public ICollection<Student_Course>? student_Courses { get; set; } = new HashSet<Student_Course>();
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        //public ICollection<Message>? SentMessages { get; set; }
        //public ICollection<Message>? ReceivedMessages { get; set; }
    }
}
