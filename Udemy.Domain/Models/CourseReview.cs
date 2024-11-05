using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Models
{
    public class CourseReview
    {
        public int Id { get; set; }
        public string Review_Content { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsApproved { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Course Course { get; set; }
        public string CourseId { get; set; }

    }
}
