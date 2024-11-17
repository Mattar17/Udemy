
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Presentation.Extensions;
using Udemy.Repository.Context;
using Udemy.Repository.Repositories;

namespace Udemy.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<ICourseRepository,CourseRepository>();
            builder.Services.AddScoped<ICourseChapterRepository,ChapterRepository>();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddJWTAuthenticationSechma(builder.Configuration);
            builder.Services.EmailServices(builder.Configuration);
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
        }
    }
}
