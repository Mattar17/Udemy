using Udemy.Domain.Models;

namespace Udemy.Presentation.DTOs
{
    public class ReviewDTO
    {
        public string Review_Content { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsApproved { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        
    }
}
