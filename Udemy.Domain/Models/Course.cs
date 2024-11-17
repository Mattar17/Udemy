using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Udemy.Domain.Enums;

namespace Udemy.Domain.Models
{
    public class Course
    {
        public Course()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? PreviewPictureUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreationDate { get; set; }
        public string Level { get; set; }
        public int CategoryId { get; set; }
        public CourseCategory Category { get; set; }
        public string? InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; }
        public ICollection<CourseReview>? CourseReviews { get; set; } = new HashSet<CourseReview>();
        public ICollection<CourseChapter>? Course_Chapters { get; set; } = new HashSet<CourseChapter>();
        public ICollection<Student_Course>? Course_Students { get; set; } = new HashSet<Student_Course>();


    }
}
