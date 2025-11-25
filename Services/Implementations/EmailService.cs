using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Wedding_Planner.Application.Services.Interfaces;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _fromEmail = "pallavisrinivas1234@gmail.com";
        private readonly string _fromPassword = "tpri sfvi jhyy kwev";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Wedding Planner", _fromEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_fromEmail, _fromPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                
                Console.WriteLine($"Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email failed to {toEmail}: {ex.Message}");
            }
        }
    }
}
