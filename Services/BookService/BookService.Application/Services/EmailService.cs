using System.Net;
using System.Net.Mail;
using LibraryWebApp.BookService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LibraryWebApp.BookService.Application.Services
{

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string email, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var portString = _configuration["EmailSettings:Port"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var fromAddress = _configuration["EmailSettings:FromAddress"];

            if (string.IsNullOrEmpty(fromAddress))
            {
                throw new ArgumentNullException(nameof(fromAddress), "From address must be provided.");
            }

            if (string.IsNullOrEmpty(portString) || !int.TryParse(portString, out int port))
            {
                throw new ArgumentException("Valid port number must be provided.", nameof(portString));
            }

            using (var client = new SmtpClient(smtpServer, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                client.Send(mailMessage);
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var portString = _configuration["EmailSettings:Port"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var fromAddress = _configuration["EmailSettings:FromAddress"];

            if (string.IsNullOrEmpty(fromAddress))
            {
                throw new ArgumentNullException(nameof(fromAddress), "From address must be provided.");
            }

            if (string.IsNullOrEmpty(portString) || !int.TryParse(portString, out int port))
            {
                throw new ArgumentException("Valid port number must be provided.", nameof(portString));
            }

            using (var client = new SmtpClient(smtpServer, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true; 

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
