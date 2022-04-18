using System.ComponentModel.DataAnnotations;

namespace OverTheBoard.WebUI.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "The Email value cannot exceed 255 characters. ")]
        public string EmailAddress { get; set; }

        [Compare("EmailAddress")]
        [StringLength(255, ErrorMessage = "The Email value cannot exceed 255 characters. ")]
        public string ConfirmEmailAddress { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The Display Name value cannot exceed 50 characters. ")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Password value cannot exceed 100 characters. ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
    }
}