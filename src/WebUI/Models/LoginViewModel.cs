using System.ComponentModel.DataAnnotations;

namespace OverTheBoard.WebUI.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }

    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Compare("EmailAddress")]
        public string ConfirmEmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
    }
}