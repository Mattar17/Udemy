using System.ComponentModel.DataAnnotations;

namespace Udemy.Presentation.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public IList<string> Role { get; set; }
        public string Token { get; set; }
    }
}
