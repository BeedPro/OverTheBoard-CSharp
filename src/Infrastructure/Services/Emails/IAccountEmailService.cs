using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Services
{
    public interface IAccountEmailService
    {
        Task<bool> SendRegistrationEmailAsync(string email, string displayName, string link);
        Task<bool> SendPasswordResetEmailAsync(string email, string displayName, string token, string callback);
    }

    public interface ITournamentEmailService
    {
        Task<bool> SendInitialEmailAsync(string userEmail, List<ChessGame> matches);
    }
}