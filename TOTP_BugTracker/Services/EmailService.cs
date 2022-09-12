using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using TOTP_BugTracker.Models;

namespace TOTP_BugTracker.Services
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }




        public async Task SendEmailAsync(string userEmail, string subject, string htmlMessage)
        {

            // View is projectId, Email, FirstName, LastName, and Message. that's it

            using var smtp = new SmtpClient();
            string configEmail = _mailSettings.EmailAddress ?? Environment.GetEnvironmentVariable("EmailAddress")!;
            string host = _mailSettings.EmailHost ?? Environment.GetEnvironmentVariable("EmailHost")!;
            int port = _mailSettings.EmailPort != 0 ? _mailSettings.EmailPort : int.Parse(Environment.GetEnvironmentVariable("EmailPort")!);
            var emailSender = _mailSettings.EmailAddress;

            MimeMessage newEmail = new();
            newEmail.Sender = MailboxAddress.Parse(configEmail);

            //add the subject for the email
            newEmail.Subject = subject;

            //add the boy for the email
            BodyBuilder emailBody = new();
            emailBody.HtmlBody = htmlMessage;
            newEmail.Body = emailBody.ToMessageBody();
            newEmail.To.Add(MailboxAddress.Parse(userEmail));

            //Send the email
            using SmtpClient smtpClient = new();
            try
            {
                await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(emailSender, _mailSettings.EmailPassword ?? Environment.GetEnvironmentVariable("EmailPassword"));

                await smtpClient.SendAsync(newEmail);
                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }
        }
    }
}
