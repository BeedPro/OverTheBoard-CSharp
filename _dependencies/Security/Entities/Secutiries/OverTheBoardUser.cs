using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using OverTheBoard.Data.Entities.Applications;

namespace OverTheBoard.Data.Entities
{
    public class OverTheBoardUser : IdentityUser
    {
        [StringLength(100)]
        public string DisplayName { get; set; }
        [StringLength(100)]
        public string DisplayImagePath { get; set; }
        [StringLength(100)]
        public string DisplayNameId { get; set; }
        public int Rating { get; set; }
        public UserRank Rank { get; set; }
    }
}
