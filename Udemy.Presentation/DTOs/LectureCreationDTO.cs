using Udemy.Domain.Models;

namespace Udemy.Presentation.DTOs
{
    public class LectureCreationDTO
    {
        public string Lecture_Title { get; set; }
        public int ChapterId { get; set; }
        public IFormFile MediaUrl { get; set; }
    }
}
