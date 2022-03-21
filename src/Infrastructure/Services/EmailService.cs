using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingOptions _options;

        public EmailService(IOptions<EmailSettingOptions> options)
        {
            _options = options.Value;
        }
        public async Task<bool> SendRegistrationEmailAsync(string email, string link)
        {
            var smtpClient = new SmtpClient(_options.SmtpServer)
            {
                Port = 25, // 587 also works
                Credentials = new NetworkCredential(_options.Username, _options.Password),
                EnableSsl = false,
            };

            smtpClient.Send(_options.Sender, email, "OverTheBoard Email Verification", $"Hello please activate by clicking the link: {link}");
            return true;
        }
    }
}
