using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Models
{
    public class CourseCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public ICollection<Course>? courses { get; set; }  = new HashSet<Course>();
    }
}
