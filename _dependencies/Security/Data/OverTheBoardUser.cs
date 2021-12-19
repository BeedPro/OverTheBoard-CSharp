using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OverTheBoard.Core.Security.Data
{
    public class OverTheBoardUser : IdentityUser
    {
        [StringLength(100)]
        public string DisplayName { get; set; }
        [StringLength(100)]
        public string DisplayImagePath { get; set; }
    }
}
