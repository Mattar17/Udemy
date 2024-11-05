using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Models;

namespace Udemy.Domain.Contracts
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(ApplicationUser user , UserManager<ApplicationUser> userManager);
    }
}
