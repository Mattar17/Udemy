using System.Runtime.CompilerServices;
using Udemy.Domain.Contracts;
using Udemy.Repository.Repositories;
using Udemy.Services;

namespace Udemy.Presentation.Extensions
{
    public static class ObjectLifeTimeExtension
    {
        public static IServiceCollection ObjectLifeTime(this IServiceCollection Service)
        {
            Service.AddScoped<IUnitOfWork , UnitOfWork>();
            Service.AddScoped<ICourseRepository , CourseRepository>();
            Service.AddScoped<ICourseChapterRepository , ChapterRepository>();
            Service.AddScoped<ILectureRepository , LectureRepository>();
            Service.AddScoped<IStudentCourseRepo , StudentCourseRepo>();
            Service.AddScoped<IPaymentService , StripePaymentService>();
            Service.AddScoped<IReviewRepository , ReviewRepository>();

            return Service;
        }
    }
}
