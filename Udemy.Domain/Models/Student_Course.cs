using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Models
{
    public class Student_Course
    {
        public DateTime EnrollmentDate { get; set; }
        public int LastWatchedLectureId { get; set; }
        public int CompletedPercentage { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string CourseId { get; set; }
        public Course Course { get; set; }
    }
}
