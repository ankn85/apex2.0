using System.ComponentModel.DataAnnotations;

namespace Apex.Websites.ViewModels.Emails
{
    public sealed class UpdateEmailAccountViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        public string DisplayName { get; set; }

        [Required]
        public string Host { get; set; }

        public int Port { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool EnableSsl { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool IsDefaultEmailAccount { get; set; }
    }
}
