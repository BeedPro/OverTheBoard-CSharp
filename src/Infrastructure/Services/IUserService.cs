using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Repositories;

namespace OverTheBoard.Infrastructure.Services
{
    public interface IUserService
    {
        Task<OverTheBoardUser> GetUserDisplayNameAndyNameIdAsync(string displayName, string number);
        Task<OverTheBoardUser> GetUserAsync(string userId);
    }

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
