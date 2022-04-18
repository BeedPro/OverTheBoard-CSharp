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
            sb.AppendLine("A new tournament has been created. All games have been listed below with their timings:");
            int i = 1;
            foreach (ChessGame game in matches)
            {
                sb.AppendLine($"Game {i} on {game.StartTime}");
                i++;
            }
            
            var body = sb.ToString();
            var subject = "Tournament";
            SendEmail(userEmail, subject, body);

            return true;
        }
    }
}