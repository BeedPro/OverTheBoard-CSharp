using System.ComponentModel.DataAnnotations;

namespace OverTheBoard.WebUI.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Compare("EmailAddress")]
        public string ConfirmEmailAddress { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
    }
}