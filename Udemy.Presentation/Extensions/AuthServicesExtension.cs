using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Repository.Context;
using Udemy.Services;

namespace Udemy.Presentation.Extensions
{
    public static class AuthServicesExtension
    {
        public static IServiceCollection AddJWTAuthenticationSechma(this IServiceCollection Services , IConfiguration configuration)
        {
            Services.AddIdentity<ApplicationUser , IdentityRole>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            Services.AddSingleton(typeof(ITokenService) , typeof(TokenService));

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false ,
                    ValidateAudience = false ,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
            return Services;
        }
    }
}
