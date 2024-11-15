using Udemy.Domain.Enums;
using Udemy.Domain.Models;

namespace Udemy.Presentation.DTOs
{
    public class CourseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? PreviewPictureUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreationDate { get; set; }
        public string Level { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
