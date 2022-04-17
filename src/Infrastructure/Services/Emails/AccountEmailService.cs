using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Options;

namespace OverTheBoard.Infrastructure.Services
{
    public class AccountEmailService : EmailServiceBase, IAccountEmailService
    {
        private readonly EmailSettingOptions _options;

        public AccountEmailService(IOptions<EmailSettingOptions> options, ILogger<EmailServiceBase> logger)
            :base(options.Value, logger)
        {
            _options = options.Value;
        }
        public async Task<bool> SendRegistrationEmailAsync(string email, string displayName,string link)
        {
            var body = $"<h3>OverTheBoard</h3>\r\n<p>Hello {displayName}. Welcome to OverTheBoard where you play Chess in a fun and gamified Tournament based system. To verify your account please click the <a href=\"{link}\">link</a>.";
            var subject = "OverTheBoard Email Verification";
            SendEmail(email, subject, body);
            return true;
        }

        
        public async Task<bool> SendPasswordResetEmailAsync(string email, string displayName,string token, string callback)
        {
            var link = callback;
            var body = $"<h3>OverTheBoard</h3>\r\n<p>Hello {displayName}. Welcome to OverTheBoard where you play Chess in a fun and gamified Tournament based system. To reset your account password please click the <a href=\"{link}\">link</a>.";
            var subject = "OverTheBoard Password Reset";
            SendEmail(email, subject, body);
            return true;
        }
    }
}
