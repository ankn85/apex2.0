using System.ComponentModel.DataAnnotations;
using System;

namespace Apex.Websites.ViewModels.Users
{
    public class UpdateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string FullName { get; set; }

        public byte Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Address { get; set; }

        public bool Locked { get; set; }

        public string[] RoleNames { get; set; }
    }
}
