using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Udemy.Presentation.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required]
        public string Email { get; set; }
        [PasswordPropertyText]
        [Required]
        public string Password { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
