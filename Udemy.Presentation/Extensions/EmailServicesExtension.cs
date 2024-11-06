using Udemy.Domain.Contracts;
using Udemy.Presentation.Helpers;

namespace Udemy.Presentation.Extensions
{
    public static class EmailServicesExtension
    {
        public static IServiceCollection EmailServices(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.Configure<EmailSetting>(configuration.GetSection("EmailSetting"));

            Services.AddScoped(typeof(IEmailService) , typeof(EmailService));

            return Services;
        }
    }
}
