using Apex.Data.Entities.Logs;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;

namespace Apex.Data.Entities.Accounts
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
            : base()
        {
        }

        public ApplicationUser(string userName)
            : base(userName)
        {
        }

        public string FullName { get; set; }

        public byte Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Address { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
