using System.Collections.Generic;
using System;

namespace Apex.Websites.ViewModels.Users
{
    public sealed class UserDto
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public byte Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Address { get; set; }

        public string Locked { get; set; }

        public IList<string> RoleNames { get; set; }
    }
}
