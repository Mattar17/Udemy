using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Models
{
    public class CourseChapter
    {
        public int Id { get; set; }
        public string Chapter_Name { get; set; }
        public int Chapter_Number { get; set; }
        public Course Course { get; set; }
        public string Course_id { get; set; }
        public ICollection<ChapterLecture>? Chapter_Lectures { get; set; }
    }
}
