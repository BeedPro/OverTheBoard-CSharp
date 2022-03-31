using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.Data.Entities;

namespace OverTheBoard.Infrastructure.Services
{
    public interface IUserService
    {
        Task<OverTheBoardUser> GetUserDisplayNameAndyNameIdAsync(string displayName, string number);
        Task<OverTheBoardUser> GetUserAsync(string userId);
        Task<bool> UpdateUserRankAsync(string userId, UserRank rank);
    }
}
