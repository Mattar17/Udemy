using AutoMapper;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;

namespace Udemy.Presentation.Helpers
{
    public class UserPictureResolver : IValueResolver<ApplicationUser , LoginDTO , string>
    {
        public string Resolve(ApplicationUser source , LoginDTO destination , string destMember , ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
