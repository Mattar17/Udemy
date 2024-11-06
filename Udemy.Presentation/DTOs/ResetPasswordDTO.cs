using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Udemy.Presentation.DTOs
{
    public class ResetPasswordDTO
    {
        [Required,EmailAddress(ErrorMessage ="Invalid EmailAddress")]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required,PasswordPropertyText]
        public string NewPassword { get; set; }
        [Required, PasswordPropertyText]
        public string ConfirmPassword { get; set; }
    }
}
