using System;
using System.Threading.Tasks;

namespace OverTheBoard.Infrastructure.Services
{
    public interface IEmailService
    {
        Task<bool> SendRegistrationEmailAsync(string email, string link);
    }
}