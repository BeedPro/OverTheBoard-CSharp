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

}