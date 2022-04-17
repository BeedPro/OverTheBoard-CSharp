using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OverTheBoard.ObjectModel.Options;

namespace OverTheBoard.Infrastructure.Services
{
    public class EmailServiceBase
    {
        private readonly EmailSettingOptions _options;
        private readonly ILogger<EmailServiceBase> _logger;

        public EmailServiceBase(EmailSettingOptions options, ILogger<EmailServiceBase> logger)
        {
            _options = options;
            _logger = logger;
        }

       
        protected virtual void SendEmail(string email, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient();

                if (_options.UseSmtpServer)
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Host = _options.SmtpServer;
                    client.Port = 25;
                    client.EnableSsl = false;

                    if (!string.IsNullOrEmpty(_options.Username) && !string.IsNullOrEmpty(_options.Password))
                    {
                        client.Credentials = new NetworkCredential(_options.Username, _options.Password);
                    }
                }
                else
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    var location = _options.SpecifiedPickupDirectory;
                    if (string.IsNullOrEmpty(location))
                    {
                        var path = Environment.CurrentDirectory;
                        location = $"{path}{System.IO.Path.DirectorySeparatorChar}Email{System.IO.Path.DirectorySeparatorChar}";
                        if (!Directory.Exists(location))
                        {
                            Directory.CreateDirectory(location);
                        }
                    }
                    client.PickupDirectoryLocation = location;

                }

                var messageMail =
                    new MailMessage(_options.Sender, email, subject, body)
                    {
                        IsBodyHtml = true
                    };

                client.Send(messageMail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Sending email");
            }
        }

    }
}