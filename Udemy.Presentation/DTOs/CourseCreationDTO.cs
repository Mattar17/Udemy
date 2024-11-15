namespace Udemy.Presentation.DTOs
{
    public class CourseCreationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? PreviewPictureUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public string Level { get; set; }
        public int CategoryId { get; set; }
    }
}
