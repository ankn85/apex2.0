using System.ComponentModel.DataAnnotations;

namespace Apex.Websites.ViewModels.Users
{
    public sealed class ResetUserPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
