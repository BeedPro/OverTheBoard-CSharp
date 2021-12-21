using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.Models
{
    public class SettingsViewModel
    {
        public string DisplayImagePath { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
