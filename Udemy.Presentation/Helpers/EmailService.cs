using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Udemy.Presentation.Helpers
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _options;

        public EmailService(IOptions<EmailSetting> options)
        {
            _options = options.Value;
        }
        public void SendEmailAsync(Email email)
        {
            var mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;

            mail.Body = builder.ToMessageBody();
            mail.From.Add(new MailboxAddress(_options.Username , _options.Email));


            using var smtpClient = new SmtpClient();
            smtpClient.Connect(_options.Host,_options.Port,SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_options.Email , _options.AppPassword);
            smtpClient.SendAsync(mail);
            smtpClient.Disconnect(false);
            
        }
    }
}
