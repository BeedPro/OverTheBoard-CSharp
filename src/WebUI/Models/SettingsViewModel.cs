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
        [StringLength(50, ErrorMessage = "The Display Name value cannot exceed 50 characters. ")]
        public string DisplayName { get; set; }
        public string DisplayNameId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [StringLength(100, ErrorMessage = "The new Password value cannot exceed 100 characters. ")]
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
