using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TaskManagementSystem.Infrastructure.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string message);
        Task SendVerificationCodeAsync(string to, string code);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Log SMTP settings for debugging
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            _logger.LogInformation("SMTP Settings - Host: {Host}, Port: {Port}, Ssl: {Ssl}, StartTls: {StartTls}, User: {User}, MailFrom: {MailFrom}",
                smtpSettings["Host"],
                smtpSettings["Port"],
                smtpSettings["Ssl"],
                smtpSettings["StartTls"],
                smtpSettings["User"],
                smtpSettings["MailFrom"]);
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                string host = smtpSettings["Host"];
                int port = int.Parse(smtpSettings["Port"]);
                bool ssl = bool.Parse(smtpSettings["Ssl"]);
                // 'StartTls' is provided but not directly used by SmtpClient
                string username = smtpSettings["User"];
                string mailFrom = smtpSettings["MailFrom"];
                // Optionally, if a password is provided (not in your current config), use it.
                string password = smtpSettings["Password"];

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailFrom);
                    mail.To.Add(new MailAddress(to));
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(host, port))
                    {
                        smtp.EnableSsl = ssl;

                        // If a password is provided, use credentials; otherwise, assume no authentication.
                        if (!string.IsNullOrEmpty(password))
                        {
                            smtp.Credentials = new NetworkCredential(username, password);
                        }
                        else
                        {
                            smtp.Credentials = null;
                        }

                        await smtp.SendMailAsync(mail);
                        _logger.LogInformation("Email sent successfully to {Email}", to);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Email}", to);
                throw;
            }
        }

        public async Task SendVerificationCodeAsync(string to, string code)
        {
            string subject = "Your Verification Code";
            string message = $"Your one-time verification code is: {code}";
            await SendEmailAsync(to, subject, message);
        }
    }
}
