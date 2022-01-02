using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Repositories;

namespace OverTheBoard.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ISecurityRepository<OverTheBoardUser> _securityRepository;

        public UserService(ISecurityRepository<OverTheBoardUser> securityRepository)
        {
            _securityRepository = securityRepository;
        }


        public async Task<OverTheBoardUser> GetUserDisplayNameAndyNameIdAsync(string displayName, string number)
        {
            return await _securityRepository.Query().FirstOrDefaultAsync(e =>
                e.DisplayName == displayName && e.DisplayNameId == number);
        }

        public async Task<OverTheBoardUser> GetUserAsync(string userId)
        {
            return await _securityRepository.Query().FirstOrDefaultAsync(e =>
                e.Id == userId);
        }
    }
}