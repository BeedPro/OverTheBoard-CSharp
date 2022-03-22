using System;
using System.Threading.Tasks;

namespace OverTheBoard.Infrastructure.Services
{
    public interface IEmailService
    {
        Task<bool> SendRegistrationEmailAsync(string email, string displayName, string link);
        Task<bool> SendPasswordResetEmailAsync(string email, string displayName, string token, string callback);
    }
}