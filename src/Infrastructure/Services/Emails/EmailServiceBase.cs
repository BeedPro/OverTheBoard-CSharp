using System.Net;
using System.Net.Mail;
using OverTheBoard.ObjectModel.Options;

namespace OverTheBoard.Infrastructure.Services
{
    public class EmailServiceBase
    {
        private readonly EmailSettingOptions _options;

        public EmailServiceBase(EmailSettingOptions options)
        {
            _options = options;
        }

        protected virtual void SendEmail(string email, string subject, string body)
        {
            using var smtpClient = new SmtpClient(_options.SmtpServer)
            {
                Port = 25,
                Credentials = new NetworkCredential(_options.Username, _options.Password),
                EnableSsl = false
            };


            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_options.Sender),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);
        }

    }
}