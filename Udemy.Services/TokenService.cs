using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;

namespace Udemy.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser User , UserManager<ApplicationUser> userManager)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var cred = new SigningCredentials(Key , SecurityAlgorithms.HmacSha256Signature);
            var AuthClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.GivenName,User.DisplayName),
                new Claim (ClaimTypes.Email,User.Email),
            };
            var UserRoles = await userManager.GetRolesAsync(User);

            foreach (var Role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role , Role));
            }
            var Token = new JwtSecurityToken(
                issuer: configuration["JWT:issuer"] ,
                audience: configuration["JWT:audience"] ,
                claims: AuthClaims ,
                expires: DateTime.Now.AddDays(1) ,
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
