using Apex.Data.Entities.Accounts;
using System;

namespace Apex.Data.Entities.Logs
{
    public class ActivityLog : BaseEntity
    {
        public DateTime CreatedOn { get; set; }

        public string ObjectFullName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string IP { get; set; }

        public int ActivityLogTypeId { get; set; }

        public virtual ActivityLogType ActivityLogType { get; set; }

        public int ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
