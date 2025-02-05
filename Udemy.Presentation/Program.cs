
using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.Extensions;
using Udemy.Presentation.Helpers;
using Udemy.Repository.Context;
using Udemy.Repository.Repositories;
using Udemy.Services;

namespace Udemy.Presentation
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services

            builder.Services.AddControllers().AddJsonOptions(x => x.
            JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.ObjectLifeTime();

            builder.Services.AddTransient<FileUpload>();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddJWTAuthenticationSechma(builder.Configuration);
            builder.Services.EmailServices(builder.Configuration);

            var stripeSettings = builder.Configuration.GetSection("Stripe");
            Stripe.StripeConfiguration.ApiKey = stripeSettings["SecretKey"];

            #region Cloudinary Configuration

            var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");
            Account account = new Account(
                cloudinaryConfig["CloudName"] ,
                cloudinaryConfig["ApiKey"] ,
                cloudinaryConfig["ApiSecret"]
                );

            Cloudinary cloudinary = new Cloudinary( account );
            builder.Services.AddSingleton(cloudinary);
            

            #endregion

            #endregion
            var app = builder.Build();

            #region Middlewares

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion

            #region Roles + Admin Seeding
            //using (var Scope = app.Services.CreateScope())
            //{
            //    var RoleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //    var Roles = new[] { "Admin" , "Instructor" , "Student" };

            //    foreach (var Role in Roles)
            //    {
            //        if (!await RoleManager.RoleExistsAsync(Role))
            //        {
            //            await RoleManager.CreateAsync(new IdentityRole(Role));
            //        }
            //    }


            //}

            //using (var Scope = app.Services.CreateScope())
            //{
            //    var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //    var Admin = await userManager.FindByEmailAsync("admin@udemy.com");

            //    if (Admin == null)
            //    {
            //        var admin = new ApplicationUser()
            //        {
            //            Email = "admin@udemy.com" ,
            //            UserName = "Admin" ,
            //            DisplayName = "Admin"
            //        };

            //        var result = await userManager.CreateAsync(admin , "UdemyAdmin@123");

            //        if (result.Succeeded)
            //        {
            //            await userManager.AddToRoleAsync(admin , "Admin");
            //        }
            //    }

            //    else
            //    {
            //        if (!await userManager.IsInRoleAsync(Admin , "Admin"))
            //        {
            //            await userManager.AddToRoleAsync(Admin , "Admin");
            //        }
            //    }


            //}
            #endregion


            app.Run();
            return Task.CompletedTask;
        }
    }
}
