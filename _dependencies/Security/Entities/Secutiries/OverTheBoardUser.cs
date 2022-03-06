using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

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
    }
}
