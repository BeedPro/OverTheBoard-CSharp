using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Options;

namespace OverTheBoard.Infrastructure.Services
{
    public class TournamentEmailService : EmailServiceBase, ITournamentEmailService
    {
        private readonly EmailSettingOptions _options;

        public TournamentEmailService(IOptions<EmailSettingOptions> options, ILogger<EmailServiceBase> logger)
            : base(options.Value, logger)
        {
            _options = options.Value;
        }

        public async Task<bool> SendInitialEmailAsync(string userEmail, List<ChessGame> matches)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("all games:");
            foreach (ChessGame game in matches)
            {
                sb.AppendLine($"on {game.StartTime}");
            }
            
            sb.AppendLine("all games:");
            var body = sb.ToString();
            var subject = "games";
            SendEmail(userEmail, subject, body);

            return true;
        }
    }
}